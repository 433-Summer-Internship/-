using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
namespace testserver
{
    class server
    {
        
        public static byte[] szData;
        public static Socket m_ServerSocket;
        static void Main(string[] args)
        {
            

             m_ServerSocket = new Socket(
                                AddressFamily.InterNetwork,
                                SocketType.Stream,
                                ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 10000);
            m_ServerSocket.Bind(ipep);
            m_ServerSocket.Listen(20);

            SocketAsyncEventArgs ae = new SocketAsyncEventArgs();
            ae.Completed
                += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);
            m_ServerSocket.AcceptAsync(ae);

            Console.ReadLine();
        }

        public static  void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket ClientSocket = e.AcceptSocket;
            

            if (ClientSocket != null)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                szData = new byte[1024];
                args.SetBuffer(szData, 0, 1024);
                args.UserToken = ClientSocket;
                args.Completed
                    += new EventHandler<SocketAsyncEventArgs>(Receive_Completed);
                ClientSocket.ReceiveAsync(args);
            }
            e.AcceptSocket = null;
            m_ServerSocket.AcceptAsync(e);
        }

        public static void Receive_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket ClientSocket = (Socket)sender;
            if (ClientSocket.Connected && e.BytesTransferred > 0)
            {
                
                Protocol receiveProtocol = function.bytearraytoprotocol(e.Buffer);

                Console.WriteLine(receiveProtocol.command);
                Console.WriteLine(receiveProtocol.GetLength());
                Console.WriteLine(Encoding.UTF8.GetString(receiveProtocol.GetData()));

                e.SetBuffer(szData, 0, 1024);



                SocketAsyncEventArgs sendEvent = new SocketAsyncEventArgs();


                if(receiveProtocol.command==Commands.HEARTBEAT)
                    receiveProtocol.command = Commands.HEARTBEAT_SUCCESS;
                else if(receiveProtocol.command == Commands.SIGNIN)
                    receiveProtocol.command = Commands.SIGNIN_SUCCESS;

                byte[] szData2 = function.ProtocolToByteArray(receiveProtocol);
                sendEvent.SetBuffer(szData2, 0, szData2.Length);


                ClientSocket.SendAsync(sendEvent);
                ClientSocket.ReceiveAsync(e);
            }
            else
            {
                ClientSocket.Disconnect(false);
                ClientSocket.Dispose();
            }
        }

    }
}
