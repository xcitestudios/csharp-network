namespace com.xcitestudios.Network.Transfer.Packets.Interfaces
{
    /// <summary>
    /// A byte[] message that is part of a larger set of bytes and therefore ordered.
    /// </summary>
    public interface IOrderedMessage
    {
        /// <summary>
        /// Index of the item in the group.
        /// </summary>
        uint Index { get; set; }

        /// <summary>
        /// Size of the message ignoring padding.
        /// </summary>
        uint MessageLength { get; set; }

        /// <summary>
        /// Raw message data.
        /// </summary>
        byte[] Message { get; set; }
    }
}
