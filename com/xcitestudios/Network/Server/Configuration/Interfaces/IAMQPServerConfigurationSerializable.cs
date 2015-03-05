namespace com.xcitestudios.Network.Server.Configuration.Interfaces
{
    using com.xcitestudios.Generic.Data.Manipulation.Interfaces;

    /// <summary>
    /// Extend server configuration to support AMQP with serialization.
    /// </summary>
    public interface IAMQPServerConfigurationSerializable : IAMQPServerConfiguration, ISerialization
    {
    }
}
