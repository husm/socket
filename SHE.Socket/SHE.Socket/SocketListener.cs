using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    public class SocketListener
    {
        /// <summary>
        /// Buffers for sockets are unmanaged by .NET
        /// So memory used for buffers gets "pinned", which makes the
        /// .NET garbage collector work around it, fragmenting the memory.
        /// Circumvent this problem by putting all buffers together
        /// in one block in memory. Then we will assign a part of that space
        /// to each SocketAsyncEventArgs object, and
        /// reuse that buffer space each time we reuse the SocketAsyncEventArgs object.
        /// Create a large reusable set of buffers for all socket operations.
        /// </summary>
        private BufferManager theBufferManager;

        /// <summary>
        /// The socket used to listen for incoming connection requests
        /// </summary>
        private Socket listentSocket;

        /// <summary>
        /// A Semaphore has two parameters, the initial number of available slots
        /// and the maximum number of slots. We'll make them the same.
        /// This Semaphore is used to keep from going over max connection number.
        /// (It is not about controlling threading really here.)
        /// </summary>
        private Semaphore theMaxConnectionsEnforcer;

        /// <summary>
        /// An object that we pass in and which has all the settings the listener needs
        /// </summary>
        private SocketListenerSettings socketListenerSettings;

        private PrefixHandler prefixHandler;
        private MessageHandler messageHandler;

        /// <summary>
        /// Pool of reusable SocketAsyncEventArgs objects for accept operations
        /// </summary>
        private SocketAsyncEventArgsPool poolOfAcceptEventArgs;

        /// <summary>
        /// Pool of reusable SocketAsyncEventArgs objects for receive and send
        /// socket operations
        /// </summary>
        private SocketAsyncEventArgsPool poolOfRecSendEventArgs;

        public SocketListener(SocketListenerSettings theSocketListenerSettings)
        {
            this.socketListenerSettings = theSocketListenerSettings;
            this.prefixHandler = new PrefixHandler();
            this.messageHandler = new MessageHandler();

            // Allocate memory for buffers. We are using a separate buffer space for
            // receive and send, instead of sharing the buffer space, like the Microsoft
            // example does.
            this.theBufferManager = new BufferManager(this.socketListenerSettings.BufferSize
                * this.socketListenerSettings.NumberOfSaeaForRecSend
                * this.socketListenerSettings.OpsToPreAllocate,
                this.socketListenerSettings.BufferSize
                * this.socketListenerSettings.OpsToPreAllocate);

            this.poolOfRecSendEventArgs = new
                SocketAsyncEventArgsPool(this.socketListenerSettings.NumberOfSaeaForRecSend);

            this.poolOfAcceptEventArgs = new
                SocketAsyncEventArgsPool(this.socketListenerSettings.MaxAcceptOps);

            // Create connections count enforcer
            this.theMaxConnectionsEnforcer = new
                Semaphore(this.socketListenerSettings.MaxConnections,
                this.socketListenerSettings.MaxConnections);

            // Microsoft's example called these from Main method, which you
            // can easily do if you wish.
            Init();
            StartListen();
        }

        internal void Init()
        {

        }
    }
}
