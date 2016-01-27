using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using MineService_Shared;
using System.Reflection;
using MineService_JSON;
using System.Windows.Controls;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using System.Windows.Documents;
using MineService_Shared.Json;
using System.Threading;

namespace UnitTestProject1
{
    [TestClass]
    public class MessageHandlerTest
    {
        private MainWindow window;
        private MessageHandler handler;

        [TestInitialize]
        public void setUp()
        {
            window = new MainWindow(new FakeMessageBoxDialogService());
            IDialogService fakeDialogService = new FakeMessageBoxDialogService();
            handler = new MessageHandler(fakeDialogService);
            Data.serverTabs = new Dictionary<string, ServerTabItem>();
        }

        [TestMethod]
        public void TestHandleStatusMessage()
        {
            FieldInfo fieldInfo = typeof(MainWindow).GetField("cluster_TabControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TabControl control = (TabControl)fieldInfo.GetValue(window);

            int tabCount = control.Items.Count;
            Assert.AreEqual(3, tabCount);

            MethodInfo methodInfo = typeof(MessageHandler).GetMethod("handleStatusMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            
            ServerStatus serverStatus = new ServerStatus(true, 1000);
            Status status = new Status(States.StatusType.Send, "0", serverStatus);

            methodInfo.Invoke(handler, new Object[] {status});

            tabCount = control.Items.Count;

            Assert.AreEqual(4, tabCount);
            Assert.AreEqual(1, Data.serverTabs.Count);
        }

        [TestMethod]
        public void TestHandleStatusMessageTwo()
        {
            FieldInfo fieldInfo = typeof(MainWindow).GetField("cluster_TabControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TabControl control = (TabControl)fieldInfo.GetValue(window);

            MethodInfo methodInfo = typeof(MessageHandler).GetMethod("handleStatusMessage", BindingFlags.NonPublic | BindingFlags.Instance);

            ServerStatus serverStatus = new ServerStatus(true, 1000);
            Status status = new Status(States.StatusType.Send, "0", serverStatus);

            methodInfo.Invoke(handler, new Object[] { status });

            int tabCount = control.Items.Count;

            Assert.AreEqual(4, tabCount);
            Assert.AreEqual(1, Data.serverTabs.Count);

            status.ServerID = "21";
            methodInfo.Invoke(handler, new Object[] { status });

            tabCount = control.Items.Count;

            Assert.AreEqual(5, tabCount);
            Assert.AreEqual(2, Data.serverTabs.Count);
        }
        
        [TestMethod]
        public void TestHandleNewWindow()
        {
            Status[] status = new Status[] {
                new Status(States.StatusType.Send, "999", new ServerStatus(true, 1000)),
                new Status(States.StatusType.Send, "21", new ServerStatus(false, 1000))
            };
            StatusArray statuses = new StatusArray(status);

            new LoginWindow().Show();
            MethodInfo methodInfo = typeof(MessageHandler).GetMethod("handleNewWindow", BindingFlags.NonPublic | BindingFlags.Instance);
            
            FieldInfo mainWindowClusterTab = typeof(MainWindow).GetField("cluster_TabControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TabControl control = (TabControl)mainWindowClusterTab.GetValue(window);

            int tabCount = control.Items.Count;

            Assert.AreEqual(3, tabCount);
            Thread thread = new Thread(delegate() { methodInfo.Invoke(handler, new Object[] { statuses }); });
            thread.Start();
            thread.Join();

            
            LoginWindow.INSTANCE.Dispatcher.Invoke(new Action(delegate()
            {
                tabCount = control.Items.Count;
                Assert.AreNotEqual(3, tabCount);
            }));
        }
        

        [TestCleanup]
        public void tearDown()
        {
            handler = null;
            window = null;
        }
    }
}
