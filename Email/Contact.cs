namespace com.xcitestudios.Network.Email
{
    using com.xcitestudios.Generic.Data.Manipulation;
    using com.xcitestudios.Network.Email.Interfaces;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// A simple person or object.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Contact : JsonSerializationHelper, IContactSerializable, IComparable<IContact>
    {
        /// <summary>
        /// Name identifying the contact.
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Email address of the contact.
        /// </summary>
        [DataMember(Name="email")]
        public string Email { get; set; }

        /// <summary>
        /// Updates the element implementing this interface using a JSON representation. 
        /// This means updating the state of this object with that defined in the JSON 
        /// as opposed to returning a new instance of this object.
        /// </summary>
        /// <param name="jsonString">Representation of the object</param>
        public void DeserializeJSON(string jsonString)
        {
            var newObj = Deserialize<Contact>(jsonString);

            Name = newObj.Name;
            Email = newObj.Email;
        }

        /// <summary>
        /// Convert this object into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A JSON representation of this object</returns>
        public string SerializeJSON()
        {
            return Serialize<Contact>();
        }

        /// <summary>
        /// Compare based on email first, then name - case insensitive.
        /// </summary>
        /// <param name="that"></param> 
        /// <returns>int</returns>
        public int CompareTo(IContact that)
        {
            if (this.Email.ToLower().Equals(that.Email.ToLower()))
            {
                return this.Name.ToLower().CompareTo(that.Name.ToLower());
            }

            return this.Email.ToLower().CompareTo(that.Email.ToLower());
        }
    }
}
