using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFMineServiceTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
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
                Console.WriteLine("conncetion successfull");
            }
            catch (Exception)
            {
                Console.WriteLine("error");
                client.Close();
            }
        }
    }
}
