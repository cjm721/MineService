using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Server
{
    public class ServerMain
    {
        public static TcpListener serverSocket;

        public static void Main(string[] args)
        {

            serverSocket = new TcpListener(System.Net.IPAddress.Any, 56552);

            serverSocket.Start(64);

        }
    }
}
