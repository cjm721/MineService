using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Net.Sockets;

namespace UnitTestProject1
{
    public partial class MainWindowTest
    {
        private IDialogService dialogService = new FakeMessageBoxDialogService();
        private MainWindow window;

        [TestInitialize]
        public void setUp()
        {
            window = new MainWindow(dialogService);
        }

        [TestMethod]
        public void TestAddNewServerButtonEmptyFolderAndName()
        {
            FieldInfo folderInfo = typeof(MainWindow).GetField("new_server_folder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FieldInfo nameInfo = typeof(MainWindow).GetField("new_server_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            TextBox folderText = (TextBox) folderInfo.GetValue(window);
            Assert.AreEqual(String.Empty, folderText.Text);

            TextBox nameText = (TextBox)nameInfo.GetValue(window);
            Assert.AreEqual(String.Empty, nameText.Text);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("add_new_server_button_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(window, new Object[] {null, null});

            FieldInfo messageBox = typeof(MainWindow).GetField("dialogService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FakeMessageBoxDialogService dialogService = (FakeMessageBoxDialogService) messageBox.GetValue(window);
            
            Assert.AreEqual("Must enter server name and server file", dialogService.message);
            Assert.AreEqual("Required Field Missing", dialogService.title);
            Assert.AreEqual(MessageBoxButton.OK, dialogService.button);
            Assert.AreEqual(MessageBoxImage.Error, dialogService.icon);
            Assert.AreEqual(1, dialogService.callCount);
        }

        [TestMethod]
        public void TestAddNewServerButtonEmptyName()
        {
            FieldInfo folderInfo = typeof(MainWindow).GetField("new_server_folder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FieldInfo nameInfo = typeof(MainWindow).GetField("new_server_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            TextBox box = new TextBox();
            box.Text = "testing";
            folderInfo.SetValue(window, box);

            TextBox nameText = (TextBox) nameInfo.GetValue(window);
            Assert.AreEqual(String.Empty, nameText.Text);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("add_new_server_button_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(window, new Object[] { null, null });

            FieldInfo messageBox = typeof(MainWindow).GetField("dialogService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FakeMessageBoxDialogService dialogService = (FakeMessageBoxDialogService)messageBox.GetValue(window);

            Assert.AreEqual("Must enter server name", dialogService.message);
            Assert.AreEqual("Required Field Missing", dialogService.title);
            Assert.AreEqual(MessageBoxButton.OK, dialogService.button);
            Assert.AreEqual(MessageBoxImage.Error, dialogService.icon);
            Assert.AreEqual(1, dialogService.callCount);
        }

        [TestMethod]
        public void TestAddNewServerButtonEmptyFolder()
        {
            FieldInfo folderInfo = typeof(MainWindow).GetField("new_server_folder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FieldInfo nameInfo = typeof(MainWindow).GetField("new_server_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            TextBox box = new TextBox();
            box.Text = "testing";
            nameInfo.SetValue(window, box);

            TextBox folderText = (TextBox) folderInfo.GetValue(window);
            Assert.AreEqual(String.Empty, folderText.Text);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("add_new_server_button_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(window, new Object[] { null, null });

            FieldInfo messageBox = typeof(MainWindow).GetField("dialogService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FakeMessageBoxDialogService dialogService = (FakeMessageBoxDialogService)messageBox.GetValue(window);

            Assert.AreEqual("Must enter server folder", dialogService.message);
            Assert.AreEqual("Required Field Missing", dialogService.title);
            Assert.AreEqual(MessageBoxButton.OK, dialogService.button);
            Assert.AreEqual(MessageBoxImage.Error, dialogService.icon);
            Assert.AreEqual(1, dialogService.callCount);
        }

        [TestMethod]
        public void TestCreateNewServer()
        {
            FakeMessageControl fakeMessageControl = new FakeMessageControl();
            CommunicationClient.INSTANCE = new CommunicationClient(fakeMessageControl, new FakeMessageBoxDialogService(), new MemoryStream());

            Assert.AreEqual(null, fakeMessageControl.messageSent);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("createServer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(window, new Object[] { "name", "folder" });

            Assert.AreNotEqual(null, fakeMessageControl.messageSent);
        }

        [TestCleanup]
        public void tearDown()
        {
            window = null;
            dialogService = null;
        }
    }
}
