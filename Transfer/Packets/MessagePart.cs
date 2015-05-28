namespace com.xcitestudios.Network.Transfer.Packets
{
    using com.xcitestudios.Generic.Interfaces;
    using com.xcitestudios.Network.Transfer.Packets.Interfaces;
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// A Message or part of a message that can be combined with other messages in a group to form 
    /// a complete message.
    /// </summary>
    public class MessagePart : IMessagePart, IOrderedMessage, IValid
    {
        private static SHA512Managed Sha512 = new SHA512Managed();

        /// <summary>
        /// Index of the item in the group.
        /// </summary>
        public uint Index { get; set; }

        /// <summary>
        /// Size of the message ignoring padding.
        /// </summary>
        public uint MessageLength { get; set; }

        /// <summary>
        /// Raw message data.
        /// </summary>
        public byte[] Message { get; set; }

        /// <summary>
        /// SHA512(Group + GroupCount + GroupIndex + MessageLength + Message)
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        /// Which group of packets this packet belongs to.
        /// </summary>
        public uint Group { get; set; }

        /// <summary>
        /// How many packets are in this group.
        /// </summary>
        public uint GroupCount { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MessagePart()
        {
            Signature = new byte[64];
        }

        /// <summary>
        /// Constructor to create a packet from a byte array. Validates the signature.
        /// </summary>
        /// <param name="completeMessage"></param>
        public MessagePart(byte[] completeMessage)
            : this()
        {
            LoadBytes(completeMessage);
        }

        /// <summary>
        /// Check if the message appears to be what it says it is.
        /// </summary>
        public bool IsValid()
        {
            return ComputeHash().SequenceEqual(Signature);
        }

        /// <summary>
        /// Compute the hash of this packet (used for Signature).
        /// </summary>
        /// <returns></returns>
        public byte[] ComputeHash()
        {
            var fullPacket = GetBytes();
            var noHash = new byte[fullPacket.Length - 64];

            Buffer.BlockCopy(fullPacket, 64, noHash, 0, noHash.Length);

            return Sha512.ComputeHash(noHash);
        }

        /// <summary>
        /// Returns a byte array that defines this message part.
        /// </summary>
        public byte[] GetBytes()
        {
            var compiledBytes = new byte[80 + MessageLength];

            Buffer.BlockCopy(Signature, 0, compiledBytes, 0, 64);

            BitConverter.GetBytes(Group).CopyTo(compiledBytes, 64);
            BitConverter.GetBytes(GroupCount).CopyTo(compiledBytes, 68);
            BitConverter.GetBytes(Index).CopyTo(compiledBytes, 72);
            BitConverter.GetBytes(MessageLength).CopyTo(compiledBytes, 76);

            Buffer.BlockCopy(Message, 0, compiledBytes, 80, Message.Length);

            return compiledBytes;
        }

        /// <summary>
        /// Update this object with the bytes specified.
        /// </summary>
        public void LoadBytes(byte[] bytes)
        {
            if (bytes.Length < 80)
            {
                throw new IndexOutOfRangeException("Byte array must be at least 80 bytes long");
            }

            Buffer.BlockCopy(bytes, 0, Signature, 0, 64);
            Group = BitConverter.ToUInt32(bytes, 64);
            GroupCount = BitConverter.ToUInt32(bytes, 68);
            Index = BitConverter.ToUInt32(bytes, 72);
            MessageLength = BitConverter.ToUInt32(bytes, 76);

            if (MessageLength + 80 > bytes.Length)
            {
                throw new IndexOutOfRangeException("Given message length is greater than the byte array actual length.");
            }

            Message = new byte[MessageLength];

            if (MessageLength > 0)
            {
                Buffer.BlockCopy(bytes, 80, Message, 0, (int)MessageLength);
            }

            if (!ComputeHash().SequenceEqual(Signature))
            {
                throw new InvalidCastException("Could not convert bytes to a message, signatures do not match.");
            }
        }
    }
}
