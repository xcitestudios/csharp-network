namespace tests.com.xcitestudios.Network.Server.Configuration
{
    using global::com.xcitestudios.Network.Transfer;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class PacketTest
    {
        [TestMethod]
        public void TestToFromBytesEquals()
        {
            string longMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus pretium ultrices arcu in mattis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Proin et nulla eu tellus condimentum cursus. In et auctor dui. Quisque at nulla sit amet metus vestibulum sagittis. In et pulvinar nunc, quis ultrices lorem. Etiam et massa ut dui dictum lobortis ut venenatis velit. Vivamus a odio eget ante sodales pulvinar. Donec nec sapien eu massa suscipit tempus. In elementum, enim vel finibus efficitur, dolor augue venenatis ligula, vitae ultrices metus felis at odio. Sed ligula mauris, egestas pretium dui ut, sagittis blandit nulla. Morbi tincidunt lorem volutpat maximus euismod. Nullam sem nibh, efficitur eu vestibulum at, accumsan vitae ligula. Aenean tristique eu diam a accumsan. Aenean pharetra dictum lorem viverra mattis. Nulla facilisi.";
            var bytes = System.Text.Encoding.UTF8.GetBytes(longMessage);

            var packet1 = new Packet() { 
                Message = bytes,
                Index = 5,
                Source = new byte[] {  192, 168, 0, 1 }
            };


            var packet2 = new Packet(packet1.GetBytes());

            Assert.AreEqual(packet1.Index, packet2.Index);
            Assert.AreEqual(packet1.MessageLength, packet2.MessageLength);
            Assert.AreEqual(packet1.SourceLength, packet2.SourceLength);

            Assert.IsTrue(packet1.Message.SequenceEqual(packet2.Message));
            Assert.IsTrue(packet1.Source.SequenceEqual(packet2.Source));
        }
    }
}
