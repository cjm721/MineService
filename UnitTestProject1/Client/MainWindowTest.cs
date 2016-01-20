using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using System.Reflection;
using System.Windows.Controls;
using Newtonsoft.Json;
using MineService_JSON;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class MainWindowTest
    {
        private IDialogService dialogService = new FakeMessageBoxDialogService();

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
    }
}
