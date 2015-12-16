using MineService_JSON;
using MineService_Shared;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace MineService_Server
{
    public partial class Client
    {
        TcpClient socket;
        Thread incomingMessageThread;
        IMessageControl control;


        public bool authenticated;

        public Client(TcpClient socket, IMessageControl control)
        {
            this.socket = socket;
            this.control = control;
        }

        public void startProcessing()
        {
            incomingMessageThread = new Thread(delegate () { messageProcessor(); });
            incomingMessageThread.Start();
        }

        public void messageProcessor()
        {
            StreamReader reader = new StreamReader(socket.GetStream());

            while (true)
            {
                String line = control.getMessage(socket.GetStream());

                System.Console.WriteLine("Message: " + line);

                processMessage(line);
            }
        }

        public void sendMessage(String message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return;
            try
            {
                System.Console.WriteLine(message + "\n");
                control.sendMessage(this.socket.GetStream(), message);
            }
            catch (IOException e)
            {
                System.Console.WriteLine("Closing Socket");
                try
                {
                    this.socket.Close();
                }
                catch (IOException e1)
                {
                    // e1.printStackTrace();
                }
                System.Console.WriteLine("Removing Client from Active");
                Data.connectedClients.Remove(this);
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
