using MineService_Shared;
using System.Net.Sockets;

namespace MineService_Server
{
    public class ServerMain
    {
        public static TcpListener serverSocket;

        public static void Main(string[] args)
        {
            Config.loadConfig();

            serverSocket = new TcpListener(System.Net.IPAddress.Any, 56552);

            serverSocket.Start(64);
            System.Console.WriteLine("Ready to accept Clients.");

            IMessageControl control = new DESMessageControl();
            while (true) {
                TcpClient clientSocket = serverSocket.AcceptTcpClient();

                Client client = new Client(clientSocket, control);
                client.startProcessing();
                Data.connectedClients.Add(client);
            }
        }
    }
}
