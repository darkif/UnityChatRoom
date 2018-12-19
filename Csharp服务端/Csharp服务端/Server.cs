using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Csharp服务端
{
    public class Server
    {
        private Socket serverSocket = null;
        private List<Client> clientList = new List<Client>();
        private Socket clientSocket = null;


        public Server() { }

        public Server(string ip,int port)
        {
            //创建socket，然后绑定ip地址和端口号
            //传输类型是流，使用的协议是Tcp
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            serverSocket.Bind(point);
        }

        public void Start()
        {
            serverSocket.Listen(0); //开始监听  0表示等待连接队列的最大数
            Console.WriteLine("开始监听");
            Console.WriteLine("等待客户端连接");
            serverSocket.BeginAccept(AcceptCallBack, null); //等待客户端连接，AcceptCallBack是回调，第三个参数目前还没用过，没去了解作用  TODO
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            clientSocket = serverSocket.EndAccept(ar);
            Console.WriteLine("一个客户端连接");
            Client client = new Client(clientSocket,this); //当客户端连接上后，用Client类处理消息的接收与发送
            clientList.Add(client);
            client.Start();

            serverSocket.BeginAccept(AcceptCallBack, null); //服务器继续等待其他客户端连接
        }

        public void Broadcast(string message) //广播消息
        {
            foreach(Client client in clientList)
            {
                client.SendMessage(message);
            }
        }
    }
}
