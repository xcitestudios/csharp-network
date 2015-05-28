namespace com.xcitestudios.Network.Transfer.Packets.Interfaces
{
    /// <summary>
    /// Interface for storing a group of related messages.
    /// </summary>
    public interface IMessagePartGroup
    {
        /// <summary>
        /// Enumerate the message to compile the complete message.
        /// </summary>
        byte[] CompleteMessage { get; }

        /// <summary>
        /// Which group of packets this packet belongs to.
        /// </summary>
        uint Group { get; }

        /// <summary>
        /// How many packets are in this group.
        /// </summary>
        uint GroupCount { get; }

        /// <summary>
        /// Messages relating to this group.
        /// </summary>
        IMessagePart[] Messages { get; }
    }
}
