namespace com.xcitestudios.Network.Email
{
    using com.xcitestudios.Generic.Data.Manipulation;
    using com.xcitestudios.Network.Email.Interfaces;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Body part of an email
    /// </summary>
    [DataContract]
    [Serializable]
    public class EmailBodyPart : JsonSerializationHelper, IEmailBodyPartSerializable
    {
        /// <summary>
        /// Encoding of this body part. For a singular email this should go into the headers 
        /// of the original email.
        /// </summary>
        [DataMember(Name="contentTransferEncoding")]
        public string ContentTransferEncoding { get; set; }

        /// <summary>
        /// RawContent type of the body. Can be a single type such as text/plain, text/html. However 
        /// if you add sub body parts then it should be multipart/alternative or multipart/mixed.
        /// </summary>
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// multipart/* body parts. Use this OR <see cref="RawContent"/>.
        /// </summary>
        [DataMember(Name = "bodyParts")]
        public IEmailBodyPartSerializable[] BodyParts { get; set; }

        /// <summary>
        /// Raw content of this body part. Use this OR <see cref="BodyParts"/>.
        /// </summary>
        [DataMember(Name = "rawContent")]
        public string RawContent { get; set; }

        /// <summary>
        /// Updates the element implementing this interface using a JSON representation. 
        /// This means updating the state of this object with that defined in the JSON 
        /// as opposed to returning a new instance of this object.
        /// </summary>
        /// <param name="jsonString">Representation of the object</param>
        public void DeserializeJSON(string jsonString)
        {
            var newObj = Deserialize<EmailBodyPart>(jsonString);

            ContentTransferEncoding = newObj.ContentTransferEncoding;
            ContentType = newObj.ContentType;
            BodyParts = newObj.BodyParts;
            RawContent = newObj.RawContent;
        }

        /// <summary>
        /// Convert this object into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A JSON representation of this object</returns>
        public string SerializeJSON()
        {
            return Serialize<EmailBodyPart>();
        }
    }
}
