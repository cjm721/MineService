using MineService_JSON;
using MineService_Shared;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void sendMessage(String message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return;
            try
            {
                System.Console.WriteLine("Sending Message: " + message + "\n");
                control.sendMessage(this.socket.GetStream(), message);
            }
            catch (IOException)
            {
                System.Console.WriteLine("Closing Socket");
                try
                {
                    this.socket.Close();
                }
                catch (IOException) { }
                System.Console.WriteLine("Removing Client from Active");
                Data.connectedClients.Remove(this);
            }
        }

        public void processMessage(String message)
        {
            Message msg = Message.fromJsonString(message);

            if (!authenticated && !(msg is Login))
            {
                return;
            }

            if (msg is MineService_JSON.Console) { 
                handleConsole((MineService_JSON.Console)msg);
            } else if (msg is Login)
            {
                    handleLogin((Login)msg);
            } else if (msg is MCCommand)
            {
                    handleMCCommand((MCCommand)msg);
            } else if (msg is Status)
            {
                    handleStatus((Status)msg);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SendMessageToAll(String msg)
        {
            for (int i = Data.connectedClients.Count - 1; i >= 0; i--)
            {
                Data.connectedClients[i].sendMessage(msg);
            }
        }
    }
}
