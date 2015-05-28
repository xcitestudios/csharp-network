namespace com.xcitestudios.Network.Transfer.Packets.Interfaces
{
    /// <summary>
    /// Representation of a message to be sent via UDP.
    /// </summary>
    public interface IMessagePart : IOrderedMessage, IAsByteArray
    {
        /// <summary>
        /// Which group of packets this packet belongs to.
        /// </summary>
        uint Group { get; set; }

        /// <summary>
        /// How many packets are in this group.
        /// </summary>
        uint GroupCount { get; set; }

        /// <summary>
        /// SHA512(Group + GroupCount + GroupIndex + MessageLength + Message)
        /// </summary>
        byte[] Signature { get; set; }

        /// <summary>
        /// Compute the hash of this packet (used for Signature).
        /// </summary>
        /// <returns></returns>
        byte[] ComputeHash();
    }
}
