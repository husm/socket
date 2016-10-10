using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHE.Socket
{
    public class SocketListenerSettings
    {
        /// <summary>
        /// The maximum number of connections to handle simultaneously
        /// </summary>
        private Int32 maxConnections;

        /// <summary>
        /// The number of SAEA objects for the receive/send pool
        /// </summary>
        private Int32 numberOfSaeaForRecSend;

        /// <summary>
        /// Max number of pending connections the listener can hold in queue
        /// </summary>
        private Int32 backlog;

        /// <summary>
        /// The maximum obects to put in pool for accept operations
        /// </summary>
        private Int32 maxSimultaneousAcceptOps;

        /// <summary>
        /// Buffer size to use for each socket receive operation
        /// </summary>
        private Int32 receiveBufferSize;

        /// <summary>
        /// Length of message prefix for receive operations
        /// </summary>
        private Int32 receivePrefixLength;

        /// <summary>
        /// Length of message prefix for send operations
        /// </summary>
        private Int32 sendPrefixLength;

        /// <summary>
        /// Default allocated buffer manager, 1 for receive and 1 for send
        /// </summary>
        private Int32 opsToPreAllocate;

        /// <summary>
        /// Endpoint for the listener
        /// </summary>
        private IPEndPoint localEndPoint;

        public SocketListenerSettings(
            Int32 maxConnections,
            Int32 excessSaeaObjectsInPool,
            Int32 backlog,
            Int32 maxSimultaneousAcceptOps,
            Int32 receivePrefixLength,
            Int32 receiveBufferSize,
            Int32 sendPrefixLength,
            Int32 opsToPreAlloc,
            IPEndPoint localEndPoint)
        {
            this.maxConnections = maxConnections;
            this.numberOfSaeaForRecSend = maxConnections + excessSaeaObjectsInPool;
            this.backlog = backlog;
            this.maxSimultaneousAcceptOps = maxSimultaneousAcceptOps;
            this.receivePrefixLength = receivePrefixLength;
            this.receiveBufferSize = receiveBufferSize;
            this.sendPrefixLength = sendPrefixLength;
            this.opsToPreAllocate = opsToPreAlloc;
            this.localEndPoint = localEndPoint;
        }

        public Int32 MaxConnections
        {
            get
            {
                return this.maxConnections;
            }
        }

        public Int32 NumberOfSaeaForRecSend
        {
            get
            {
                return this.numberOfSaeaForRecSend;
            }
        }

        public Int32 Backlog
        {
            get
            {
                return this.backlog;
            }
        }

        public Int32 MaxAcceptOps
        {
            get
            {
                return this.maxSimultaneousAcceptOps;
            }
        }

        public Int32 ReceivePrefixLength
        {
            get
            {
                return this.receivePrefixLength;
            }
        }

        public Int32 BufferSize
        {
            get
            {
                return this.receiveBufferSize;
            }
        }

        public Int32 SendPrefixLength
        {
            get
            {
                return this.sendPrefixLength;
            }
        }

        public Int32 OpsToPreAllocate
        {
            get
            {
                return this.opsToPreAllocate;
            }
        }

        public IPEndPoint LocalEndPoint
        {
            get
            {
                return this.localEndPoint;
            }
        }
    }
}
