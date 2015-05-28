namespace com.xcitestudios.Network.Server.Configuration.Interfaces
{
    /// <summary>
    /// Extend server configuration to add in authentication.
    /// </summary>
    public interface IUsernameAuthenticatedServerConfiguration : IServerConfiguration
    {
        /// <summary>
        /// Username for authentication.
        /// </summary>
        string Username { get; set; }
        
        /// <summary>
        /// Password for authentication.
        /// </summary>
        string Password { get; set; }
    }
}
