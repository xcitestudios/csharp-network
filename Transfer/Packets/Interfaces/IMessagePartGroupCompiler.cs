namespace com.xcitestudios.Network.Transfer.Packets.Interfaces
{
    /// <summary>
    /// Interface for adding messages together into a single group.
    /// </summary>
    public interface IMessagePartGroupCompiler
    {
        /// <summary>
        /// Check if all packets in this group have been received.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Return an array of missing packet indices in this group.
        /// </summary>
        uint[] MissingPacketIndices { get; }

        /// <summary>
        /// Add a message into this group if it matches the group.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool AddMessage(IMessagePart message);
    }
}
