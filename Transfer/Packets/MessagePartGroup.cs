namespace com.xcitestudios.Network.Transfer.Packets
{
    using com.xcitestudios.Network.Transfer.Packets.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A group of UDPMessages related to each other.
    /// </summary>
    public class MessagePartGroup : IMessagePartGroup, IMessagePartGroupCompiler
    {
        /// <summary>
        /// Which group of packets this packet belongs to.
        /// </summary>
        public uint Group { get; private set; }

        /// <summary>
        /// How many packets are in this group.
        /// </summary>
        public uint GroupCount { get; private set; }

        /// <summary>
        /// Packets relating to this group.
        /// </summary>
        public IMessagePart[] Messages { get; private set; }

        /// <summary>
        /// Check if all packets in this group have been received.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                for (var i = 0; i < GroupCount; i++)
                {
                    if (Messages[i] == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Return an array of missing packet indices in this group.
        /// </summary>
        public uint[] MissingPacketIndices
        {
            get
            {
                List<uint> missing = new List<uint>();

                for (uint i = 0; i < GroupCount; i++)
                {
                    if (Messages[i] == null)
                    {
                        missing.Add(i);
                    }
                }

                return missing.ToArray();
            }
        }

        /// <summary>
        /// Enumerate the message to compile the complete message
        /// </summary>
        public byte[] CompleteMessage
        {
            get
            {
                if (!IsComplete)
                {
                    throw new OverflowException("Message is incomplete and cannot be read");
                }

                byte[] completeMessage;
                uint totalSize = 0;
                int offset = 0;

                for (var i = 0; i < GroupCount; i++)
                {
                    totalSize += Messages[i].MessageLength;
                }

                completeMessage = new byte[totalSize];

                for (var i = 0; i < GroupCount; i++)
                {
                    Buffer.BlockCopy(Messages[i].Message, 0, completeMessage, offset, (int)Messages[i].MessageLength);

                    offset += (int)Messages[i].MessageLength;
                }

                return completeMessage;
            }
        }

        /// <summary>
        /// Constructor for creating a group from a messsage to be further added to.
        /// </summary>
        /// <param name="oneMessage"></param>
        public MessagePartGroup(IMessagePart oneMessage)
        {
            Group = oneMessage.Group;
            GroupCount = oneMessage.GroupCount;

            Messages = new MessagePart[GroupCount];

            Messages[oneMessage.Index] = oneMessage;
        }

        /// <summary>
        /// Constructor for creating a complete set of messages in a group.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="groupSize"></param>
        /// <param name="completeMessage"></param>
        public MessagePartGroup(uint group, uint groupSize, byte[] completeMessage)
        {
            Group = group;
            GroupCount = (uint)((completeMessage.Length + groupSize - 1) / groupSize);
            Messages = new MessagePart[GroupCount];

            int offset = 0;
            for (uint i = 0; i < GroupCount; i++)
            {
                int length = (int)(offset + groupSize < completeMessage.Length ? offset + groupSize : completeMessage.Length - offset);
                byte[] message = new byte[length];

                Buffer.BlockCopy(completeMessage, offset, message, 0, length);

                offset += (int)length;

                Messages[i] = new MessagePart()
                {
                    Group = Group,
                    GroupCount = GroupCount,
                    Index = i,
                    Message = message,
                    MessageLength = (uint)length
                };

                Messages[i].Signature = Messages[i].ComputeHash();
            }
        }

        /// <summary>
        /// Add a message into this group if it matches the group.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AddMessage(IMessagePart message)
        {
            if (IsComplete)
            {
                throw new OverflowException("Group is complete");
            }

            if (message.Group == this.Group && message.GroupCount == this.GroupCount)
            {
                Messages[message.Index] = message;
                return true;
            }

            return false;
        }
    }
}
