namespace com.xcitestudios.Network.Email.Interfaces
{
    /// <summary>
    /// A body part of an email.
    /// </summary>
    public interface IEmailBodyPart<T> where T: IEmailBodyPart<T>
    {
        /// <summary>
        /// Encoding of this body part. For a singular email this should go into the headers 
        /// of the original email.
        /// </summary>
        string ContentTransferEncoding { get; set; }

        /// <summary>
        /// RawContent type of the body. Can be a single type such as text/plain, text/html; or 
        /// a multipart type such as multipart/alternative or multipart/mixed.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Body of the body part. If you're using a singular ContentType this should be an array
        /// of length one with the body set.
        /// </summary>
        T[] BodyParts { get; set; }

        /// <summary>
        /// Raw content of this body part if it doesn't contain sub parts.
        /// </summary>
        string RawContent { get; set; }
    }

    /// <summary>
    /// A body part of an email.
    /// </summary>
    public interface IEmailBodyPart: IEmailBodyPart<IEmailBodyPart>
    {
    }
}
