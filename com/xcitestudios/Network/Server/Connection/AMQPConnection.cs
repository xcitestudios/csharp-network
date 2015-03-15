namespace com.xcitestudios.Network.Server.Connection
{
    using com.xcitestudios.Network.Server.Configuration;
    using RabbitMQ.Client;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Helper to connect to AMQP servers.
    /// </summary>
    public class AMQPConnection
    {
        private static Dictionary<string, object> Connections = new Dictionary<string, object>();

        /// <summary>
        /// Create a connection using the RabbitMQ library
        /// </summary>
        /// <param name="configuration">Configuration for the connection.</param>
        /// <param name="sslServerName">Only used when configuration.SSL is true. Override the expected server name in the SSL certificate, otherwise use configuration.Host</param>
        /// <param name="sslCertificatePath">Only used when configuration.SSL is true. Path to certificate to use for the connection if not in key store</param>
        /// <param name="sslCertificatePassphrase">Only used when configuration.SSL is true and sslCertificatePath file is specified. Passphrase to decrypt the sslCertificatePath file</param>
        /// <returns>RabbitMQ.Client.IConnection</returns>
        public static IConnection createConnectionUsingRabbitMQ(AMQPServerConfiguration configuration, string sslServerName = null, string sslCertificatePath = null, string sslCertificatePassphrase = null)
        {
            if (!Connections.ContainsKey("RabbitMQ" + configuration.SerializeJSON()))
            {
                var factory = new ConnectionFactory();
                factory.HostName = configuration.Host;
                factory.Port = configuration.Port;
                factory.UserName = configuration.Username;
                factory.Password = configuration.Password;
                factory.VirtualHost = configuration.VHost;
                factory.Protocol = Protocols.AMQP_0_9_1;

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

                Connections["RabbitMQ" + configuration.SerializeJSON()] = factory;
            }


            return (Connections["RabbitMQ" + configuration.SerializeJSON()] as ConnectionFactory).CreateConnection();
        }
    }
}
