namespace com.xcitestudios.Network.Server.Configuration
{
    using com.xcitestudios.Generic.Data.Manipulation;
    using com.xcitestudios.Network.Server.Configuration.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Implementation of the <see cref="I:com.xcitestudios.Network.Server.Configuration.Interfaces.IServerConfigurationSerializable"/> interface with serialization support.
    /// </summary>
    [DataContract]
    public class GearmanServerConfiguration : ServerConfiguration, IGearmanServerConfigurationSerializable
    {
    }
}
