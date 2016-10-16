﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    public class MessageHandler
    {
        public bool HandleMessage(SocketAsyncEventArgs receiveSendEventArgs,
            DataHoldingUserToken receiveSendToken,
            Int32 remainingBytesToProcess)
        {
            bool incomingTcpMessageIsReady = false;

            // Create the array where we'll store the complete message,
            // if it has not been created on a previous receive op.
            if (receiveSendToken.receivedMessageBytesDoneCount == 0)
            {
                receiveSendToken.theDataHolder.dataMessageReceived = new Byte[receiveSendToken.lengthOfCurrentIncomingMessage];
            }

            // Rememer there is a receiveSendToken.receivedPrefixBytesDoneCount
            // variable, which allowed us to handle the prefix even when it
            // requires mutiple receive ops. In the same way, we have a
            // receiveSendToken.receivedMessageBytesDoneCount varialbe, which
            // helps us handle message data, whether it requires one receive
            // operation or many.
            if (remainingBytesToProcess + receiveSendToken.receivedMessageBytesDoneCount
                == receiveSendToken.lengthOfCurrentIncomingMessage)
            {
                // If we are inside this if-statement, then we got
                // the end of the message. In other words,
                // the total number of bytes we received for this message matched the
                // message length value that we got from the prefix.

                // Write/append the bytes received to the byte array in the
                // DataHolder object that we are using to store our data.
                Buffer.BlockCopy(receiveSendEventArgs.Buffer,
                    receiveSendToken.receiveMessageOffset,
                    receiveSendToken.theDataHolder.dataMessageReceived,
                    receiveSendToken.receivedMessageBytesDoneCount,
                    remainingBytesToProcess);

                incomingTcpMessageIsReady = true;
            }
            else
            {
                // If we are inside this else-statement, then that means that we
                // need another receive op. We still haven't got the whole message,
                // even though we have examined all the data that was received.
                // Not a problem. In SocketListener.ProcessReceived we will just call
                // StartReceive to do another receive op to receive more data.
                Buffer.BlockCopy(receiveSendEventArgs.Buffer,
                    receiveSendToken.receiveMessageOffset,
                    receiveSendToken.theDataHolder.dataMessageReceived,
                    receiveSendToken.receivedMessageBytesDoneCount,
                    remainingBytesToProcess);

                receiveSendToken.receiveMessageOffset =
                    receiveSendToken.receiveMessageOffset - receiveSendToken.recPrefixBytesDoneThisOp;

                receiveSendToken.receivedMessageBytesDoneCount += remainingBytesToProcess;
            }
            return incomingTcpMessageIsReady;
        }
    }
}
