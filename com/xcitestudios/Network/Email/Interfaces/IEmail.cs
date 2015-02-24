namespace com.xcitestudios.Network.Email.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An email object with unlimited bodies.
    /// </summary>
    public interface IEmail<T, U> 
        where T: IContact
        where U: IEmailBodyPart<U>
    {
        /// <summary>
        /// Any non standard headers here - these should never overwrite the explicit headers.
        /// </summary>
        Dictionary<string, string> CustomHeaders { get; set; }

        /// <summary>
        /// Who is the email from? Can be multiple people.
        /// </summary>
        T[] From { get; set; }

        /// <summary>
        /// Optional OR Required. Optional where From is one person. Required where From is multiple people.
        /// </summary>
        T Sender { get; set; }

        /// <summary>
        /// Optional. When hitting reply, who should the emails go to?
        /// </summary>
        T[] ReplyTo { get; set; }

        /// <summary>
        /// Recipients of the email.
        /// </summary>
        T[] To { get; set; }

        /// <summary>
        /// Optional. CC Recipients of the email.
        /// </summary>
        T[] CC { get; set; }

        /// <summary>
        /// Optional. BCC Recipients of the email.
        /// </summary>
        T[] BCC { get; set; }

        /// <summary>
        /// Optional. The time the email was "sent" (finished by a person/system). This is not
        /// necessarily the time the email entered a mail server.
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Optional. Subject of the email.
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// Optional. Which unique MessageID this is in reply to.
        /// </summary>
        string InReplyTo { get; set; }

        /// <summary>
        /// Body of the email. For a single content-type email, just put one in this array. 
        /// It is presumed if you add multiple items to this array then it must be multipart.
        /// 
        /// For multipart/alternative (multiple versions of the body such as text / html) add in
        /// one body part of type multipart/alternative which has multiple body parts.
        /// </summary>
        U[] BodyParts { get; set; }
    }
}
