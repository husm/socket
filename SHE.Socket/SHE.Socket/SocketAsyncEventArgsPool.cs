using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    internal sealed class SocketAsyncEventArgsPool
    {
        /// <summary>
        /// Just for assigning an ID so we can watch our objects while testing.
        /// </summary>
        private Int32 nextTokenId = 0;

        /// <summary>
        /// Pool of reusable SocketAsyncEventArgs objects.
        /// </summary>
        private Stack<SocketAsyncEventArgs> pool;

        /// <summary>
        /// Initializes the object pool to the specified size.
        /// "capacity" = Maximum number of SocketAsyncEventArgs objects.
        /// </summary>
        /// <param name="capacity"></param>
        internal SocketAsyncEventArgsPool(Int32 capacity)
        {
            this.pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// The number of SocketAsyncEventArgs instances in the pool.
        /// </summary>
        internal Int32 Count
        {
            get
            {
                return this.pool.Count;
            }
        }

        internal Int32 AssignTokenId()
        {
            Int32 tokenId = Interlocked.Increment(ref nextTokenId);
            return tokenId;
        }

        /// <summary>
        /// Removes a SocketAsyncEventArgs instance from the pool.
        /// returns 
        /// </summary>
        /// <returns></returns>
        internal SocketAsyncEventArgs Pop()
        {
            lock (this.pool)
            {
                return this.pool.Pop();
            }
        }

        /// <summary>
        /// Add a SocketAsyncEventArg instance to the pool.
        /// "item" = SocketAsyncEventArgs instance to add to the pool.
        /// </summary>
        /// <param name="item"></param>
        internal void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item added to a SocketAsyncEventArgsPool cannot be null");
            }
            
            lock (this.pool)
            {
                this.pool.Push(item);
            }
        }
    }
}
