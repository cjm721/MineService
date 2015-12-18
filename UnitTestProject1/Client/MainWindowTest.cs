using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class MainWindowTest
    {
        [TestMethod]
        public void getDataTest()
        {
            StringWriter testWriter = new StringWriter();
            System.Console.SetOut(testWriter);

            String[] names;
            names = new String[8];
            names[0] = "overview_TabItem";
            names[1] = "FTP_TabItem";
            names[2] = "users_TabItem";
            names[3] = "settings_TabItem";
            names[4] = "Status_TabItem";
            names[5] = "Console_TabItem";
            names[6] = "Settings_TabItem";
            names[7] = "Schedule_TabItem";

            String[] print;
            print = new String[8];
            print[0] = "overview\r\n";
            print[1] = "FTP\r\n";
            print[2] = "users\r\n";
            print[3] = "settings\r\n";
            print[4] = "Status\r\n";
            print[5] = "Console\r\n";
            print[6] = "Settings\r\n";
            print[7] = "Schedule\r\n";

            for (int i = 0; i < names.Length; ++i)
            {
                MainWindow window = new MainWindow(new FakeMessageBoxDialogService());
                MethodInfo methodInfo = typeof(MainWindow).GetMethod("getData", System.Reflection.BindingFlags.NonPublic | BindingFlags.Instance);
                TabItem testTab = new TabItem();
                testTab.Name = names[i];
                methodInfo.Invoke(window, new Object[] { testTab });

                Assert.AreEqual(print[i], testWriter.ToString());
                testWriter.GetStringBuilder().Clear();
            }
            
        }
    }
}
