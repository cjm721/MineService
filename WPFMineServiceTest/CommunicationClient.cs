using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
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
        private IMessageHandler messageHandler;

        public CommunicationClient(IMessageControl control, IDialogService dialogService, IMessageHandler messageHandler, Stream stream)
        {
            INSTANCE = this;
            this.control = control;
            this.dialogService = dialogService;
            this.messageHandler = messageHandler;

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
                    messageHandler.handleMessage(line);
                }
                catch (IOException)
                {
                    // todo: a pop-up
                    break;
                }
            }
        }
    }
}
