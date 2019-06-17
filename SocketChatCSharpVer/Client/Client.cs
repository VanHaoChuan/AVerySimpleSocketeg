using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace SocketChatCSharpVersion
{
    public class Client
    {
        Socket clientSocket;
        EndPoint targetEndPoint;
        static byte[] messageByte = new byte[1024];

        public Client(Socket serverSocket)
        {
            clientSocket = new Socket(serverSocket.AddressFamily, serverSocket.SocketType, serverSocket.ProtocolType);
            targetEndPoint = serverSocket.LocalEndPoint;
        }
        public Client(SocketType socketType,string iP,int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork,socketType,ProtocolType.Tcp);
            IPAddress targetIPAddress = IPAddress.Parse(iP);
            targetEndPoint = new IPEndPoint(targetIPAddress,port);
        }
        void InitClient()
        {
            clientSocket.Connect(targetEndPoint);
        }
        void SendMessageToServer(string message)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes(message));
        }
        string ReceiveMessageFromServer()
        {
            int messageLength = clientSocket.Receive(messageByte);
            return Encoding.UTF8.GetString(messageByte, 0, messageLength);
        }
        static void ThreadAwakeCheck(Thread targetThread)
        {
            if(targetThread.ThreadState == ThreadState.Stopped)
            {
                Console.WriteLine(targetThread.ThreadState);
                targetThread.Start();
            }
        }
    
        public static void Main(String[] args)
        {
            Client client = new Client(SocketType.Stream,"127.0.0.1",7788);
            client.InitClient();
            //Thread sendingThread = new Thread(()=>{client.SendMessageToServer(Console.ReadLine());Thread.Sleep(500);});
            //sendingThread.Start();
            //Thread receivingThread = new Thread(()=>{Console.WriteLine(client.ReceiveMessageFromServer());Thread.Sleep(200);messageByte = new Byte[1024];});
            //receivingThread.Start();
            System.Threading.Timer threadCheckingTimer = new System.Threading.Timer((object state)=>{
                //Console.WriteLine("Recieving:"+receivingThread.ThreadState+"\n"+"Sending:"+sendingThread.ThreadState+"\n"+"Server connecting state:"+client.clientSocket.Connected+"\n"+"Byte message:"+Encoding.UTF8.GetString(messageByte)+"\n");
                //ThreadAwakeCheck(receivingThread);
                client.SendMessageToServer(Console.ReadLine());
                Console.WriteLine(client.ReceiveMessageFromServer());
                },"",200,100);
                while(true);
        }
    }
}
