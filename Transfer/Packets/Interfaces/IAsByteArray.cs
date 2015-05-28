using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.xcitestudios.Network.Transfer.Packets.Interfaces
{
    public interface IAsByteArray
    {
        /// <summary>
        /// Update this object with the bytes specified.
        /// </summary>
        void LoadBytes(byte[] bytes);

        /// <summary>
        /// Returns a byte array that defines this object.
        /// </summary>
        byte[] GetBytes();
    }
}
