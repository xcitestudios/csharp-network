namespace com.xcitestudios.Network.Server.Connection
{
    using com.xcitestudios.Network.Server.Configuration;
    using Twingly.Gearman;
    using System.Collections.Generic;

    /// <summary>
    /// Helper to connect to Gearman servers.
    /// </summary>
    public class GearmanConnection
    {
        private static Dictionary<string, object> Connections = new Dictionary<string, object>();

        /// <summary>
        /// Create a Gearman client connection.
        /// </summary>
        /// <param name="configurations"></param>
        /// <returns>GearmanClient with servers added</returns>
        public static GearmanClient createClientConnectionUsingGearman(GearmanServerConfiguration[] configurations)
        {
            var client = new GearmanClient();

            foreach (var config in configurations)
            {
                client.AddServer(config.Host, config.Port);
            }

            return client;
        }

        /// <summary>
        /// Create a Gearman worker connection.
        /// </summary>
        /// <param name="configurations"></param>
        /// <returns>GearmanWorker with servers added</returns>
        public static GearmanWorker createWorkerConnectionUsingGearman(GearmanServerConfiguration[] configurations)
        {
            var worker = new GearmanWorker();

            foreach (var config in configurations)
            {
                worker.AddServer(config.Host, config.Port);
            }

            return worker;
        }
    }
}
