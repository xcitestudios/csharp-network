namespace com.xcitestudios.Network.Email.Interfaces
{
    /// <summary>
    /// A simple person or object.
    /// </summary>
    public interface IContact
    {
        /// <summary>
        /// Name identifying the contact.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Email address of the contact.
        /// </summary>
        string Email { get; set; }
    }
}
