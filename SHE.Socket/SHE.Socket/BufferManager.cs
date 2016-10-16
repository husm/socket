using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    /// <summary>
    /// Creates a single large buffer which can be divided up
    /// and assigned to SocketAsyncEventArgs objects for use with each
    /// socket I/O operation.
    /// This enables buffers to be easily reused and guards against
    /// fragmenting heap memory.
    /// 
    /// This buffer is a byte array which the windows TCP buffer can copy its data to.
    /// the total number of bytes controlled by the buffer pool
    /// </summary>
    public class BufferManager
    {
        /// <summary>
        /// The total number of bytes controlled by the buffer pool
        /// </summary>
        private Int32 totalBytesInBufferBlock;

        /// <summary>
        /// Byte array maintained by the Buffer Manager.
        /// </summary>
        private byte[] bufferBlock;

        private Stack<int> freeIndexPool;

        private Int32 currentIndex;

        private Int32 bufferBytesAllocatedForEachSaea;

        public BufferManager(Int32 totalBytes, Int32 totalBufferBytesInEachSaeaObject)
        {
            totalBytesInBufferBlock = totalBytes;
            this.currentIndex = 0;
            this.bufferBytesAllocatedForEachSaea = totalBufferBytesInEachSaeaObject;
            this.freeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// Allocates buffer space used by the buffer pool
        /// </summary>
        internal void InitBuffer()
        {
            // Create one large buffer block
            this.bufferBlock = new byte[totalBytesInBufferBlock];
        }

        /// <summary>
        /// Divide that one large buffer block out to each SocketAsyncEventArg object.
        /// Assign a buffer space from the buffer block to the
        /// specified SocketAsyncEventArgs object.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (this.freeIndexPool.Count > 0)
            {
                // This if-statement is only true if you have called the FreeBuffer method previously,
                // which would put an offset for a buffer space back into this stack.
                args.SetBuffer(this.bufferBlock, this.freeIndexPool.Pop(), this.bufferBytesAllocatedForEachSaea);
            }
            else
            {
                // Inside this else-statement is the code that is used to set the
                // buffer for each SAEA oject when the pool of SAEA objects is built
                // in the Init method.
                if ((totalBytesInBufferBlock - this.bufferBytesAllocatedForEachSaea) < this.currentIndex)
                {
                    return false;
                }
                args.SetBuffer(this.bufferBlock, this.currentIndex, this.bufferBytesAllocatedForEachSaea);
                this.currentIndex += this.bufferBytesAllocatedForEachSaea;
            }
            return true;
        }

        /// <summary>
        /// Removes the buffer from a SocketAsyncEventArg object. This frees the
        /// buffer back to the buffer pool. Try NOT to use FreeBuffer method,
        /// unless you need to destroy the SAEA object, or maybe in the case
        /// of some exception handling. Instead, on the server
        /// keep the same buffer space assigned to one SAEA object for the duration of
        /// this app's running
        /// </summary>
        /// <param name="args"></param>
        internal void FreeBuffer(SocketAsyncEventArgs args)
        {
            this.freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
