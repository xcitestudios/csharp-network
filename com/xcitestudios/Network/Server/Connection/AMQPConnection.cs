namespace com.xcitestudios.Network.Server.Connection
{
    using com.xcitestudios.Network.Server.Configuration.Interfaces;
    using RabbitMQ.Client;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Helper to connect to AMQP servers.
    /// </summary>
    public class AMQPConnection
    {
        private static Dictionary<string, ConnectionFactory> ConnectionFactories = new Dictionary<string, ConnectionFactory>();
        private static Dictionary<string, IConnection> Connections = new Dictionary<string, IConnection>();

        /// <summary>
        /// Create a connection using the RabbitMQ library, a new connection is always returned.
        /// </summary>
        /// <param name="configuration">Configuration for the connection.</param>
        /// <param name="sslServerName">Only used when configuration.SSL is true. Override the expected server name in the SSL certificate, otherwise use configuration.Host</param>
        /// <param name="sslCertificatePath">Only used when configuration.SSL is true. Path to certificate to use for the connection if not in key store</param>
        /// <param name="sslCertificatePassphrase">Only used when configuration.SSL is true and sslCertificatePath file is specified. Passphrase to decrypt the sslCertificatePath file</param>
        /// <returns>RabbitMQ.Client.IConnection</returns>
        public static IConnection createConnectionUsingRabbitMQ(IAMQPServerConfigurationSerializable configuration, string sslServerName = null, string sslCertificatePath = null, string sslCertificatePassphrase = null)
        {
            var factory = SetupFactory(configuration, sslServerName, sslCertificatePath, sslCertificatePassphrase);

            lock (factory)
            {
                return factory.CreateConnection();
            }
        }

        /// <summary>
        /// Create a connection using the RabbitMQ library or reuse a connection if it already exists and is open.
        /// </summary>
        /// <param name="configuration">Configuration for the connection.</param>
        /// <param name="sslServerName">Only used when configuration.SSL is true. Override the expected server name in the SSL certificate, otherwise use configuration.Host</param>
        /// <param name="sslCertificatePath">Only used when configuration.SSL is true. Path to certificate to use for the connection if not in key store</param>
        /// <param name="sslCertificatePassphrase">Only used when configuration.SSL is true and sslCertificatePath file is specified. Passphrase to decrypt the sslCertificatePath file</param>
        /// <returns>RabbitMQ.Client.IConnection</returns>
        public static IConnection createOrReuseConnectionUsingRabbitMQ(IAMQPServerConfigurationSerializable configuration, string sslServerName = null, string sslCertificatePath = null, string sslCertificatePassphrase = null)
        {
            var keyHash = Encoding.ASCII.GetString((new SHA1CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(configuration.SerializeJSON())));

            lock (Connections)
            {
                if (!Connections.ContainsKey(keyHash) || !Connections[keyHash].IsOpen)
                {
                    Connections[keyHash] = createConnectionUsingRabbitMQ(configuration, sslServerName, sslCertificatePath, sslCertificatePassphrase);
                }

                return Connections[keyHash];
            }
        }

        private static ConnectionFactory SetupFactory(IAMQPServerConfigurationSerializable configuration, string sslServerName = null, string sslCertificatePath = null, string sslCertificatePassphrase = null)
        {
            lock (ConnectionFactories)
            {
                var keyHash = Encoding.ASCII.GetString((new SHA1CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(configuration.SerializeJSON())));

                if (!ConnectionFactories.ContainsKey(keyHash))
                {
                    var factory = new ConnectionFactory();
                    factory.HostName = configuration.Host;
                    factory.Port = configuration.Port;
                    factory.UserName = configuration.Username;
                    factory.Password = configuration.Password;
                    factory.VirtualHost = configuration.VHost;
                    factory.Protocol = Protocols.AMQP_0_9_1;
                    factory.RequestedConnectionTimeout = configuration.ConnectionTimeout * 1000;
                    factory.UseBackgroundThreadsForIO = true;
                    factory.TopologyRecoveryEnabled = true;
                    factory.AutomaticRecoveryEnabled = true;

                    if (configuration.SSL)
                    {
                        factory.Ssl.Enabled = true;
                        factory.Ssl.ServerName = sslServerName == null ? configuration.Host : sslServerName;

                        if (sslCertificatePath != null)
                        {
                            factory.Ssl.CertPath = sslCertificatePath;

                            if (sslCertificatePassphrase != null)
                            {
                                factory.Ssl.CertPassphrase = sslCertificatePassphrase;
                            }
                        }
                    }

                    ConnectionFactories[keyHash] = factory;
                }

                return ConnectionFactories[keyHash];
            }
        }
    }
}
