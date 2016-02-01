using System.Windows;

namespace MineService_Client
{
    public class MessageBoxDialogService : IDialogService
    {
        public void ShowMessageBox(string message, string title, System.Windows.MessageBoxButton button, System.Windows.MessageBoxImage icon)
        {
            MessageBox.Show(message, title, button, icon);
        }
    }
}
