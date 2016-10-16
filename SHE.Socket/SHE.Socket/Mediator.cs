using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    public class Mediator
    {
        private IncomingDataPreparer theIncomingDataPreparer;
        private OutgoingDataPreparer theOutgoingDataPreparer;
        private DataHolder theDataHolder;
        private SocketAsyncEventArgs saeaObject;

        public Mediator(SocketAsyncEventArgs e)
        {
            this.saeaObject = e;
            this.theIncomingDataPreparer = new IncomingDataPreparer(saeaObject);
            this.theOutgoingDataPreparer = new OutgoingDataPreparer();
        }

        internal void HandleData(DataHolder incomingDataHolder)
        {
            theDataHolder = theIncomingDataPreparer.HandleReceivedData(incomingDataHolder, this.saeaObject);
        }

        internal void PrepareOutgoingData()
        {
            theOutgoingDataPreparer.PrepareOutgoingData(saeaObject, theDataHolder);
        }

        internal SocketAsyncEventArgs GiveBack()
        {
            return saeaObject;
        }
    }
}
