using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace WPFMineServiceTest
{
    public class Class1
    {
        static TcpClient client = new TcpClient();
       
        public static TcpClient getClient()
        {
            return client;
        }
        

        public static void connect(String host, Int32 port)
        {
            client.Connect(host, port);
        }
    }
}
