using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp服务端
{
    class Message
    {

        public byte[] data = new byte[1024];

        public void Read(int len,Action<string>OnProessMessage)
        {
            string message = Encoding.UTF8.GetString(data, 0, len);
            OnProessMessage(message);
            Array.Clear(data, 0, len);
        }


        public byte[] Pack(string msg)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(msg);
            return dataBytes;
        }

    }
}
