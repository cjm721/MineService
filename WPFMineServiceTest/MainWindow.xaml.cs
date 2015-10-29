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
        public String currentTabName;

        public MainWindow()
        {
            InitializeComponent();


            home_TabControl.SelectionChanged += (o, e) =>
            {
                TabItem temp = ((o as TabControl).SelectedItem as TabItem);
                getData(temp);
                currentTabName = temp.Name;
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

        private void start_stop_button_Click(object sender, RoutedEventArgs e)
        {
            if(currentTabName.Contains("server_Tab_"))
            {
                Button button = ((Button)sender);
                String server = currentTabName.Remove(0,11);
                States.MCCommandTYPE state = States.MCCommandTYPE.Start;

                if (((Button)sender).Content.ToString().Contains("Stop"))
                {
                    state = States.MCCommandTYPE.Stop;
                }

                MCCommand command = new MCCommand(state, server, "");
                System.Console.WriteLine("command: " + command.ToString());
                String json = JsonConvert.SerializeObject(command);
                Message msg = new Message(States.MessageTYPE.MCCommand, json);
                String js = JsonConvert.SerializeObject(msg);
                System.Console.WriteLine("before sending to server, js: " + js.ToString());
                CommunicationClient.INSTANCE.sendToServer(js);
                System.Console.WriteLine("after send to server");

                button.Content = "Pending";
                button.IsEnabled = false;
            }

        }
    }
}