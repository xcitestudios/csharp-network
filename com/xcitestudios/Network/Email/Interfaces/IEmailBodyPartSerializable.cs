namespace com.xcitestudios.Network.Email.Interfaces
{
    using com.xcitestudios.Generic.Data.Manipulation.Interfaces;

    /// <summary>
    /// A body part of an email.
    /// </summary>
    public interface IEmailBodyPartSerializable<T>: IEmailBodyPart<T>, ISerialization where T : IEmailBodyPartSerializable<T>
    {
    }

    /// <summary>
    /// A body part of an email.
    /// </summary>
    public interface IEmailBodyPartSerializable : IEmailBodyPartSerializable<IEmailBodyPartSerializable>
    {
    }
}
