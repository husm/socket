using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHE.GprsSocket
{
    class Program
    {
        //This variable determines the number of
        //SocketAsyncEventArg objects put in the pool of objects for receive/send.
        //The value of this variable also affects the Semaphore.
        //This app uses a Semaphore to ensure that the max # of connections
        //value does not get exceeded.
        //Max # of connections to a socket can be limited by the Windows Operating System
        //also.
        public const Int32 maxNumberOfConnections = 3000;

        //If this port # will not work for you, it's okay to change it.
        public const Int32 port = 4444;

        //You would want a buffer size larger than 25 probably, unless you know the
        //data will almost always be less than 25. It is just 25 in our test app.
        public const Int32 testBufferSize = 25;

        //This is the maximum number of asynchronous accept operations that can be
        //posted simultaneously. This determines the size of the pool of
        //SocketAsyncEventArgs objects that do accept operations. Note that this
        //is NOT the same as the maximum # of connections.
        public const Int32 maxSimultaneousAcceptOps = 10;

        //The size of the queue of incoming connections for the listen socket.
        public const Int32 backlog = 100;

        //For the BufferManager
        public const Int32 opsToPreAlloc = 2;    // 1 for receive, 1 for send

        //allows excess SAEA objects in pool.
        public const Int32 excessSaeaObjectsInPool = 1;

        //This number must be the same as the value on the client.
        //Tells what size the message prefix will be. Don't change this unless
        //you change the code, because 4 is the length of 32 bit integer, which
        //is what we are using as prefix.
        public const Int32 receivePrefixLength = 4;
        public const Int32 sendPrefixLength = 4;

        public static Int32 mainTransMissionId = 10000;
        public static Int32 startingTid; //
        public static Int32 mainSessionId = 1000000000;

        public static List<DataHolder> listOfDataHolders;

        // To keep a record of maximum number of simultaneous connections
        // that occur while the server is running. This can be limited by operating
        // system and hardware. It will not be higher than the value that you set
        // for maxNumberOfConnections.
        public static Int32 maxSimultaneousClientsThatWereConnected = 0;
        public static Object lockerForList = new Object();

        static void Main(string[] args)
        {
            try
            {
                // Get endpoint for the listener.
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

                //WriteInfoToConsole(localEndPoint);

                //This object holds a lot of settings that we pass from Main method
                //to the SocketListener. In a real app, you might want to read
                //these settings from a database or windows registry settings that
                //you would create.
                SocketListenerSettings theSocketListenerSettings =
                    new SocketListenerSettings(maxNumberOfConnections,
                    excessSaeaObjectsInPool, backlog, maxSimultaneousAcceptOps,
                    receivePrefixLength, testBufferSize, sendPrefixLength, opsToPreAlloc,
                    localEndPoint);

                Console.WriteLine("SocketListener init begin");

                //instantiate the SocketListener.
                SocketListener socketListener = new SocketListener(theSocketListenerSettings);

                Console.WriteLine("SocketListener init end");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
