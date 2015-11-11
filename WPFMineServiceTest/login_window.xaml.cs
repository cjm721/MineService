﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using MineService_Client_JSON;

namespace MineService
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public static LoginWindow INSTANCE;
        public LoginWindow()
        {
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
                new CommunicationClient(all[0], Convert.ToInt32(all[1]));

                Login log = new Login(user, pass);
                String json = JsonConvert.SerializeObject(log);
                Message msg = new Message(States.MessageTYPE.Login, json);

                String js = JsonConvert.SerializeObject(msg);
                System.Console.WriteLine(js);

                CommunicationClient.INSTANCE.sendToServer(js);

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