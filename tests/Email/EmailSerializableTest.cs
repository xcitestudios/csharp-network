namespace tests.com.xcitestudios.Network.Email
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using global::com.xcitestudios.Network.Email;
    using System.Collections.Generic;
    using System.Net;
    using Newtonsoft.Json.Schema;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class EmailSerializableTest
    {
        [TestMethod]
        public void TestSerializationConsistency()
        {
            var email = new EmailSerializable();
            email.From = new Contact[] { 
                new Contact { Name = "From Name", Email = "from@email.com" },
            };
            email.To = new Contact[] {
                new Contact { Name = "To Name", Email = "to@email.com" }
            };
            email.ReplyTo = new Contact[] {
                new Contact { Name = "No Reply", Email = "noreply@email.com" }
            };
            email.CC = new Contact[] {
                new Contact { Name = "To Name CC", Email = "tocc@email.com" }
            };
            email.BCC = new Contact[] {
                new Contact { Name = "To Name BCC", Email = "tobcc@email.com" }
            };

            email.Subject = "This subject";
            email.InReplyTo = "messageid@local<local>";
            email.Date = new DateTime(2015, 1, 5, 23, 11, 45, DateTimeKind.Utc);

            var subparts = new List<EmailBodyPart>();

            var subPartA = new EmailBodyPart();
            subPartA.ContentType = "image/jpg";
            subPartA.ContentTransferEncoding = "quoted-printable";
            subPartA.RawContent = "this-would-be-an-image";
            subparts.Add(subPartA);

            var subPartB = new EmailBodyPart();
            subPartB.ContentType = "image/gif";
            subPartB.ContentTransferEncoding = "quoted-printable";
            subPartB.RawContent = "this-would-be-an-image-also";
            subparts.Add(subPartB);

            var collectionPart = new EmailBodyPart();
            collectionPart.ContentType = "multipart/mixed";

            collectionPart.BodyParts = subparts.ToArray();

            email.BodyParts = new EmailBodyPart[] {collectionPart};

            var newEmail = new EmailSerializable();
            newEmail.DeserializeJSON(email.SerializeJSON());

            CollectionAssert.AllItemsAreInstancesOfType(newEmail.From, typeof(Contact));
            Assert.AreEqual(email.From.Length, newEmail.From.Length);
            Assert.AreEqual(email.From[0].Name, newEmail.From[0].Name);
            Assert.AreEqual(email.From[0].Email, newEmail.From[0].Email);

            CollectionAssert.AllItemsAreInstancesOfType(newEmail.To, typeof(Contact));
            Assert.AreEqual(email.To.Length, newEmail.To.Length);
            Assert.AreEqual(email.To[0].Name, newEmail.To[0].Name);
            Assert.AreEqual(email.To[0].Email, newEmail.To[0].Email);

            CollectionAssert.AllItemsAreInstancesOfType(newEmail.To, typeof(Contact));
            Assert.AreEqual(email.ReplyTo.Length, newEmail.ReplyTo.Length);
            Assert.AreEqual(email.ReplyTo[0].Name, newEmail.ReplyTo[0].Name);
            Assert.AreEqual(email.ReplyTo[0].Email, newEmail.ReplyTo[0].Email);

            CollectionAssert.AllItemsAreInstancesOfType(newEmail.To, typeof(Contact));
            Assert.AreEqual(email.CC.Length, newEmail.CC.Length);
            Assert.AreEqual(email.CC[0].Name, newEmail.CC[0].Name);
            Assert.AreEqual(email.CC[0].Email, newEmail.CC[0].Email);

            CollectionAssert.AllItemsAreInstancesOfType(newEmail.To, typeof(Contact));
            Assert.AreEqual(email.BCC.Length, newEmail.BCC.Length);
            Assert.AreEqual(email.BCC[0].Name, newEmail.BCC[0].Name);
            Assert.AreEqual(email.BCC[0].Email, newEmail.BCC[0].Email);

            Assert.AreEqual(email.Subject, newEmail.Subject);
            Assert.AreEqual(email.InReplyTo, newEmail.InReplyTo);
            Assert.AreEqual(email.Date, newEmail.Date);

            CollectionAssert.AllItemsAreInstancesOfType(newEmail.BodyParts, typeof(EmailBodyPart));
            Assert.AreEqual(email.BodyParts.Length, newEmail.BodyParts.Length);

            Assert.AreEqual(email.BodyParts[0].ContentTransferEncoding, newEmail.BodyParts[0].ContentTransferEncoding);
            Assert.AreEqual(email.BodyParts[0].ContentType, newEmail.BodyParts[0].ContentType);
        }

        [TestMethod]
        public void TestSerializationSchemaFails()
        {
            
            var email = new EmailSerializable();
            email.From = new Contact[] { 
                new Contact { Name = "From Name", Email = "from@email.com" },
                new Contact { Name = "From Name Two", Email = "from2@email.com" },
            };
            email.To = new Contact[] {
                new Contact { Name = "To Name", Email = "to@email.com" }
            };
            email.ReplyTo = new Contact[] {
                new Contact { Name = "No Reply", Email = "noreply@email.com" }
            };
            email.CC = new Contact[] {
                new Contact { Name = "To Name CC", Email = "tocc@email.com" }
            };
            email.BCC = new Contact[] {
                new Contact { Name = "To Name BCC", Email = "tobcc@email.com" }
            };

            email.Subject = "This subject";
            email.InReplyTo = "messageid@local<local>";
            email.Date = new DateTime(2015, 1, 5, 23, 11, 45, DateTimeKind.Utc);

            var subparts = new List<EmailBodyPart>();

            var subPartA = new EmailBodyPart();
            subPartA.ContentType = "image/jpg";
            subPartA.ContentTransferEncoding = "quoted-printable";
            subPartA.RawContent = "this-would-be-an-image";
            subparts.Add(subPartA);

            var subPartB = new EmailBodyPart();
            subPartB.ContentType = "image/gif";
            subPartB.ContentTransferEncoding = "quoted-printable";
            subPartB.RawContent = "this-would-be-an-image-also";
            subparts.Add(subPartB);

            var collectionPart = new EmailBodyPart();
            collectionPart.ContentType = "multipart/mixed";

            collectionPart.BodyParts = subparts.ToArray();

            email.BodyParts = new EmailBodyPart[] { collectionPart };

            var newEmail = new EmailSerializable();
            var json = email.SerializeJSON();

            string textSchema;

            using (var client = new WebClient())
            {
                textSchema = client.DownloadString("https://cdn.rawgit.com/xcitestudios/json-schemas/cdn-tag-2/com/xcitestudios/schemas/Network/Email.json");
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

            Assert.IsFalse(isValid, errorMessage);
        }

        [TestMethod]
        public void TestSerializationSchemaSucceeds()
        {
            var email = new EmailSerializable();
            email.From = new Contact[] { 
                new Contact { Name = "From Name", Email = "from@email.com" },
            };
            email.To = new Contact[] {
                new Contact { Name = "To Name", Email = "to@email.com" }
            };
            email.ReplyTo = new Contact[] {
                new Contact { Name = "No Reply", Email = "noreply@email.com" }
            };
            email.CC = new Contact[] {
                new Contact { Name = "To Name CC", Email = "tocc@email.com" }
            };
            email.BCC = new Contact[] {
                new Contact { Name = "To Name BCC", Email = "tobcc@email.com" }
            };

            email.Subject = "This subject";
            email.InReplyTo = "messageid@local<local>";
            email.Date = new DateTime(2015, 1, 5, 23, 11, 45, DateTimeKind.Utc);

            var subparts = new List<EmailBodyPart>();

            var subPartA = new EmailBodyPart();
            subPartA.ContentType = "image/jpg";
            subPartA.ContentTransferEncoding = "quoted-printable";
            subPartA.RawContent = "this-would-be-an-image";
            subparts.Add(subPartA);

            var subPartB = new EmailBodyPart();
            subPartB.ContentType = "image/gif";
            subPartB.ContentTransferEncoding = "quoted-printable";
            subPartB.RawContent = "this-would-be-an-image-also";
            subparts.Add(subPartB);

            var collectionPart = new EmailBodyPart();
            collectionPart.ContentType = "multipart/mixed";

            collectionPart.BodyParts = subparts.ToArray();

            email.BodyParts = new EmailBodyPart[] { collectionPart };

            var newEmail = new EmailSerializable();
            var json = email.SerializeJSON();

            string textSchema;

            using (var client = new WebClient())
            {
                textSchema = client.DownloadString("https://cdn.rawgit.com/xcitestudios/json-schemas/cdn-tag-2/com/xcitestudios/schemas/Network/Email.json");
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