namespace com.xcitestudios.Network.Server.Configuration
{
    using com.xcitestudios.Network.Server.Configuration.Interfaces;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Implementation of the <see cref="I:com.xcitestudios.Network.Server.Configuration.Interfaces.IAMQPServerConfigurationSerializable"/> interface with serialization support.
    /// </summary>
    [DataContract]
    [Serializable]
    public class AMQPServerConfiguration : UsernameAuthenticatedServerConfiguration, IAMQPServerConfigurationSerializable
    {
        /// <summary>
        /// Username for authentication.
        /// </summary>
        [DataMember(Name="vhost")]
        public string VHost { get; set; }

        /// <summary>
        /// Connection time out.
        /// </summary>
        [DataMember(Name="connectionTimeout")]
        public int ConnectionTimeout { get; set; }

        /// <summary>
        /// Updates the element implementing this interface using a JSON representation. 
        /// This means updating the state of this object with that defined in the JSON 
        /// as opposed to returning a new instance of this object.
        /// </summary>
        /// <param name="jsonString">Representation of the object</param>
        public new void DeserializeJSON(string jsonString)
        {
            var newObj = Deserialize<AMQPServerConfiguration>(jsonString);

            Host = newObj.Host;
            Port = newObj.Port;
            SSL = newObj.SSL;
            TLS = newObj.TLS;

            Username = newObj.Username;
            Password = newObj.Password;

            VHost = newObj.VHost;
            ConnectionTimeout = newObj.ConnectionTimeout;
        }

        /// <summary>
        /// Convert this object into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A JSON representation of this object</returns>
        public new string SerializeJSON()
        {
            return Serialize<AMQPServerConfiguration>();
        }
    }
}
