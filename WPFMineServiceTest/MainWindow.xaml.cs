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
                    Console.WriteLine("overview"); //get server info here
                    break;
                case "FTP_TabItem":
                    Console.WriteLine("FTP");  //get server info here
                    break;
                case "users_TabItem":
                    Console.WriteLine("users"); //get server info here
                    break;
                case "settings_TabItem":
                    Console.WriteLine("settings");  //get server info here
                    break;
                case "Status_TabItem":
                    Console.WriteLine("Status"); //get server info here
                    break;
                case "Console_TabItem":
                    Console.WriteLine("Console");  //get server info here
                    break;
                case "Settings_TabItem":
                    Console.WriteLine("Settings"); //get server info here
                    break;
                case "Schedule_TabItem":
                    Console.WriteLine("Schedule");  //get server info here
                    break;
            }
        }

        private void tabControl2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
