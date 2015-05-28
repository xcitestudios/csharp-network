namespace tests.com.xcitestudios.Network.Server.Configuration
{
    using global::com.xcitestudios.Network.Server.Configuration;
    using global::com.xcitestudios.Network.Server.Connection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AMQPConnectionTest
    {
        [TestMethod]
        public void TestAMQPConnectionWithSSL()
        {
            var configuration = new AMQPServerConfiguration();
            configuration.Host = "33.0.0.101";
            configuration.Port = 5671;
            configuration.VHost = "/";
            configuration.Username = "guest";
            configuration.Password = "guest";
            configuration.SSL = true;

            var conn = AMQPConnection.createConnectionUsingRabbitMQ(configuration, null, "/tmp/keycert.p12", "password");
        }
    }
}
