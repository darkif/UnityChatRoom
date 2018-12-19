using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Csharp服务端
{
    class Program
    {

        static void Main(string[] args)
        {
            string ip = "127.0.0.1";//这个ip地址表示本机
            int port = 6688; //测试用的端口号，应该可以自己随便写吧
            Server server = new Server(ip, port);
            server.Start();


            Console.ReadKey();
        }

    }
}
