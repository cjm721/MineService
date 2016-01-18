using MineService_JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace MineService_Client
{
    public class MessageHandler : IMessageHandler
    {
        private IDialogService dialogService;

        public MessageHandler(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void handleMessage(string line)
        {
            if (line == null)
                return;

            Message msg;
            try
            {
                msg = JsonConvert.DeserializeObject<Message>(line);
            }
            catch (Exception e)
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
                System.Diagnostics.Debug.WriteLine("testing " + pr.ToString());

                tab.consoleRichTextBox.Document.Blocks.Add(pr);
                tab.consoleRichTextBox.ScrollToEnd();
            }));
        }
    }
}
