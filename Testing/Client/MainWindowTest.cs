using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using System.Reflection;
using System.Windows.Controls;
using MineService_JSON;
using System.IO;
using System.Windows;

namespace UnitTestProject1
{
    [TestClass]
    public class MainWindowTest
    {
        private IDialogService dialogService = new FakeMessageBoxDialogService();
        private MainWindow window;

        /// <summary>
        /// Tests the getData method of MainWindow to ensure that the proper messages are being sent to the server.
        /// </summary>
        [TestMethod]
        public void getDataTest()
        {
            String[] names;
            names = new String[4];
            names[0] = "overview_TabItem";
            names[1] = "FTP_TabItem";
            names[2] = "users_TabItem";
            names[3] = "settings_TabItem";

            Type[] types;
            types = new Type[4];
            types[0] = typeof(OverviewData);
            types[1] = typeof(FTPData);
            types[2] = typeof(Users);
            types[3] = typeof(Settings);


            MainWindow window = new MainWindow(new FakeMessageBoxDialogService());
            MethodInfo methodInfo = typeof(MainWindow).GetMethod("getData", System.Reflection.BindingFlags.NonPublic | BindingFlags.Instance);
            FakeMessageControl fakeControl = new FakeMessageControl();
            CommunicationClient.INSTANCE = new CommunicationClient(fakeControl, dialogService, new MessageHandler(dialogService), new MemoryStream());

            for (int i = 0; i < names.Length; ++i)
            {
                TabItem testTab = new TabItem();
                testTab.Name = names[i];
                methodInfo.Invoke(window, new Object[] { testTab });

                Message msg = Message.fromJsonString(fakeControl.getSentMessage());
                Assert.AreEqual(types[i], msg.GetType());
            }
        }


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

            TextBox folderText = (TextBox)folderInfo.GetValue(window);
            Assert.AreEqual(String.Empty, folderText.Text);

            TextBox nameText = (TextBox)nameInfo.GetValue(window);
            Assert.AreEqual(String.Empty, nameText.Text);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("add_new_server_button_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(window, new Object[] { null, null });

            FieldInfo messageBox = typeof(MainWindow).GetField("dialogService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FakeMessageBoxDialogService dialogService = (FakeMessageBoxDialogService)messageBox.GetValue(window);

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

            TextBox nameText = (TextBox)nameInfo.GetValue(window);
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

            TextBox folderText = (TextBox)folderInfo.GetValue(window);
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
            CommunicationClient.INSTANCE = new CommunicationClient(fakeMessageControl, dialogService, new MessageHandler(dialogService), new MemoryStream());

            Assert.AreEqual(null, fakeMessageControl.messageSent);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("createServer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(window, new Object[] { "name", "folder" });

            Assert.AreNotEqual(null, fakeMessageControl.messageSent);
        }

        [TestMethod]
        public void TestGetServerErrorCode0()
        {
            FieldInfo folderInfo = typeof(MainWindow).GetField("new_server_folder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            TextBox box = new TextBox();
            box.Text = "testing";
            folderInfo.SetValue(window, box);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("getCreateServerError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var val = methodInfo.Invoke(window, null);
            Assert.AreEqual(2, (int)val);
        }

        [TestMethod]
        public void TestGetServerErrorCode1()
        {
            FieldInfo nameInfo = typeof(MainWindow).GetField("new_server_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            TextBox box = new TextBox();
            box.Text = "testing";
            nameInfo.SetValue(window, box);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("getCreateServerError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var val = methodInfo.Invoke(window, null);
            Assert.AreEqual(1, (int)val);
        }

        [TestMethod]
        public void TestGetServerErrorCode2()
        {
            MethodInfo methodInfo = typeof(MainWindow).GetMethod("getCreateServerError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var val = methodInfo.Invoke(window, null);
            Assert.AreEqual(3, (int)val);
        }

        [TestMethod]
        public void TestGetServerErrorCode3()
        {
            FieldInfo folderInfo = typeof(MainWindow).GetField("new_server_folder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            FieldInfo nameInfo = typeof(MainWindow).GetField("new_server_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            TextBox box = new TextBox();
            box.Text = "testing";
            folderInfo.SetValue(window, box);
            nameInfo.SetValue(window, box);

            MethodInfo methodInfo = typeof(MainWindow).GetMethod("getCreateServerError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var val = methodInfo.Invoke(window, null);
            Assert.AreEqual(0, (int)val);
        }

        [TestCleanup]
        public void tearDown()
        {
            window = null;
            dialogService = null;
        }
    }
}
