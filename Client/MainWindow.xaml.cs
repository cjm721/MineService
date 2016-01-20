using System;
using System.Windows;
using System.Windows.Controls;
using MineService_JSON;
using MineService_Client.Tabs;

namespace MineService_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public String currentTabName;

        public static MainWindow INSTANCE;

        private IDialogService dialogService;

        public MainWindow(IDialogService dialogService)
        {
            INSTANCE = this;
            InitializeComponent();
            this.MinWidth = 750;
            this.MinHeight = 500;
            this.dialogService = dialogService;

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

            TabFactory factory = new TabFactory();
            String message = factory.createRequestDataMsg(name);

            CommunicationClient.INSTANCE.sendToServer(message);
        }

        private void tabControl2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }       

        private void add_new_server_button_Click(object sender, RoutedEventArgs e)
        {
            String msg = String.Empty;

            switch (getCreateServerError())
            {
                case 0:
                    createServer(new_server_name.Text, new_server_folder.Text);
                    return;
                case 1:
                    msg = "Must enter server folder";
                    break;
                case 2:
                    msg = "Must enter server name";
                    break;
                case 3:
                    msg = "Must enter server name and server file";
                    break;
            }

            dialogService.ShowMessageBox(msg, "Required Field Missing", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private int getCreateServerError()
        {
            bool isFolderNull = String.IsNullOrWhiteSpace(new_server_folder.Text);
            bool isNameNull = String.IsNullOrWhiteSpace(new_server_name.Text);

            return Convert.ToInt32(isFolderNull) | ((Convert.ToInt32(isNameNull)) << 1);
        }

        private void createServer(string name, string folder)
        {
            MCCommand createCommand = new MCCommand(States.MCCommandTYPE.Create, new_server_name.Text, new_server_folder.Text);
            CommunicationClient.INSTANCE.sendToServer(createCommand.toJsonString());
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