using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Csharp服务端
{
    public class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();

        public Client() { }

        public Client(Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }


        public void Start()
        {
            //判断客户端是否被关闭
            if (clientSocket.Connected == false || clientSocket.Poll(10, SelectMode.SelectRead))
            {
                clientSocket.Close();
                return;
            }
            //等待从客户端的消息的接收
            clientSocket.BeginReceive(msg.data, 0, msg.data.Length, SocketFlags.None, ReceiveCallBack, null);
        }


        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                //需要用try catch 捕捉异常，不然出现异常服务器会终止
                //判断客户端是否被关闭
                if (clientSocket.Connected==false || clientSocket.Poll(10,SelectMode.SelectRead))
                {
                    clientSocket.Close();
                    return;
                }

                int len = clientSocket.EndReceive(ar);//返回的len表示接收的数据长度
                if (len == 0)//客户端被关闭了 服务端会接收到0字节的消息
                {
                    clientSocket.Close();
                    return;
                }

                msg.Read(len, OnProessMassage);//Message类解析消息
                //Console.WriteLine("receive:" + message);

                clientSocket.BeginReceive(msg.data, 0, msg.data.Length, SocketFlags.None, ReceiveCallBack, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("ReceiveCallBack:" + e);
            }
            
        }

        private void OnProessMassage(string msg)
        {
            server.Broadcast(msg);//通过服务器广播消息
        }


        public void SendMessage(string message)
        {
           　//判断客户端是否被关闭  发送的时候客户端也可能被关闭
            if (clientSocket.Connected == false || clientSocket.Poll(10, SelectMode.SelectRead))
            {
                clientSocket.Close();
                return;
            }


            //MethodInfo mi = msg.GetType().GetMethod("Pack");

            clientSocket.Send(msg.Pack(message));//发送消息给客户端
            //Console.WriteLine("send:"+message);
        }

    }
}
