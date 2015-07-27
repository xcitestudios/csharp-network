namespace com.xcitestudios.Network.Email.Interfaces
{
    using com.xcitestudios.Generic.Data.Manipulation.Interfaces;
    using System;

    /// <summary>
    /// An email object with unlimited bodies which is serializable as JSON.
    /// </summary>
    public interface IEmailSerializable<T, U> : IEmail<T, U>, ISerialization
        where T: IContactSerializable
        where U: IEmailBodyPartSerializable<U>
    {
    }
}
