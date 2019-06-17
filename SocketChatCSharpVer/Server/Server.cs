using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace SocketChatCSharpVersion
{
    public class Server
    {
        
        public Socket serverSocket;
        Socket clientSocket;
        IPAddress targetIP;
        EndPoint targetEndPoint;
        public bool debugMode = false;
        static byte[] messageByte = new byte[1024];

        public Server(SocketType streamType,string iP,int port)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, streamType, ProtocolType.Tcp);
            targetIP = IPAddress.Parse(ipString:iP);
            targetEndPoint = new IPEndPoint(address:targetIP,port:port);

        }
        public void InitServer(int maxLinkingCount)
        {
            serverSocket.Bind(localEP:targetEndPoint);
            serverSocket.Listen(backlog: maxLinkingCount);
            if(debugMode)
            {
                Console.WriteLine("Waiting for client socket connecting...");
            }
            clientSocket = serverSocket.Accept();
            if(debugMode)
            {
                Console.WriteLine("Client socket connected...");
            }
        }
        public void SendMessageToClientServer(string message)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes(message));
            if(debugMode)
            {
                Console.WriteLine(message + " " + "has been sended to the client socket...");
            }
        }
        public string ReceiveMessageFromClient()
        {
            int messageLength = clientSocket.Receive(messageByte);
            string receiveMessage = Encoding.UTF8.GetString(messageByte, 0, messageLength);
            if(debugMode)
            {
                Console.WriteLine(receiveMessage + " " + "has been gotten from the client socket");
            }
            return receiveMessage;
        }
        public static void Main(String[] args)
        {
            Server server = new Server(SocketType.Stream,"127.0.0.1",7788);
            server.InitServer(100);
            server.debugMode = false;
            //Thread receivingThread = new Thread(()=>{Console.WriteLine(server.ReceiveMessageFromClient());Thread.Sleep(200);});
            //receivingThread.Start();
            //Thread sendingThread = new Thread(()=>{server.SendMessageToClientServer(Console.ReadLine());Thread.Sleep(500);});
            //sendingThread.Start();
            Timer serverTimer = new Timer((object state)=>{
                Console.WriteLine(server.ReceiveMessageFromClient());
                server.SendMessageToClientServer(Console.ReadLine());
            },"",200,100);
            while (true);
        }
    }
}
