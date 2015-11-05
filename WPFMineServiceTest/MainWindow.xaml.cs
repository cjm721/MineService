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


namespace WPFMineServiceTest
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
            if (server_name_TextBlock.Name.Contains("server_name_"))
            {
                System.Console.WriteLine("DEBUG: Gets here");
                Button button = ((Button)sender);
                String server = server_name_TextBlock.Text;
                States.MCCommandTYPE state = States.MCCommandTYPE.Start;

                if (((Button)sender).Content.ToString().Contains("Stop"))
                {
                    state = States.MCCommandTYPE.Stop;
                }
                else if (((Button)sender).Content.ToString().Contains("Start"))
                {
                    state = States.MCCommandTYPE.Start;
                }

                MCCommand command = new MCCommand(state, server, "");
                System.Console.WriteLine("command: " + command.type);
                String json = JsonConvert.SerializeObject(command);
                Message msg = new Message(States.MessageTYPE.MCCommand, json);
                String js = JsonConvert.SerializeObject(msg);
                System.Console.WriteLine("before sending to server, js: " + js.ToString());
                CommunicationClient.INSTANCE.sendToServer(js);
                System.Console.WriteLine("after send to server");

                button.Content = "Pending";
                button.IsEnabled = false;


                button.Content = (state == States.MCCommandTYPE.Stop) ? "Start Server" : "Stop Server";
                button.IsEnabled = true;
            }

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

            TabItem new_tab = GetNewServerTabItemFromObject();

            if (new_tab != null)
            {
                cluster_TabControl.Items.Insert(cluster_TabControl.Items.Count - 1, new_tab);
            }
        }

        public static T TrycloneElement<T>(T orig)
        {
            try
            {
                string s = XamlWriter.Save(orig);
                StringReader stringReader = new StringReader(s);
                XmlReader xmlReader = XmlTextReader.Create(stringReader, new XmlReaderSettings());
                XmlReaderSettings sx = new XmlReaderSettings();
                object x = XamlReader.Load(xmlReader);

                return (T)x;
            }
            catch
            {
                return (T)((object)null);
            }
        }

        public static TabItem GetNewServerTabItem()
        {
            string s = System.IO.File.ReadAllText("newServer.xaml.cs");
            StringReader stringReader = new StringReader(s);
            XmlReader xmlReader = XmlTextReader.Create(stringReader, new XmlReaderSettings());
            object x = XamlReader.Load(xmlReader);
            return ((TabItem)x);
        }

        public static TabItem GetNewServerTabItemFromObject()
        {
            Window2 w2 = new Window2();
            TabItem item = w2.server_Tab_1;
            w2.OverGrid.Children.Remove(item);
            return TrycloneElement(item);
        }
    }
}