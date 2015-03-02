namespace tests.com.xcitestudios.Network.Server.Configuration
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Net;
    using Newtonsoft.Json.Schema;
    using Newtonsoft.Json.Linq;
    using global::com.xcitestudios.Network.Server.Configuration;

    [TestClass]
    public class ServerConfigurationTest
    {
        [TestMethod]
        public void TestSerializationConsistency()
        {
            var config = new ServerConfiguration();
            config.Host = "192.168.5.1";
            config.Port = 32755;
            config.SSL = true;
            config.TLS = true;

            var newConfig = new ServerConfiguration();
            newConfig.DeserializeJSON(config.SerializeJSON());

            Assert.AreEqual(config.Host, newConfig.Host);
            Assert.AreEqual(config.Port, newConfig.Port);
            Assert.AreEqual(config.SSL, newConfig.SSL);
            Assert.AreEqual(config.TLS, newConfig.TLS);
        }

        [TestMethod]
        public void TestSchemaValidation()
        {
            var config = new ServerConfiguration();
            config.Host = "192.168.5.1";
            config.Port = 32755;
            config.SSL = true;
            config.TLS = true;

            var json = config.SerializeJSON();

            string textSchema;

            using (var client = new WebClient())
            {
                textSchema = client.DownloadString("https://cdn.rawgit.com/xcitestudios/json-schemas/cdn-tag-3/com/xcitestudios/schemas/Network/Server/Configuration/ServerConfiguration.json");
            }

            var schema = JSchema.Parse(textSchema);

            var jsonObject = JObject.Parse(json);

            IList<string> errorMessages = new List<string>();
            var isValid = jsonObject.IsValid(schema, out errorMessages);

            string errorMessage = "";

            if (!isValid)
            {
                for (var i = 0; i < errorMessages.Count; i++)
                {
                    errorMessage += errorMessages[i] + "\n";
                }
            }

            Assert.IsTrue(isValid, errorMessage);
        }
    }
}