﻿using System;
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
            if (!server_name_TextBlock.Name.Equals("cluster_tab"))
            {
                Button button = ((Button)sender);
                String server = server_name_TextBlock.Text;
                States.MCCommandTYPE state = States.MCCommandTYPE.Start;

                if (((Button)sender).Content.ToString().Contains("Stop"))
                {
                    state = States.MCCommandTYPE.Stop;
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
            }

        }

        private void add_new_server_button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(new_server_folder.Text) || String.IsNullOrWhiteSpace(new_server_name.Text))
            {
                if(String.IsNullOrWhiteSpace(new_server_name.Text) && String.IsNullOrWhiteSpace(new_server_folder.Text))
                {
                    MessageBox.Show("Must enter server name and server file","Required Field Missing",MessageBoxButton.OK, MessageBoxImage.Error);
                } else if(String.IsNullOrWhiteSpace(new_server_name.Text)) {
                    MessageBox.Show("Must enter server name", "Required Field Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                } else
                {
                    MessageBox.Show("Must enter server folder","Required Field Missing",MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }
            TabItem new_tab = TrycloneElement(server_Tab_1);
            //TabItem new_tab = GetNewServerTabItem();
            if (new_tab != null)
            {
                cluster_TabControl.Items.Add(new_tab);
                cluster_TabControl.Items.Remove(add_new_server); //These make sure that the add new server tab is always at the bottom
                cluster_TabControl.Items.Add(add_new_server);
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

        //public static T GetNewServerTabItem<T>()
        //{
        //    string s = XamlWriter.Save(File.ReadAllText("newServerTab.xaml.new"));
        //    StringReader stringReader = new StringReader(s);
        //    XmlReader xmlReader = XmlTextReader.Create(stringReader, new XmlReaderSettings());
        //    XmlReaderSettings sx = new XmlReaderSettings();
        //    object x = XamlReader.Load(xmlReader);
        //    return (T)x;
        //    //return  (TabItem) XamlReader.Parse(File.ReadAllText("newServerTab.xaml.new"));
        //}
    }
}