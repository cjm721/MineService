using MineService_Client_JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MineService_Server
{
    public partial class Client
    {
        TcpClient socket;
        Thread incomingMessageThread;

        public bool authenticated;

        public Client(TcpClient socket)
        {
            this.socket = socket;

        }

        public void startProcessing()
        {
            incomingMessageThread = new Thread(delegate () { messageProcessor(); });
        }

        public void messageProcessor()
        {
            StreamReader reader = new StreamReader(socket.GetStream());

            while (true)
            {
                String line = reader.ReadLine();

                System.Console.WriteLine("Message: " + line);

                processMessage(line);
            }
        }

        public void processMessage(String message)
        {
            Message msg = JsonConvert.DeserializeObject<Message>(message);

            if (!authenticated && msg.type != States.MessageTYPE.Login)
            {
                return;
            }

            switch (msg.type)
            {
                case States.MessageTYPE.Console:

                    break;
                case States.MessageTYPE.Login:

                    break;
                case States.MessageTYPE.MCCommand:

                    break;
                case States.MessageTYPE.Status:

                    break;
            }
        }
    }
}
