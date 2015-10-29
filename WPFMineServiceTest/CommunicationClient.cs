using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace WPFMineServiceTest
{
    public partial class CommunicationClient
    {
        public static CommunicationClient INSTANCE;

        public TcpClient clientSocket;
        private StreamReader reader;
        private Boolean Enabled;
        private StreamWriter writer;

        public CommunicationClient(String ip, int port)
        {
            INSTANCE = this;

            this.clientSocket = new TcpClient();
            this.clientSocket.Connect(ip, port);

            writer = new StreamWriter(this.clientSocket.GetStream(),Encoding.UTF8);

            Thread pmThread = new Thread(queueMessageAsync);
            pmThread.Start();
        }

        public void sendToServer(String msg)
        {
            if(this.clientSocket.Connected)
            {
                writer.WriteLine(msg);
                writer.Flush();
            }
        }

        public async void queueMessageAsync()
        {
            while (Enabled && clientSocket.Connected)
            {
                this.reader = new StreamReader(clientSocket.GetStream(), Encoding.UTF8);
                string line;
                Task<String> task = reader.ReadLineAsync();
                try
                {
                    line = await task;
                    Console.WriteLine("Message: " + line);
                }
                catch (IOException)
                {
                    // todo: a pop-up
                }
            }
            Enabled = false;
        }
    }
}
