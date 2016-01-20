using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MineService_Client
{
    public interface IDialogService
    {
        void ShowMessageBox(String message, String title, MessageBoxButton button, MessageBoxImage icon);
    }
}
