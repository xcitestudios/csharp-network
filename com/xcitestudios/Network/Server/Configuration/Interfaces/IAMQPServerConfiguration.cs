namespace com.xcitestudios.Network.Server.Configuration.Interfaces
{
    /// <summary>
    /// Extend server configuration to support AMQP.
    /// </summary>
    public interface IAMQPServerConfiguration : IUsernameAuthenticatedServerConfiguration
    {
        /// <summary>
        /// VHost to use, default should be "/".
        /// </summary>
        string VHost { get; set; }
    }
}
