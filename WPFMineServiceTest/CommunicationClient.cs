using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Documents;
using MineService_JSON;
using MineService_Shared;

namespace MineService
{
    public partial class CommunicationClient
    {
        public static CommunicationClient INSTANCE;

        public TcpClient clientSocket;
        private IMessageControl control;

        public CommunicationClient(String ip, int port, IMessageControl control)
        {
            INSTANCE = this;
            this.control = control;

            this.clientSocket = new TcpClient();
            this.clientSocket.Connect(ip, port);

            Thread pmThread = new Thread(queueMessageAsync);
            pmThread.Start();
        }

        public void sendToServer(String msg)
        {
            if(this.clientSocket.Connected)
            {
                control.sendMessage(clientSocket.GetStream(), msg);
            }
        }

        public void queueMessageAsync()
        {
            while (clientSocket.Connected)
            {
                string line;
                try
                {
                    line = control.getMessage(clientSocket.GetStream()); 
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

                                item.UpdateTab(s.serverStatus);

                                MainWindow.INSTANCE.AddServerTab(item);                                
                            }
                        }));
                    }

                    break;
                case States.MessageTYPE.Error:
                    MessageBox.Show(msg.message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    break;
                case States.MessageTYPE.Console:
                    MineService_JSON.Console console = JsonConvert.DeserializeObject<MineService_JSON.Console>(msg.message);
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
