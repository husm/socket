using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHE.Socket
{
    class Program
    {
        /// <summary>
        /// Determins the number of <see cref="SocketAsyncEventArg"/> objects put in the pool
        /// of objects for receive/send. 
        /// </summary>
        public const Int32 MAX_CONNECTIONS = 3000;

        /// <summary>
        /// Listening port
        /// </summary>
        public const Int32 PORT = 6699;

        /// <summary>
        /// The default buffer size for this test
        /// </summary>
        public const Int32 TEST_BUFFER_SIZE = 25;

        /// <summary>
        /// The maximum number of ansynchronous accept operations that can be posted simultaneously.
        /// Determines the size of the pool of SocketAsyncEventArgs objects that do accept operations.
        /// </summary>
        public const Int32 MAX_ACCEPT_OPS = 10;

        /// <summary>
        /// The size of the queue of incoming connections for the listen socket.
        /// </summary>
        public const Int32 BACKLOG_SIZE = 100;

        /// <summary>
        /// Default buffer manager number.
        /// 1 for receive, 1 for send.
        /// </summary>
        public const Int32 BUFFER_MANAGERS = 2;

        /// <summary>
        /// Allow excess SocketAsyncEventArg objects in pool.
        /// </summary>
        public const Int32 ALLOW_EXCESS_SAEA = 1;

        /// <summary>
        /// The number must be the same as the value on the client.
        /// Tells what size the message prefix will be.
        /// 4 is the length of 32 bit integer.
        /// </summary>
        public const Int32 RECEIVE_PREFIX_LENGTH = 4;
        public const Int32 SEND_PREFIX_LENGTH = 4;

        public static Int32 mainTransMissionId = 10000;
        public static Int32 startingTid;
        public static Int32 mainSessionId = 1000000000;

        public static List<DataHolder> listOfDataHolders;

        /// <summary>
        /// Keep a record of maximum number of simultaneous connections
        /// that occur while the server is running. It can not be higher
        /// than the value of <see cref="MAX_CONNECTIONS"/>
        /// </summary>
        public static Int32 maxConnectedClients = 0;

        static void Main(string[] args)
        {
            try
            {
                // Get the endpoint for the listener
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, PORT);

                //WriteInfoToConsole(localEndPoint);

                // TODO: Use config file to setup the settings
                SocketListenerSettings socketListenerSettings = new SocketListenerSettings(MAX_CONNECTIONS,
                    ALLOW_EXCESS_SAEA, BACKLOG_SIZE, MAX_ACCEPT_OPS,
                    RECEIVE_PREFIX_LENGTH, TEST_BUFFER_SIZE, SEND_PREFIX_LENGTH, BUFFER_MANAGERS,
                    localEndPoint);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
