using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using MineService_Client_JSON;
using System.Windows;
using System.Windows.Documents;

namespace WPFMineServiceTest
{
    public partial class CommunicationClient
    {
        public static CommunicationClient INSTANCE;

        public TcpClient clientSocket;
        private StreamReader reader;
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
            while (clientSocket.Connected)
            {
                this.reader = new StreamReader(clientSocket.GetStream(), Encoding.UTF8);
                string line;
                Task<String> task = reader.ReadLineAsync();
                try
                {
                    line = await task;
                    if(line == null)
                    {
                        return; // TODO: Make Error
                    }

                    System.Diagnostics.Debug.WriteLine("Message: " + line);
                    processMessage(line);
                }
                catch (IOException)
                {
                    // todo: a pop-up
                }
            }
        }

        public void processMessage(String line)
        {
            if (line == null)
                return;
            Message msg = JsonConvert.DeserializeObject<Message>(line);

            switch (msg.type)
            {
                case States.MessageTYPE.Status:
                    if (MainWindow.INSTANCE == null)
                    {
                        LoginWindow.INSTANCE.Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            new MainWindow().Show();
                            LoginWindow.INSTANCE.Close();
                        }));
                    }



                    break;
                case States.MessageTYPE.Error:
                    MessageBox.Show(msg.message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    break;
                case States.MessageTYPE.Console:
                    MineService_Client_JSON.Console console = JsonConvert.DeserializeObject<MineService_Client_JSON.Console>(msg.message);
                    ServerTabItem tab = Data.serverTabs[console.ServerID];

                    tab.consoleRichTextBox.Document.Dispatcher.BeginInvoke(new Action( delegate ()
                    {
                        Paragraph pr = new Paragraph();
                        foreach (String s in console.messages)
                            pr.Inlines.Add(s);

                        tab.consoleRichTextBox.Document.Blocks.Add(pr);
                    }));


                    break;
            }
        }
    }
}
