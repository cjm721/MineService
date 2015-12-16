using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using System.Reflection;

namespace UnitTestProject1
{
    public partial class MainWindowTest
    {
        [TestMethod]
        public void TestAddNewServerButton()
        {
            MainWindow window = new MainWindow();
            MethodInfo methodInfo = typeof(MainWindow).GetMethod("add_new_server_buttonClick", System.Reflection.BindingFlags.NonPublic);
            methodInfo.Invoke(window, new Object[] {null, null});

            FieldInfo fieldInfo = typeof(MainWindow).GetField("new_server_folder");
            var text = fieldInfo.GetValue(window);
            Console.WriteLine(text);
        }
    }
}
