namespace com.xcitestudios.Network.Email
{
    using com.xcitestudios.Generic.Data.Manipulation;
    using com.xcitestudios.Network.Email.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// Implementation of the <see cref="I:com.xcitestudios.Network.Email.Interfaces.IEmailSerializable"/> interface with serialization support.
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(Contact)), KnownType(typeof(EmailBodyPart))]
    public class EmailSerializable : JsonSerializationHelper, IEmailSerializable<IContactSerializable, IEmailBodyPartSerializable>
    {
        /// <summary>
        /// Any non standard headers here - these should never overwrite the explicit headers.
        /// </summary>
        [DataMember(Name = "customHeaders")]
        public Dictionary<string, string> CustomHeaders { get; set; }

        /// <summary>
        /// Who is the email from? Can be multiple people.
        /// </summary>
        [DataMember(Name = "from")]
        public IContactSerializable[] From { get; set; }

        /// <summary>
        /// Optional OR Required. Optional where From is one person. Required where From is multiple people.
        /// </summary>
        [DataMember(Name = "sender")]
        public IContactSerializable Sender { get; set; }

        /// <summary>
        /// Optional. When hitting reply, who should the emails go to?
        /// </summary>
        [DataMember(Name = "replyTo")]
        public IContactSerializable[] ReplyTo { get; set; }

        /// <summary>
        /// Recipients of the email.
        /// </summary>
        [DataMember(Name = "to")]
        public IContactSerializable[] To { get; set; }

        /// <summary>
        /// Optional. CC Recipients of the email.
        /// </summary>
        [DataMember(Name = "cc")]
        public IContactSerializable[] CC { get; set; }

        /// <summary>
        /// Optional. BCC Recipients of the email.
        /// </summary>
        [DataMember(Name = "bcc")]
        public IContactSerializable[] BCC { get; set; }

        /// <summary>
        /// Optional. The time the email was "sent" (finished by a person/system). This is not
        /// necessarily the time the email entered a mail server.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// This just exists for ease of JSON serialization of <see cref="Date"/>.
        /// </summary>
        [DataMember(Name = "date")]
        private string _Date
        {
            get
            {
                return Date.ToString(@"yyyy-MM-ddTHH\:mm\:sszzz");
            }
            set
            {
                Date = DateTime.Parse(value, null, DateTimeStyles.RoundtripKind);
            }
        }

        /// <summary>
        /// Optional. Subject of the email.
        /// </summary>
        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Optional. Which unique MessageID this is in reply to.
        /// </summary>
        [DataMember(Name = "inReplyTo")]
        public string InReplyTo { get; set; }

        /// <summary>
        /// Body of the email. For a single content-type email, just put one in this array. 
        /// It is presumed if you add multiple items to this array then it must be multipart.
        /// 
        /// For multipart/alternative (multiple versions of the body such as text / html) add in
        /// one body part of type multipart/alternative which has multiple body parts.
        /// </summary>
        [DataMember(Name = "bodyParts")]
        public IEmailBodyPartSerializable[] BodyParts { get; set; }

        /// <summary>
        /// Updates the element implementing this interface using a JSON representation. 
        /// This means updating the state of this object with that defined in the JSON 
        /// as opposed to returning a new instance of this object.
        /// </summary>
        /// <param name="jsonString">Representation of the object</param>
        public void DeserializeJSON(string jsonString)
        {
            var newObj = Deserialize<EmailSerializable>(jsonString);

            CustomHeaders = newObj.CustomHeaders;
            From = newObj.From;
            Sender = newObj.Sender;
            ReplyTo = newObj.ReplyTo;
            To = newObj.To;
            CC = newObj.CC;
            BCC = newObj.BCC;
            Date = newObj.Date;
            Subject = newObj.Subject;
            InReplyTo = newObj.InReplyTo;
            BodyParts = newObj.BodyParts;
        }

        /// <summary>
        /// Convert this object into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A JSON representation of this object</returns>
        public string SerializeJSON()
        {
            return Serialize<EmailSerializable>();
        }
    }
}
