using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MineService_Client_JSON;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;

namespace WPFMineServiceTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            home_TabControl.SelectionChanged += (o, e) =>
            {
                getData(((o as TabControl).SelectedItem as TabItem));
            };

            server_TabControl.SelectionChanged += (o, e) =>
            {
                getData(((o as TabControl).SelectedItem as TabItem));
            };            
        }

        private void getData(TabItem tabitem)
        {
            String name = tabitem.Name;
            switch (name)
            {
                case "overview_TabItem":
                    System.Console.WriteLine("overview"); //get server info here
                    break;
                case "FTP_TabItem":
                    System.Console.WriteLine("FTP");  //get server info here
                    break;
                case "users_TabItem":
                    System.Console.WriteLine("users"); //get server info here
                    break;
                case "settings_TabItem":
                    System.Console.WriteLine("settings");  //get server info here
                    break;
                case "Status_TabItem":
                    System.Console.WriteLine("Status"); //get server info here
                    break;
                case "Console_TabItem":
                    System.Console.WriteLine("Console");  //get server info here
                    break;
                case "Settings_TabItem":
                    System.Console.WriteLine("Settings"); //get server info here
                    break;
                case "Schedule_TabItem":
                    System.Console.WriteLine("Schedule");  //get server info here
                    break;
            }
        }

        private void tabControl2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Console.WriteLine("Line 83");
                TcpClient client = Class1.getClient();
                System.Console.WriteLine("Line 85, client: " + client.ToString());
                StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII);
                System.Console.WriteLine("line 86");

                MCCommand command = new MCCommand(States.MCCommandTYPE.Stop, "Monster", "");
                String json = JsonConvert.SerializeObject(command);
                Message msg = new Message(States.MessageTYPE.MCCommand, json);
                String js = JsonConvert.SerializeObject(msg);
                writer.WriteLine(js);
                System.Console.WriteLine("line 93");

            }
            catch (Exception)
            {
               
            }
        }
    }
}
