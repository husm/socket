using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    public class DataHoldingUserToken
    {
        internal Mediator theMediator;
        internal DataHolder theDataHolder;
        internal readonly Int32 bufferOffsetReceive;
        internal readonly Int32 permanentReceiveMessageOffset;
        internal readonly Int32 bufferOffsetSend;

        private Int32 idOfThisObject;

        internal Int32 lengthOfCurrentIncomingMessage;

        /// <summary>
        /// receiveMessageOffset is used to mark the byte position where the message
        /// begins in the reveive buffer. This value can sometimes be out of
        /// bounds for the data stream just received. But, if it is out of bounds, the
        /// code will not access it.
        /// </summary>
        internal Int32 receiveMessageOffset;
        internal byte[] byteArrayForPrefix;
        internal readonly Int32 receivePrefixLength;
        internal Int32 receivedPrefixBytesDoneCount = 0;
        internal Int32 receivedMessageBytesDoneCount = 0;
        /// <summary>
        /// This variable will be needed to calculate the value of the
        /// receiveMessageOffset variable in one situation. Notice that the
        /// name is similar but the usage is different from the variable
        /// receiveSendToken.receivePrefixBytesDone.
        /// </summary>
        internal Int32 recPrefixBytesDoneThisOp = 0;

        internal Int32 sendBytesRemainingCount;
        internal readonly Int32 sendPrefixLength;
        internal Byte[] dataToSend;
        internal Int32 bytesSentAlreadyCount;

        /// <summary>
        /// The session ID correlates with all the data sent in a connected session.
        /// It is different from the transmission ID in the DataHolder, which relates
        /// to one TCP message. A connected session could have many messages, if you
        /// set up your app to allow it.
        /// </summary>
        private Int32 sessionId;

        public DataHoldingUserToken(SocketAsyncEventArgs e, Int32 rOffset, Int32 sOffset,
            Int32 receivePrefixLength, Int32 sendPrefixLength, Int32 identifier)
        {
            this.idOfThisObject = identifier;

            // Create a Mediator that has a reference to the SAEA object.
            this.theMediator = new Mediator(e);
            this.bufferOffsetReceive = rOffset;
            this.bufferOffsetSend = sOffset;
            this.receivePrefixLength = receivePrefixLength;
            this.sendPrefixLength = sendPrefixLength;
            this.receiveMessageOffset = rOffset + receivePrefixLength;
            this.permanentReceiveMessageOffset = this.receiveMessageOffset;
        }

        /// <summary>
        /// Let's use an ID for this object during testing, just so we can see what
        /// is happening better if we want to.
        /// </summary>
        public Int32 TokenId
        {
            get
            {
                return this.idOfThisObject;
            }
        }

        internal void CreateNewDataHolder()
        {
            theDataHolder = new DataHolder();
        }

        /// <summary>
        /// Used to create sessionId variable in DataHoldingUserToken.
        /// Called in ProcessAccept().
        /// </summary>
        internal void CreateSessionId()
        {
            sessionId = Interlocked.Increment(ref Program.mainSessionId);
        }

        public Int32 SessionId
        {
            get
            {
                return this.sessionId;
            }
        }

        public void Reset()
        {
            this.receivedPrefixBytesDoneCount = 0;
            this.receivedMessageBytesDoneCount = 0;
            this.recPrefixBytesDoneThisOp = 0;
            this.receiveMessageOffset = this.permanentReceiveMessageOffset;
        }
    }
}
