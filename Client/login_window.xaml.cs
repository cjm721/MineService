using System;
using System.Windows;
using MineService_JSON;
using MineService_Shared;
using System.Net.Sockets;

namespace MineService_Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public static LoginWindow INSTANCE;
        private IDialogService dialogService;
        private IMessageHandler messageHandler;

        public LoginWindow()
        {
            this.dialogService = new MessageBoxDialogService();
            this.messageHandler = new MessageHandler(this.dialogService);
            InitializeComponent();
            INSTANCE = this;
        }

        private void login_button_click(object sender, RoutedEventArgs e)
        {
            String temp = cluster_select_combobox.Text.Trim();
            string[] all = temp.Split(':');
            string user = username.Text;
            string pass = password.Password;
            try
            {
                IMessageControl control = new DESMessageControl();
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(all[0], Convert.ToInt32(all[1]));
                NetworkStream stream = tcpClient.GetStream();

                new CommunicationClient(control, this.dialogService, this.messageHandler, stream);

                Login log = new Login(user, pass);

                System.Console.WriteLine(log.toJsonString());

                CommunicationClient.INSTANCE.sendToServer(log.toJsonString());

                /*
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
                */

            }
            catch (Exception)
            {
                System.Console.WriteLine("error");
            }

        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
