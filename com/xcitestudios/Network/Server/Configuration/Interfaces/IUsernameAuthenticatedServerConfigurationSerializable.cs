namespace com.xcitestudios.Network.Server.Configuration.Interfaces
{
    using com.xcitestudios.Generic.Data.Manipulation.Interfaces;

    /// <summary>
    /// Extend server configuration to add in authentication with serialization.
    /// </summary>
    public interface IUsernameAuthenticatedServerConfigurationSerializable : IUsernameAuthenticatedServerConfiguration, ISerialization
    {
    }
}
