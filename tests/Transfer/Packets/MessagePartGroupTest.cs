namespace tests.com.xcitestudios.Network.Server.Configuration
{
    using global::com.xcitestudios.Network.Transfer.Packets;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    [TestClass]
    public class MessagePartGroupTest
    {
        [TestMethod]
        public void TestToFromBytesEquals()
        {
            string longMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus pretium ultrices arcu in mattis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Proin et nulla eu tellus condimentum cursus. In et auctor dui. Quisque at nulla sit amet metus vestibulum sagittis. In et pulvinar nunc, quis ultrices lorem. Etiam et massa ut dui dictum lobortis ut venenatis velit. Vivamus a odio eget ante sodales pulvinar. Donec nec sapien eu massa suscipit tempus. In elementum, enim vel finibus efficitur, dolor augue venenatis ligula, vitae ultrices metus felis at odio. Sed ligula mauris, egestas pretium dui ut, sagittis blandit nulla. Morbi tincidunt lorem volutpat maximus euismod. Nullam sem nibh, efficitur eu vestibulum at, accumsan vitae ligula. Aenean tristique eu diam a accumsan. Aenean pharetra dictum lorem viverra mattis. Nulla facilisi.";
            var bytes = System.Text.Encoding.UTF8.GetBytes(longMessage);

            var group = new MessagePartGroup(1, 512, bytes);

            var groupText = System.Text.Encoding.UTF8.GetString(group.CompleteMessage);

            Assert.AreEqual(longMessage, groupText);

            var newMessages = new List<MessagePart>();

            foreach (var message in group.Messages)
            {
                var byt = message.GetBytes();
                newMessages.Add(new MessagePart(byt));
            }

            var group2 = new MessagePartGroup(newMessages[0]);

            Assert.IsFalse(group2.IsComplete);
            CollectionAssert.Contains(group2.MissingPacketIndices, (uint)1);

            group2.AddMessage(newMessages[1]);

            Assert.IsTrue(group2.IsComplete);

            Assert.AreEqual(group.GroupCount, group2.GroupCount);
            Assert.AreEqual(group.Group, group2.Group);

            groupText = System.Text.Encoding.UTF8.GetString(group2.CompleteMessage);

            Assert.AreEqual(longMessage, groupText);
        }
    }
}
