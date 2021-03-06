﻿namespace com.xcitestudios.Network.Server.Configuration.Interfaces
{
    using com.xcitestudios.Generic.Data.Manipulation.Interfaces;

    /// <summary>
    /// Simple configuration for connecting to a server with serialization.
    /// </summary>
    public interface IServerConfigurationSerializable : IServerConfiguration, ISerialization
    {
    }
}
