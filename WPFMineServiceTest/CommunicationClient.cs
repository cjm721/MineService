using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Documents;
using MineService_JSON;
using MineService_Shared;

namespace MineService_Client
{
    public partial class CommunicationClient
    {
        public static CommunicationClient INSTANCE;

        public Stream stream;
        private IMessageControl control;
        private IDialogService dialogService;

        public CommunicationClient(IMessageControl control, IDialogService dialogService, Stream stream)
        {
            INSTANCE = this;
            this.control = control;
            this.dialogService = dialogService;

            this.stream = stream;

            Thread pmThread = new Thread(queueMessageAsync);
            pmThread.Start();
        }

        public void sendToServer(String msg)
        {
            try {
                control.sendMessage(this.stream, msg);
            }
            catch (IOException)
            {
                //TODO: do something more than swallow this error
            }
        }

        public void queueMessageAsync()
        {
            while (true)
            {
                string line;
                try
                {
                    line = control.getMessage(this.stream); 
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
                    break;
                }
            }
        }

        private void handleStatusMessage(Status status)
        {
            if (!Data.serverTabs.ContainsKey(status.ServerID))
            {
                MainWindow.INSTANCE.Dispatcher.Invoke(new Action(delegate()
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
        }

        private void handleNewWindow(Message msg)
        {
            LoginWindow.INSTANCE.Dispatcher.BeginInvoke(new Action(delegate()
            {
                new MainWindow(new MessageBoxDialogService()).Show();
                LoginWindow.INSTANCE.Close();

                Status[] array = JsonConvert.DeserializeObject<Status[]>(msg.message);

                foreach (Status s in array)
                {
                    ServerTabItem item = new ServerTabItem(s.ServerID);

                    item.UpdateTab(s.serverStatus);

                    MainWindow.INSTANCE.AddServerTab(item);
                }
            }));
        }

        private void handleConsole(MineService_JSON.Console console)
        {
            ServerTabItem tab = Data.serverTabs[console.ServerID];

            tab.consoleRichTextBox.Document.Dispatcher.BeginInvoke(new Action(delegate()
            {
                Paragraph pr = new Paragraph();
                foreach (String s in console.messages)
                    pr.Inlines.Add(s);

                tab.consoleRichTextBox.Document.Blocks.Add(pr);
                tab.consoleRichTextBox.ScrollToEnd();
            }));
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
                    handleStatusMessage(status);

                    break;
                case States.MessageTYPE.StatusArray:
                    if (MainWindow.INSTANCE == null)
                    {
                        handleNewWindow(msg);
                    }

                    break;
                case States.MessageTYPE.Error:
                    dialogService.ShowMessageBox(msg.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    break;
                case States.MessageTYPE.Console:
                    MineService_JSON.Console console = JsonConvert.DeserializeObject<MineService_JSON.Console>(msg.message);
                    handleConsole(console);

                    break;
            }
        }
    }
}
