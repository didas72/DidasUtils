using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace DidasUtils.Networking
{
    public static class NetworkingUtils
    {
        public delegate void RecievedMessageCallback(string message);
        public delegate void ConnectionChangeCallback(TcpClient socket);
        public delegate void ServerLoopCallback();



        public class ConnectionClient
        {
            public TcpClient clientSocket { get; private set; }
            private Thread clientReadThread;


            private RecievedMessageCallback recievedMessageCallback;


            private bool checkingRead = false;
            public string messageEndExpression { get; set; } = "!EOM";


            public ConnectionClient()
            {
                clientSocket = new TcpClient();
            }
            public ConnectionClient(string ip, int port)
            {
                clientSocket = new TcpClient(ip, port);
            }


            public void Connect(string ip, int port)
            {
                clientSocket.Close();
                clientSocket.Connect(ip, port);
            }
            public void StartRead(RecievedMessageCallback callback)
            {
                recievedMessageCallback = callback;

                checkingRead = true;

                clientReadThread = new Thread(RecieveLoop);
                clientReadThread.Start();
            }
            public void StopRead()
            {
                checkingRead = false;
            }
            private void RecieveLoop()
            {
                while (checkingRead)
                {
                    if (!clientSocket.Client.Connected)
                    {
                        checkingRead = false;
                        return;
                    }

                    string recievedMessage = ReadMessage();
                    recievedMessageCallback.DynamicInvoke(recievedMessage);
                }
            }

            public void SendMessage(string msg)
            {
                msg += messageEndExpression;

                NetworkStream networkStream = clientSocket.GetStream();

                byte[] sendBytes = Encoding.ASCII.GetBytes(msg);

                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
            }
            private string ReadMessage()
            {
                NetworkStream networkStream = clientSocket.GetStream();

                byte[] recievedBytes = new byte[clientSocket.ReceiveBufferSize];

                networkStream.Read(recievedBytes, 0, clientSocket.ReceiveBufferSize);

                string recievedString = Encoding.ASCII.GetString(recievedBytes);

                if (!recievedString.Contains(messageEndExpression))
                {
                    throw new MessageWithNoTerminatorException();
                }

                recievedString = recievedString.Substring(0, recievedString.LastIndexOf(messageEndExpression));

                return recievedString;
            }
        }

        public class ConnectionServer
        {
            public Dictionary<int, Client> clients { get; private set; } = new Dictionary<int, Client>();
            int clientIdCounter = 0;


            public int MaxConnections { get; set; }
            public int CurrentConnections { get
                {
                    return clients.Count;
                }
            }


            public RecievedMessageCallback recievedMessageCallback { get; set; }
            public ConnectionChangeCallback clientConnectedCallback { private get; set; }
            public ConnectionChangeCallback clientDisconnectedCallback { private get; set; }
            public ServerLoopCallback serverLoopCallback { private get; set; }


            private Thread serverLoopThread;
            private Thread serverAcceptConnectionThread;
            private Thread serverReadThread;


            public TcpListener serverSocket { get; private set; }


            private bool running = false;
            private bool acceptingConnections = false;
            private bool checkingRead = false;



            public string messageEndExpression { get; set; }



            public ConnectionServer(int port)
            {
                serverSocket = new TcpListener(IPAddress.Any, port);
            }
            public ConnectionServer(IPAddress ip, int port)
            {
                serverSocket = new TcpListener(ip, port);
            }



            public void StartServer()
            {
                serverSocket.Start();

                running = true;

                serverLoopThread = new Thread(ServerLoop);
                serverLoopThread.Start();
            }
            private void ServerLoop()
            {
                while (running)
                {
                    serverLoopCallback.Invoke();
                }
            }
            public void StopServer()
            {
                running = false;
                checkingRead = false;
                acceptingConnections = false;

                CloseAllConnections();

                serverSocket.Stop();
            }
            


            public void StartAcceptConnection()
            {
                acceptingConnections = true;

                serverAcceptConnectionThread = new Thread(AcceptConnectionLoop);
                serverAcceptConnectionThread.Start();
            }
            public void AcceptConnectionLoop()
            {
                while (acceptingConnections)
                {
                    int id = AcceptConnection(serverSocket.AcceptTcpClient());

                    if (checkingRead)
                    {
                        clients[id].StartClientThread();
                    }
                }
            }
            public void StopAccetConnection()
            {
                acceptingConnections = false;
            }



            public void StartRead(RecievedMessageCallback callback)
            {
                recievedMessageCallback = callback;

                checkingRead = true;

                foreach (KeyValuePair<int, Client> pair in clients)
                {
                    pair.Value.StartClientThread();
                }
            }
            public void StopRead()
            {
                checkingRead = false;

                foreach (KeyValuePair<int, Client> pair in clients)
                {
                    pair.Value.KILL_THE_DAMN_PROCESS_FUCKS_SAKE();
                }
            }
            


            public void SendMessage(string msg, TcpClient socket)
            {
                msg += messageEndExpression;

                NetworkStream networkStream = socket.GetStream();

                byte[] sendBytes = Encoding.ASCII.GetBytes(msg);

                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
            }
            private string ReadMessage(TcpClient socket)
            {
                NetworkStream networkStream = socket.GetStream();

                byte[] recievedBytes = new byte[socket.ReceiveBufferSize];

                networkStream.Read(recievedBytes, 0, socket.ReceiveBufferSize);

                string recievedString = Encoding.ASCII.GetString(recievedBytes);

                if (!recievedString.Contains(messageEndExpression))
                {
                    throw new MessageWithNoTerminatorException();
                }

                recievedString = recievedString.Substring(0, recievedString.LastIndexOf(messageEndExpression));

                return recievedString;
            }



            public int AcceptConnection(TcpClient socket)
            {
                clientIdCounter++;

                Client client = new Client(socket, clientIdCounter, this);

                clients.Add(clientIdCounter, client);

                clientConnectedCallback.Invoke(socket);

                return clientIdCounter;
            }
            public int AcceptConnection()
            {
                clientIdCounter++;

                TcpClient clientSocket = serverSocket.AcceptTcpClient();
                Client client = new Client(clientSocket, clientIdCounter, this);

                clients.Add(clientIdCounter, client);

                clientConnectedCallback.Invoke(clientSocket);

                return clientIdCounter;
            }
            public void CloseConnection(int id)
            {
                clients[id].clientThread.Abort();

                clientDisconnectedCallback.Invoke(clients[id].clientSocket);

                clients[id].clientSocket.Client.Close();
                clients[id].clientSocket.Close();

                clients.Remove(id);
            }
            public void CloseConnection(TcpClient socket)
            {
                int id = clients.First(t => t.Value.clientSocket == socket).Key;

                clients[id].clientThread.Abort();

                clientDisconnectedCallback.Invoke(clients[id].clientSocket);

                clients[id].clientSocket.Client.Close();
                clients[id].clientSocket.Close();

                clients.Remove(id);
            }
            public void CloseAllConnections()
            {
                List<int> idsToDisconnect = new List<int>();

                foreach (KeyValuePair<int,Client> pair in clients)
                {
                    idsToDisconnect.Add(pair.Key);
                }

                foreach (int i in idsToDisconnect)
                {
                    CloseConnection(i);
                }
            }
        }



        public class Client
        {
            public int id;
            public TcpClient clientSocket;
            private ConnectionServer parentServer;

            public Thread clientThread;

            public Client(TcpClient socket, int Id, ConnectionServer server)
            {
                clientSocket = socket; id = Id; parentServer = server;
            }

            public void StartClientThread()
            {
                clientThread = new Thread(doChat);
                clientThread.Start();
            }

            [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
            public void KILL_THE_DAMN_PROCESS_FUCKS_SAKE()
            {
                clientThread.Abort();
            }

            private void doChat()
            {
                string recievedData;

                while (true)
                {
                    try
                    {
                        if (!clientSocket.Client.Connected)
                        {
                            break;
                        }

                        recievedData = ReadMessage(clientSocket);

                        parentServer.recievedMessageCallback.Invoke(recievedData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        break;
                    }
                }
            }

            static string ReadMessage(TcpClient socket)
            {
                NetworkStream networkStream = socket.GetStream();

                byte[] recievedBytes = new byte[socket.ReceiveBufferSize];

                networkStream.Read(recievedBytes, 0, socket.ReceiveBufferSize);

                string recievedString = Encoding.ASCII.GetString(recievedBytes);

                if (!recievedString.Contains("$$$"))
                    return string.Empty;

                recievedString = recievedString.Substring(0, recievedString.LastIndexOf("$$$"));

                return recievedString;
            }
        }



        public class MessageWithNoTerminatorException : Exception
        {
            public MessageWithNoTerminatorException() : base("Message recieved did not have the specified message terminator.")
            {
                
            }
        }
    }
}
