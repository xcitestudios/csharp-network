namespace com.xcitestudios.Network.Server.Configuration
{
    using com.xcitestudios.Network.Server.Configuration.Interfaces;
    using System.Runtime.Serialization;

    /// <summary>
    /// Implementation of the <see cref="I:com.xcitestudios.Network.Server.Configuration.Interfaces.IUsernameAuthenticatedServerConfigurationSerializable"/> interface with serialization support.
    /// </summary>
    [DataContract]
    public class UsernameAuthenticatedServerConfiguration : ServerConfiguration, IUsernameAuthenticatedServerConfigurationSerializable
    {
        /// <summary>
        /// Username for authentication.
        /// </summary>
        [DataMember(Name="username")]
        public string Username { get; set; }

        /// <summary>
        /// Password for authentication.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }

        /// <summary>
        /// Updates the element implementing this interface using a JSON representation. 
        /// This means updating the state of this object with that defined in the JSON 
        /// as opposed to returning a new instance of this object.
        /// </summary>
        /// <param name="jsonString">Representation of the object</param>
        public new void DeserializeJSON(string jsonString)
        {
            var newObj = Deserialize<UsernameAuthenticatedServerConfiguration>(jsonString);

            Host = newObj.Host;
            Port = newObj.Port;
            SSL = newObj.SSL;
            TLS = newObj.TLS;

            Username = newObj.Username;
            Password = newObj.Password;
        }

        /// <summary>
        /// Convert this object into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A JSON representation of this object</returns>
        public new string SerializeJSON()
        {
            return Serialize<UsernameAuthenticatedServerConfiguration>();
        }
    }
}
