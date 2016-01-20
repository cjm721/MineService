using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Client;
using MineService_JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UnitTestProject1.Client
{
    [TestClass]
    public class ServerTabItemTest
    {
        public ServerTabItem tabItem;

        [TestInitialize]
        public void setup()
        {
            tabItem = new ServerTabItem("0");
        }

        [TestMethod]
        public void TestUpdateTabNewRunning() {
            ServerStatus status = new ServerStatus(true, 0);

            FieldInfo fieldInfo = typeof(ServerTabItem).GetField("aliveTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TextBlock aliveTimeBlock = (TextBlock) fieldInfo.GetValue(tabItem);

            String expected = "Offline";
            Assert.AreEqual(expected, aliveTimeBlock.Text);
            
            tabItem.UpdateTab(status);

            expected = "Starting";
            Assert.AreEqual(expected, aliveTimeBlock.Text);
        }

        [TestMethod]
        public void TestUpdateTabOffline()
        {
            ServerStatus status = new ServerStatus(false, 0);

            FieldInfo fieldInfo = typeof(ServerTabItem).GetField("aliveTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TextBlock aliveTimeBlock = (TextBlock)fieldInfo.GetValue(tabItem);

            String expected = "Offline";
            Assert.AreEqual(expected, aliveTimeBlock.Text);
            
            tabItem.UpdateTab(status);
            Assert.AreEqual(expected, aliveTimeBlock.Text);
        }

        [TestMethod]
        public void TestUpdateTabOfflineWithTimeRunning()
        {
            ServerStatus status = new ServerStatus(false, 100);

            FieldInfo fieldInfo = typeof(ServerTabItem).GetField("aliveTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TextBlock aliveTimeBlock = (TextBlock)fieldInfo.GetValue(tabItem);

            String expected = "Offline";
            Assert.AreEqual(expected, aliveTimeBlock.Text);

            tabItem.UpdateTab(status);
            Assert.AreEqual(expected, aliveTimeBlock.Text);
        }

        [TestMethod]
        public void TestUpdateTabOnlineWithTimeString()
        {
            long uptime = 100;
            ServerStatus status = new ServerStatus(true, uptime);

            FieldInfo fieldInfo = typeof(ServerTabItem).GetField("aliveTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TextBlock aliveTimeBlock = (TextBlock)fieldInfo.GetValue(tabItem);

            String expected = "Offline";
            Assert.AreEqual(expected, aliveTimeBlock.Text);
            
            tabItem.UpdateTab(status);

            TimeSpan ts = new TimeSpan(uptime * 10000);
            expected = ts.ToString(@"dd\D\ hh\H\ mm\M\ ss\S");

            Assert.AreEqual(expected, aliveTimeBlock.Text);
        }

        [TestCleanup]
        public void tearDown()
        {
            tabItem = null;
        }
    }
}
