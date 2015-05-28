namespace com.xcitestudios.Network.Transfer
{
    using com.xcitestudios.Network.Transfer.Packets.Interfaces;
    using System;

    public class Packet : IOrderedMessage, IAsByteArray
    {
        /// <summary>
        /// Overall index of the packet.
        /// </summary>
        public uint Index { get; set; }

        /// <summary>
        /// Length of the Source identifier.
        /// </summary>
        public uint SourceLength { get; set; }

        private byte[] _Source;
        /// <summary>
        /// Identifier of the source.
        /// </summary>
        public byte[] Source
        {
            get
            {
                return _Source;
            }
            set
            {
                _Source = value;
                SourceLength = (uint)value.Length;
            }
        }

        /// <summary>
        /// Length of the message packet in total.
        /// </summary>
        public uint MessageLength { get; set; }

        private byte[] _Message;
        /// <summary>
        /// Message in this packet (can be encrypted before going in here).
        /// </summary>
        public byte[] Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                MessageLength = (uint)value.Length;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Packet()
        {
        }

        /// <summary>
        /// Constructor to create a packet from a byte array.
        /// </summary>
        /// <param name="completePacket"></param>
        public Packet(byte[] completePacket)
            : this()
        {
            LoadBytes(completePacket);
        }

        /// <summary>
        /// Returns a byte array that defines this packet.
        /// </summary>
        public byte[] GetBytes()
        {
            var compiledBytes = new byte[12 + Source.Length + Message.Length];

            BitConverter.GetBytes(Index).CopyTo(compiledBytes, 0);
            BitConverter.GetBytes(SourceLength).CopyTo(compiledBytes, 4);

            Buffer.BlockCopy(Source, 0, compiledBytes, 8, Source.Length);

            BitConverter.GetBytes(MessageLength).CopyTo(compiledBytes, 8 + Source.Length);

            Buffer.BlockCopy(Message, 0, compiledBytes, 12 + Source.Length, Message.Length);

            return compiledBytes;
        }

        /// <summary>
        /// Update this object with the bytes specified.
        /// </summary>
        public void LoadBytes(byte[] bytes)
        {
            if (bytes.Length < 12)
            {
                throw new IndexOutOfRangeException("Byte array must be at least 12 bytes long");
            }

            Index = BitConverter.ToUInt32(bytes, 0);
            SourceLength = BitConverter.ToUInt32(bytes, 4);

            if (8 + SourceLength > bytes.Length)
            {
                throw new IndexOutOfRangeException("Given source length is greater than the byte array actual length.");
            }

            Source = new byte[SourceLength];

            Buffer.BlockCopy(bytes, 8, Source, 0, (int)SourceLength);

            if (8 + SourceLength + 4 > bytes.Length)
            {
                throw new IndexOutOfRangeException("Given byte array is not long enough to represent a packet.");
            }

            MessageLength = BitConverter.ToUInt32(bytes, 8 + (int)SourceLength);

            if (12 + SourceLength + MessageLength > bytes.Length)
            {
                throw new IndexOutOfRangeException("Given message length is greater than the byte array actual length.");
            }

            Message = new byte[MessageLength];

            if (MessageLength > 0)
            {
                Buffer.BlockCopy(bytes, 12 + (int)SourceLength, Message, 0, (int)MessageLength);
            }
        }
    }
}
