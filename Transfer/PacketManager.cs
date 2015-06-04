using com.xcitestudios.Network.Transfer.Packets;
using com.xcitestudios.Network.Transfer.Packets.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.xcitestudios.Network.Transfer
{
    public class UDPEncryptedPacketSend : IDisposable
    {
        private uint CurrentIndex = 0;

        private uint CurrentPacketSize = 1024;

        private MemoryStream ByteBuffer;
        private CryptoStream EncryptionStream;

        private Task Sender;
        private Timer Pinger;

        private EventWaitHandle WaitBytes;

        private int PingInterval = 5;

        private byte[] Source;

        private MemoryStream EncryptedBuffer;

        private List<Packet> SentPackets;

        public UDPEncryptedPacketSend(ICryptoTransform encryptor)
        {
            Source = new byte[] { 192, 168, 0, 1 };

            Sender = Task.Factory.StartNew(PacketSender, TaskCreationOptions.LongRunning);
            Pinger = new Timer(Ping, this, PingInterval * 1000, PingInterval * 1000);
            WaitBytes = new EventWaitHandle(false, EventResetMode.ManualReset);

            ByteBuffer = new MemoryStream();
            EncryptedBuffer = new MemoryStream();
            EncryptionStream = new CryptoStream(ByteBuffer, encryptor, CryptoStreamMode.Write);

            SentPackets = new List<Packet>();

        }

        private void Ping(object state)
        {
            var pingPacket = new byte[CurrentPacketSize];
            Encoding.UTF8.GetBytes("PING!").CopyTo(pingPacket, 0);

            SendBytes(pingPacket);
        }

        private void PacketSender()
        {
            while (true)
            {
                WaitBytes.WaitOne();

                lock (EncryptedBuffer)
                {
                    while(EncryptedBuffer.Length > CurrentPacketSize)
                    {
                        var nextPacket = new byte[CurrentPacketSize - 12 - Source.Length];

                        EncryptedBuffer.Read(nextPacket, 0, nextPacket.Length);

                        var packet = new Packet() { 
                            Source = Source,
                            Message = nextPacket,
                            Index = CurrentIndex
                        };

                        SendPacket(packet);

                    }
                }
            }
        }

        private void SendPacket(Packet packet)
        {
            SentPackets.Add(packet);
        }

        public void SendBytes(byte[] bytes)
        {
            lock (ByteBuffer)
            {
                EncryptionStream.Write(bytes, 0, bytes.Length);

                var encryptedBytes = ByteBuffer.ToArray();
                ByteBuffer.Seek(0, SeekOrigin.Begin);
                ByteBuffer.SetLength(0);

                lock (EncryptedBuffer)
                {
                    EncryptedBuffer.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
            }

            WaitBytes.Set();
        }

        public void Dispose()
        {
            EncryptionStream.FlushFinalBlock();

            WaitBytes.Set();

            Thread.Sleep(100);

            Sender.Dispose();
            EncryptionStream.Dispose();
        }

        public class PacketReceivedEventArgs : EventArgs
        {
            public PacketManager Manager { get; private set; }

            public IOrderedMessage Packet { get; private set; }

            public PacketReceivedEventArgs(PacketManager manager, IOrderedMessage packet)
            {
                Manager = manager;
                Packet = packet;
            }
        }

        public class PacketManager
        {
            public event EventHandler<PacketReceivedEventArgs> PacketReceived;

            private Dictionary<uint, IOrderedMessage> Packets = new Dictionary<uint, IOrderedMessage>();

            public void AddPacket(IOrderedMessage packet)
            {
                Packets[packet.Index] = packet;
                OnPacketReceived(packet);
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

        public class EncryptedPacketFilter
        {
            private uint DecryptionStartIndex = 0;

            private ICryptoTransform Transformer;

            private Dictionary<uint, PacketReceivedEventArgs> EncryptedPackets { get; set; }

            private Dictionary<uint, IOrderedMessage> DecryptedPackets { get; set; }

            private Dictionary<uint, uint> DecryptedSizes { get; set; }

            private MemoryStream MemoryStream;

            private CryptoStream CryptoStream;

            public EncryptedPacketFilter(PacketManager manager, ICryptoTransform transformer)
            {
                Transformer = transformer;

                MemoryStream = new MemoryStream(128);
                CryptoStream = new CryptoStream(MemoryStream, Transformer, CryptoStreamMode.Write);

                DecryptedPackets = new Dictionary<uint, IOrderedMessage>();
                EncryptedPackets = new Dictionary<uint, PacketReceivedEventArgs>();
                DecryptedSizes = new Dictionary<uint, uint>();

                manager.PacketReceived += PacketReceived;
            }

            private void PacketReceived(object sender, PacketReceivedEventArgs e)
            {
                /*if(DecryptionStartIndex > e.Packet.Index)
                {
                    // Duplicate packet below what we've already handled, ignore it.
                    return;
                }*/

                EncryptedPackets[e.Packet.Index] = e;

                Decrypt();
            }

            private void Decrypt()
            {
                if (EncryptedPackets.ContainsKey(DecryptionStartIndex))
                {
                    DecryptIndex(DecryptionStartIndex);
                    DecryptionStartIndex++;

                    Decrypt();
                }
            }

            private void DecryptIndex(uint index)
            {
                var packet = EncryptedPackets[index].Packet;

                MemoryStream.Seek(0, SeekOrigin.Begin);
                MemoryStream.SetLength(0);

                CryptoStream.Write(packet.Message, 0, (int)packet.MessageLength);
                CryptoStream.Flush();

                var decrypted = MemoryStream.ToArray();

                var newPacket = new MessagePart(decrypted);

                DecryptedPackets[index] = newPacket;
                EncryptedPackets.Remove(index);
            }
        }
    }
}
