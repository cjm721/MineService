using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using System.Reflection;
using System.Windows.Controls;

namespace UnitTestProject1
{
    public partial class MainWindowTest
    {
        [TestMethod]
        public void TestAddNewServerButtonEmptyFolder()
        {
            MainWindow window = new MainWindow(new FakeMessageBoxDialogService());

            FieldInfo fieldInfo = typeof(MainWindow).GetField("new_server_folder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            TextBox text = (TextBox)fieldInfo.GetValue(window);
            Assert.AreEqual(String.Empty, text.Text);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("add_new_server_button_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(window, new Object[] {null, null});
        }
    }
}
