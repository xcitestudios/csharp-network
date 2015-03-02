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
    public class ServerConfiguration : JsonSerializationHelper, IServerConfigurationSerializable
    {
        /// <summary>
        /// Hostname/IPv4/IPv6 address to connect.
        /// </summary>
        [DataMember(Name="host")]
        public string Host { get; set;  }

        /// <summary>
        /// Port to connect to.
        /// </summary>
        [DataMember(Name = "port")]
        public UInt16 Port { get; set; }

        /// <summary>
        /// SSL should/shouldn't be used.
        /// </summary>
        [DataMember(Name = "ssl")]
        public bool SSL { get; set; }

        /// <summary>
        /// TLS should/shouldn't be used.
        /// </summary>
        [DataMember(Name = "tls")]
        public bool TLS { get; set; }

        /// <summary>
        /// Updates the element implementing this interface using a JSON representation. 
        /// This means updating the state of this object with that defined in the JSON 
        /// as opposed to returning a new instance of this object.
        /// </summary>
        /// <param name="jsonString">Representation of the object</param>
        public void DeserializeJSON(string jsonString)
        {
            var newObj = Deserialize<ServerConfiguration>(jsonString);

            Host = newObj.Host;
            Port = newObj.Port;
            SSL = newObj.SSL;
            TLS = newObj.TLS;
        }

        /// <summary>
        /// Convert this object into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A JSON representation of this object</returns>
        public string SerializeJSON()
        {
            return Serialize<ServerConfiguration>();
        }
    }
}
