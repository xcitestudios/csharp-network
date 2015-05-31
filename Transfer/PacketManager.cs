using com.xcitestudios.Network.Transfer.Packets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.xcitestudios.Network.Transfer
{
    class PacketReceivedEventArgs : EventArgs
    {
        public PacketManager Manager {get; private set;}

        public IOrderedMessage Packet {get; private set;}

        public PacketReceivedEventArgs(PacketManager manager, IOrderedMessage packet)
        {
            Manager = manager;
            Packet = packet;
        }
    }

    class PacketManager
    {
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;

        private Dictionary<uint, IOrderedMessage> Packets = new Dictionary<uint, IOrderedMessage>();

        public void AddPacket(IOrderedMessage packet)
        {
            Packets[packet.Index] = packet;
        }

        public void RemovePacket(uint Index)
        {
            Packets.Remove(Index);
        }

        protected virtual void OnPacketReceived(IOrderedMessage packet)
        {
            if (PacketReceived != null)
            {
                PacketReceived(this, new PacketReceivedEventArgs(this, packet));
            }
        }
    }

    class EncryptedPacketFilter
    {
        private uint? NextPacketIndex = 0;

        private Dictionary<uint, PacketReceivedEventArgs> Packets = new Dictionary<uint, PacketReceivedEventArgs>();

        private Dictionary<uint, IOrderedMessage> DecryptedPackets = new Dictionary<uint, IOrderedMessage>();

        public EncryptedPacketFilter(PacketManager manager)
        {
            manager.PacketReceived += PacketReceived;
        }

        private void PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            Packets[e.Packet.Index] = e;

            if(e.Packet.Index == NextPacketIndex + 1)
            {
                // how about wrap Packet and if this packet - 1 is unencrypted, do this one - recurse it
            }
        }
    }
}
