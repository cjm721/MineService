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
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;


namespace MineService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public String currentTabName;

        public static MainWindow INSTANCE;

        public MainWindow()
        {
            INSTANCE = this;
            InitializeComponent();
            this.MinWidth = 750;
            this.MinHeight = 500;

            cluster_TabControl.SelectionChanged += (o, e) =>
            {
                TabItem temp = ((o as TabControl).SelectedItem as TabItem);
                currentTabName = temp.Name;
            };

            home_TabControl.SelectionChanged += (o, e) =>
            {
                TabItem temp = ((o as TabControl).SelectedItem as TabItem);
                getData(temp);
                currentTabName = temp.Name;
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


        private void add_new_server_button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(new_server_folder.Text) || String.IsNullOrWhiteSpace(new_server_name.Text))
            {
                if (String.IsNullOrWhiteSpace(new_server_name.Text) && String.IsNullOrWhiteSpace(new_server_folder.Text))
                {
                    MessageBox.Show("Must enter server name and server file", "Required Field Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (String.IsNullOrWhiteSpace(new_server_name.Text))
                {
                    MessageBox.Show("Must enter server name", "Required Field Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Must enter server folder", "Required Field Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }

            MCCommand createCommand = new MCCommand(States.MCCommandTYPE.Create, new_server_name.Text, new_server_folder.Text);
            String createCommandStr = JsonConvert.SerializeObject(createCommand);

            Message toSend = new Message(States.MessageTYPE.MCCommand, createCommandStr);
            CommunicationClient.INSTANCE.sendToServer(JsonConvert.SerializeObject(toSend));
        }

        public void AddServerTab(ServerTabItem item)
        {
            TabItem newTab = new TabItem();
            newTab.Header = item.ServerID;
            newTab.Content = item;

            cluster_TabControl.Items.Insert(cluster_TabControl.Items.Count - 1, newTab);

            Data.serverTabs.Add(item.ServerID, item);
        }
    }
}