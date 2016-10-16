using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    public class DataHolder
    {
        /// <summary>
        /// Remember, if a socket uses a byte array for its buffer, that byte array is
        /// unmanaged in .NET and can cause memory fragmentation. So, first write to the
        /// buffer block used by the SAEA object. Then, you can copy that data to another
        /// byte array, if you need to keep it or work on it, and want to be able to put
        /// the SAEA object back in the pool quickly, or continue with the data
        /// transmission quickly.
        /// Dataholder has this byte array to which you can copy the data.
        /// </summary>
        internal Byte[] dataMessageReceived;

        internal Int32 receivedTransMissionId;

        internal Int32 sessionId;

        /// <summary>
        /// For testing. With a packet analyzer this can help you see specific connections.
        /// </summary>
        internal EndPoint remoteEndPoint;
    }
}
