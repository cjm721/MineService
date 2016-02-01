using MineService_JSON;
using MineService_Shared.Json;
using System;
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
                msg = Message.fromJsonString(line);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return;
            }

            if (msg is MineService_JSON.Console)
            {
                    handleConsole((MineService_JSON.Console)msg);
            }
            //else if (msg is Login)
            //{
            //}
            //else if (msg is MCCommand)
            //{
            //}
            else if (msg is Status)
            {
                    handleStatusMessage((Status)msg);
            } else if (msg is Error)
            {
                dialogService.ShowMessageBox(((Error)msg).errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if (msg is StatusArray)
            {
                if (MainWindow.INSTANCE == null)
                {
                    handleNewWindow((StatusArray)msg);
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

        private void handleNewWindow(StatusArray sArray)
        {
            LoginWindow.INSTANCE.Dispatcher.BeginInvoke(new Action(delegate()
            {
                new MainWindow(new MessageBoxDialogService()).Show();
                LoginWindow.INSTANCE.Close();

                foreach (Status s in sArray.array)
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
