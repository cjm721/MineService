using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using MineService_Client_JSON;

namespace WPFMineServiceTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
           // InitializeComponent();
        }

        private void login_button_click(object sender, RoutedEventArgs e)
        {
            TcpClient client = new TcpClient();
            String temp = cluster_select_combobox.Text.Trim();
            string[] all = temp.Split(':');
            string user = username.Text;
            string pass = password.SecurePassword.ToString();
            try
            {
                client.Connect(all[0], Convert.ToInt32(all[1]));
                System.Console.WriteLine("conncetion successfull");

                StreamReader reader = new StreamReader(client.GetStream(), Encoding.ASCII);
                StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII);

                Login log = new Login(user, pass);
                String json = JsonConvert.SerializeObject(log);
                Message msg = new Message(States.MessageTYPE.Login, json);

                String js = JsonConvert.SerializeObject(msg);
                System.Console.WriteLine(js);
                writer.WriteLine(js);
                writer.Flush();
            }
            catch (Exception)
            {
                System.Console.WriteLine("error");
                client.Close();
            }

        }
    }
}
