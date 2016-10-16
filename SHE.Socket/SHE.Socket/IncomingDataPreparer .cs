using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    public class IncomingDataPreparer
    {
        private DataHolder theDataHolder;
        private SocketAsyncEventArgs theSaeaObject;

        public IncomingDataPreparer(SocketAsyncEventArgs e)
        {
            this.theSaeaObject = e;
        }

        private Int32 ReceivedTransMissionIdGetter()
        {
            Int32 receivedTransMissionId = Interlocked.Increment(ref Program.mainTransMissionId);
            return receivedTransMissionId;
        }

        private EndPoint GetRemoteEndpoint()
        {
            return this.theSaeaObject.AcceptSocket.RemoteEndPoint;
        }

        internal DataHolder HandleReceivedData(DataHolder incomingDataHolder, SocketAsyncEventArgs theSaeaObject)
        {
            DataHoldingUserToken receiveToken = (DataHoldingUserToken)theSaeaObject.UserToken;
            theDataHolder = incomingDataHolder;
            theDataHolder.sessionId = receiveToken.SessionId;
            theDataHolder.receivedTransMissionId = this.ReceivedTransMissionIdGetter();
            theDataHolder.remoteEndPoint = this.GetRemoteEndpoint();
            
        }

        private void AddDataHolder()
        {
            lock (Program.lockerForList)
            {
                Program.listOfDataHolders.Add(theDataHolder);
            }
        }
    }
}
