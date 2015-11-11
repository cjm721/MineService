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
using System.Windows.Controls;

namespace MineService
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

                    System.Diagnostics.Debug.WriteLine( System.DateTime.Now.ToLongTimeString() + " Message: " + line);
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

            Message msg;
            try {
                msg = JsonConvert.DeserializeObject<Message>(line);
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return;
            }

            switch (msg.type)
            {
                case States.MessageTYPE.Status:
                    Status status = JsonConvert.DeserializeObject<Status>(msg.message);

                    if(!Data.serverTabs.ContainsKey(status.ServerID))
                    {
                        MainWindow.INSTANCE.Dispatcher.Invoke(new Action(delegate ()
                        {
                            ServerTabItem item = new ServerTabItem(status.ServerID);
                            MainWindow.INSTANCE.AddServerTab(item);
                        }));
                    }

                    ServerTabItem tab = Data.serverTabs[status.ServerID];

                    tab.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        tab.UpdateTab(status.serverStatus);
                    }));

                    break;
                case States.MessageTYPE.StatusArray:
                    if (MainWindow.INSTANCE == null)
                    {
                        LoginWindow.INSTANCE.Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            new MainWindow().Show();
                            LoginWindow.INSTANCE.Close();

                            Status[] array = JsonConvert.DeserializeObject<Status[]>(msg.message);

                            foreach(Status s in array)
                            {
                                ServerTabItem item = new ServerTabItem(s.ServerID);

                                MainWindow.INSTANCE.AddServerTab(item);
                                
                            }
                        }));
                    }

                    break;
                case States.MessageTYPE.Error:
                    MessageBox.Show(msg.message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    break;
                case States.MessageTYPE.Console:
                    MineService_Client_JSON.Console console = JsonConvert.DeserializeObject<MineService_Client_JSON.Console>(msg.message);
                    tab = Data.serverTabs[console.ServerID];

                    tab.consoleRichTextBox.Document.Dispatcher.BeginInvoke(new Action( delegate ()
                    {
                        Paragraph pr = new Paragraph();
                        foreach (String s in console.messages)
                            pr.Inlines.Add(s);

                        tab.consoleRichTextBox.Document.Blocks.Add(pr);
                        tab.consoleRichTextBox.ScrollToEnd();
                    }));


                    break;
            }
        }
    }
}
