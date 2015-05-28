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
    public class AMQPServerConfigurationTest
    {
        [TestMethod]
        public void TestSerializationConsistency()
        {
            var config = new AMQPServerConfiguration();
            config.VHost = "/abc123";

            var newConfig = new AMQPServerConfiguration();
            newConfig.DeserializeJSON(config.SerializeJSON());

            Assert.AreEqual(config.Username, newConfig.Username);
            Assert.AreEqual(config.Password, newConfig.Password);
        }

        [TestMethod]
        public void TestSchemaValidation()
        {
            var config = new AMQPServerConfiguration();
            config.Host = "192.168.5.1";
            config.Port = 32755;
            config.SSL = true;
            config.TLS = true;
            config.Username = "abc";
            config.Password = "123";
            config.VHost = "/abc123";

            var json = config.SerializeJSON();

            var resolver = new JsonSchemaResolver();
            string textSchema;

            using (var client = new WebClient())
            {
                textSchema = client.DownloadString("https://cdn.rawgit.com/xcitestudios/json-schemas/1.0.2/com/xcitestudios/schemas/Network/Server/Configuration/AMQPServerConfiguration.json");
            }

            var schema = JSchema.Parse(textSchema, new JSchemaReaderSettings {
                Resolver = new JSchemaUrlResolver(),
                BaseUri = new Uri("https://cdn.rawgit.com/xcitestudios/json-schemas/1.0.2/com/xcitestudios/schemas/Network/Server/Configuration/")
            });

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