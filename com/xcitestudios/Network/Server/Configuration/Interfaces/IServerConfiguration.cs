namespace com.xcitestudios.Network.Server.Configuration.Interfaces
{
    using System;

    /// <summary>
    /// Simple configuration for connecting to a server.
    /// </summary>
    public interface IServerConfiguration
    {
        /// <summary>
        /// Hostname/IPv4/IPv6 address to connect.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Port to connect to.
        /// </summary>
        UInt16 Port {get; set; }

        /// <summary>
        /// SSL should/shouldn't be used.
        /// </summary>
        bool SSL { get; set; }

        /// <summary>
        /// TLS should/shouldn't be used.
        /// </summary>
        bool TLS { get; set; }
    }
}
