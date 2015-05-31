namespace com.xcitestudios.Network.Transfer.Packets.Interfaces
{
    /// <summary>
    /// A byte[] message that is part of a larger set of bytes and therefore ordered.
    /// </summary>
    public interface IOrderedMessageAsByteArray: IOrderedMessage, IAsByteArray
    {
    }
}
