using System;
using System.Windows;

namespace MineService_Client
{
    public interface IDialogService
    {
        void ShowMessageBox(String message, String title, MessageBoxButton button, MessageBoxImage icon);
    }
}
