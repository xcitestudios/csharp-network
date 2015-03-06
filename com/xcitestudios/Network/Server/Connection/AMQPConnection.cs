namespace com.xcitestudios.Network.Server.Connection
{
    using com.xcitestudios.Network.Server.Configuration;
    using RabbitMQ.Client;
    using System.Collections.Generic;

    /// <summary>
    /// Helper to connect to AMQP servers.
    /// </summary>
    public class AMQPConnection
    {
        private static Dictionary<string, object> Connections = new Dictionary<string, object>();

        /// <summary>
        /// Create a connection using the RabbitMQ library
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>RabbitMQ.Client.IConnection</returns>
        public static IConnection createConnectionUsingRabbitMQ(AMQPServerConfiguration configuration)
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

                Connections["RabbitMQ" + configuration.SerializeJSON()] = factory;
            }


            return (Connections["RabbitMQ" + configuration.SerializeJSON()] as ConnectionFactory).CreateConnection();
        }
    }
}
