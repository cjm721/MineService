using MineService_Client_JSON;
using Newtonsoft.Json;
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

namespace WPFMineServiceTest
{
    /// <summary>
    /// Interaction logic for ServerTabItem.xaml
    /// </summary>
    public partial class ServerTabItem : UserControl
    {
        public String ServerID;

        public ServerTabItem(String ServerID) : this()
        {
            this.ServerID = ServerID;
        }

        public ServerTabItem()
        {
            InitializeComponent();

            this.start_stop_button.Click += Start_stop_button_Click;
        }

        private void Start_stop_button_Click(object sender, RoutedEventArgs e)
        {
            States.MCCommandTYPE state = States.MCCommandTYPE.Start;

            if (start_stop_button.Content.ToString().Contains("Stop"))
            {
                state = States.MCCommandTYPE.Stop;
            }
            else if (start_stop_button.Content.ToString().Contains("Start"))
            {
                state = States.MCCommandTYPE.Start;
            }

            MCCommand command = new MCCommand(state, this.ServerID, "");
            System.Console.WriteLine("command: " + command.type);
            String json = JsonConvert.SerializeObject(command);
            Message msg = new Message(States.MessageTYPE.MCCommand, json);
            String js = JsonConvert.SerializeObject(msg);
            System.Console.WriteLine("before sending to server, js: " + js.ToString());
            CommunicationClient.INSTANCE.sendToServer(js);
            System.Console.WriteLine("after send to server");

            start_stop_button.Content = "Pending";
            start_stop_button.IsEnabled = false;
        }
    }
}
