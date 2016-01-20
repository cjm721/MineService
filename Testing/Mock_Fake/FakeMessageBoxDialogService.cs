using MineService_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UnitTestProject1
{
    public class FakeMessageBoxDialogService : IDialogService
    {
        public String message;
        public String title;
        public MessageBoxButton button;
        public MessageBoxImage icon;
        public int callCount;

        public FakeMessageBoxDialogService()
        {
            this.callCount = 0;
        }

        public void ShowMessageBox(string message, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            this.message = message;
            this.title = title;
            this.button = button;
            this.icon = icon;
            this.callCount++;
        }
    }
}
