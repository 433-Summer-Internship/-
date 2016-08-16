using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace client
{
    class Client
    {
        //client socket
        private Socket mySocket = null;
        
        //server IPEP, SOCKET
        private IPEndPoint serverIPEP;
        private Socket serverSocket;

        private System.Timers.Timer timer;
        private int heartbeat = 0;

        private string errormsg = "";
        private int roomNumber;

        private void ResetState()
        {
            Console.Clear();
            if(errormsg!="")
            {
                Console.WriteLine(errormsg);
            }
        }
        //client state
        enum ClientState
        {
            None=0,
            Loading,
            Connect,
            Lobby,
            Room,
            Disconnect
        }
        private ClientState myState = ClientState.Loading;

        //Init Server IP, port
        public void Init(String IP,int port)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverIPEP = new IPEndPoint(IPAddress.Parse(IP), port);
        }
        static void Main(string[] args)
        {
            Client client = new Client();
            client.Init("127.0.0.1", 10000);
            client.Connecting();
            client.Process();
        }
        private bool StringCheckLengthExceed(string input,int Limit)
        {
            if (0 >= input.Length || Limit <= input.Length)
            {
                return true;
            }
            return false;
        }
        public void Connecting()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            myState = ClientState.Loading;
            //try connect
            SocketAsyncEventArgs serverConnect = new SocketAsyncEventArgs();
            serverConnect.RemoteEndPoint = serverIPEP;

            //connect request completed
            serverConnect.Completed += new EventHandler<SocketAsyncEventArgs>(Connect_Completed);
            //connect request
            serverSocket.ConnectAsync(serverConnect);
        }
        public void Process()
        {
            while (true)//로그아웃 or 종료
            {
                ResetState();
                switch (myState)
                {
                    #region case ClientState.None:
                    case ClientState.None:
                        StringBuilder IDPW = new StringBuilder();
                        IDPW.Remove(0, IDPW.Length);
                        string ID = null;
                        string PW = null;

                        #region write ID,pw
                        Console.Write("ID : ");
                        ID = Console.ReadLine();

                        if (StringCheckLengthExceed(ID, 12))
                        {
                            Console.WriteLine("ID length check(Enter)");
                            Console.ReadLine();
                            break;
                        }
                        Console.Write("PassWord : ");
                        PW = Console.ReadLine();

                        if (StringCheckLengthExceed(PW, 12))
                        {
                            Console.WriteLine("Password length check(Enter)");
                            Console.ReadLine();
                            break;
                        }
                        #endregion write ID,pw

                        IDPW.Append(ID);
                        IDPW.Append(",");
                        IDPW.Append(PW);

                        

                        //서버 연결이 성공하면 id체크를 시작한다.
                        Login(IDPW.ToString());
                        break;
                    #endregion case ClientState.None:
                    #region case ClientState.loading :
                    case ClientState.Loading:

                        Console.WriteLine("로딩 중");
                        Thread.Sleep(100);
                        
                        break;
                    #endregion loading
                    #region case ClientState.Disconnect:
                    case ClientState.Disconnect:
                        Console.WriteLine("*** Disconnected ***");
                        Console.WriteLine("try agin? (y/n)");
                        char d = Console.ReadKey().KeyChar;

                        do
                        {
                            if (d == 'y' || d == 'Y')
                            {
                                Connecting();
                                break;
                            }
                            else if (d == 'n' || d == 'N')
                            {
                                return;
                            }
                            else
                            {
                                Console.WriteLine("\nY or N only");
                                d = Console.ReadKey().KeyChar;
                            }
                        } while (true);
                        Console.WriteLine();
                        break;
                    #endregion case ClientState.Disconnect:
                    #region case ClientState.Lobby :
                    case ClientState.Lobby:

                        roomNumber = 0;
                        string lobbyMsg;

                        Console.WriteLine("-- Room List --");
                        Console.WriteLine("-- %Create / %Join --");
                        //반복문으로 출력해주고
                        //포문으로 룸스트럭트 배열이든 스트링 배열이든 출력

                        while (true)
                        {
                            Console.Write(">>> :");
                            lobbyMsg = Console.ReadLine();

                            if (lobbyMsg.ToUpper().Equals("%JOIN"))
                            {
                                Console.WriteLine("Room Number :");
                                bool numberCheck = Int32.TryParse(Console.ReadLine(), out roomNumber);

                                if (!numberCheck)
                                {
                                    Console.WriteLine("Romm Number Check(Enter)");
                                    Console.ReadLine();
                                    continue;
                                }
                                
                                //JoinRoom(roomNumber);
                                break;
                            }
                            else if (lobbyMsg.ToUpper().Equals("%CREATE"))
                            {
                                
                                //CreateRoom()
                                break;
                            }
                        }
                        break;
                    #endregion

                        
                    case ClientState.Room:
                        string msg;

                        Console.WriteLine("Enter Room : "+roomNumber);

                        do
                        {
                            Console.Write("send :");
                            msg = Console.ReadLine();

                            MessageProcess(msg);


                        } while (myState == ClientState.Room);//블린형 변수로 바꾸고 receive받아서 상태 변경
                        break;
                }
            }
        }
        private void Connect_Completed(object sender, SocketAsyncEventArgs e)
        {
            mySocket = (Socket)sender;
            
            //connected
            if (true == mySocket.Connected)
            {
                //Recive Event
                SocketAsyncEventArgs receiveEvent = new SocketAsyncEventArgs();

                receiveEvent.UserToken = mySocket;
                //receive buff
                receiveEvent.SetBuffer(new byte[1024], 0, 1024);
                //receive Event
                receiveEvent.Completed += new EventHandler<SocketAsyncEventArgs>(Recieve_Completed);
                //request receive
                mySocket.ReceiveAsync(receiveEvent);
                Console.WriteLine("*** 서버 연결 성공 ***");

                myState = ClientState.None;

                timer = new System.Timers.Timer();          
                timer.Interval = 3 * 1000;                 
                timer.Elapsed += new System.Timers.ElapsedEventHandler(SendHeartbeat);
                timer.Start();
            }
            else
            {
                Disconnection();
            }
        }
        private void SendHeartbeat(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (++heartbeat > 3)
            {
                Disconnection();
                heartbeat = 0;
                return;
            }
            SendMSG(Commands.HEARTBEAT);
        }
         
        private void Recieve_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket socketClient = (Socket)sender;
            
            if (true == socketClient.Connected)
            {//연결이 되어 있다.

                //데이터 수신
                Protocol receiveProtocol = function.bytearraytoprotocol(e.Buffer);

                //명령을 분석 한다.
                ProtocolAnalysis(receiveProtocol);

                //다음 메시지를 받을 준비를 한다.
                socketClient.ReceiveAsync(e);
            }
            else
            {
                Disconnection();
            }
        }
        private void ProtocolAnalysis(Protocol protocol)
        {
            switch (protocol.command)
            {
                case Commands.SIGNIN_FAIL:
                    errormsg = "login Fil";
                    myState = ClientState.None;
                    break;

                case Commands.SIGNIN_SUCCESS:
                    myState = ClientState.Loading;
                    //룸 리스트 요청하고
                    RoomList();
                    break;
                case Commands.ROOM_LIST_FAIL:
                    errormsg = "room list fail";
                    myState = ClientState.Disconnect;//??
                    break;

                case Commands.ROOM_LIST_SUCCESS:
                    myState = ClientState.Lobby;
                    //프로토콜을 룸 리스트로 바꿔서 저장
                    break;

                case Commands.JOIN_ROOM_SUCCESS:
                    myState = ClientState.Room;
                    //룸 서버 정보를 받아서
                    //init
                    
                    //연결 요청
                    //connecting

                    break;

                case Commands.JOIN_ROOM_FAIL:
                    errormsg = "join room fail";
                    myState = ClientState.Disconnect;//??
                    break;

                case Commands.ROOM_FULL:
                    errormsg = "Room Full";
                    myState = ClientState.Lobby;
                    break;

                case Commands.NO_ROOM:
                    errormsg = "Room not Exist";
                    myState = ClientState.Lobby;
                    break;

                case Commands.WRONG_SERVER:
                    break;

                case Commands.CREATE_ROOM_FAIL:
                    errormsg = "Create Room Fail";
                    myState = ClientState.Lobby;
                    break;

                case Commands.CREATE_ROOM_SUCCESS:
                    myState = ClientState.Room;
                    break;

                case Commands.LEAVE_ROOM_FAIL:
                    myState = ClientState.Disconnect;
                    break;

                case Commands.LEAVE_ROOM_SUCCESS:
                    myState = ClientState.Lobby;
                    break;

                case Commands.MSG_FAIL:
                    break;

                case Commands.MSG_SUCCESS:
                    string message = Encoding.UTF8.GetString(protocol.data,0,protocol.data.Length);
                    Console.Write(message);
                    break;

                case Commands.HEARTBEAT_SUCCESS:
                    heartbeat = 0;
                    break;

                case Commands.HEARTBEAT:
                    SendMSG(Commands.HEARTBEAT_SUCCESS);
                    break;

            }
        }
        
        private void Login(string IDPW)
        {
            SendMSG(Commands.SIGNIN, Encoding.UTF8.GetBytes(IDPW.ToString()));
        }
        private void JoinRoom(int roomNumber)
        {
            SendMSG(Commands.JOIN_ROOM, BitConverter.GetBytes(roomNumber));
        }
        private void CreateRoom()
        {
            SendMSG(Commands.CREATE_ROOM);
        }
        private void RoomList()
        {
            SendMSG(Commands.ROOM_LIST);
        }
        private void LeaveRoom()
        {
            roomNumber = 0;
            SendMSG(Commands.LEAVE_ROOM);
        }
        private void Message(string inpuMSG)
        {
            SendMSG(Commands.MSG, Encoding.UTF8.GetBytes(inpuMSG));
        }
        private void MessageProcess(string inpuMSG)
        {
            //명령어인지 메시지 인지 검사
            switch (inpuMSG.ToUpper())
            {
                case "%LEAVE":
                    myState = ClientState.Loading;
                    LeaveRoom();
                    break;

                default:
                    Message(inpuMSG);
                    break;
            }
        }
        private void Disconnection()
        {
            //mySocket.Disconnect(true);//???
            //init socket
            serverSocket.Close();
            
            if (timer!=null)
                timer.Stop();

            myState = ClientState.Disconnect;
            
            heartbeat = 0;
            mySocket = null;
        }
        
        private void SendMSG(ushort command,byte[] data=null)
        {
            
            errormsg="";

            if (mySocket == null)
                return;
            myState = ClientState.Loading;

            Protocol sendProtocol = new Protocol();
            SocketAsyncEventArgs sendEvent = new SocketAsyncEventArgs();
            
            sendProtocol.command = command;
            if (data == null)
            {
                sendProtocol.data=new byte[0];
                sendProtocol.length = (ushort)sendProtocol.data.Length;
            }
            else
            {
                sendProtocol.data=data;
                sendProtocol.length = (ushort)sendProtocol.data.Length;
            }
            
            byte[] szData = function.ProtocolToByteArray(sendProtocol);
            sendEvent.SetBuffer(szData, 0, szData.Length);
            mySocket.SendAsync(sendEvent);
        }
    }
}