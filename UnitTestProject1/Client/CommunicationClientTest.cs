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

namespace UnitTestProject1
{
    [TestClass]
    public class CommunicationClientTest
    {
        private MainWindow window;
        private CommunicationClient client;

        [TestInitialize]
        public void setUp()
        {
            window = new MainWindow(new FakeMessageBoxDialogService());

            client = new CommunicationClient(new FakeMessageControl(), new FakeMessageBoxDialogService(), new MemoryStream());
            Data.serverTabs = new Dictionary<string, ServerTabItem>();
        }

        [TestMethod]
        public void TestHandleStatusMessage()
        {
            FieldInfo fieldInfo = typeof(MainWindow).GetField("cluster_TabControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TabControl control = (TabControl)fieldInfo.GetValue(window);

            int tabCount = control.Items.Count;
            Assert.AreEqual(3, tabCount);

            MethodInfo methodInfo = typeof(CommunicationClient).GetMethod("handleStatusMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            
            ServerStatus serverStatus = new ServerStatus(true, 1000);
            Status status = new Status(States.StatusType.Send, "0", serverStatus);

            methodInfo.Invoke(client, new Object[] {status});

            tabCount = control.Items.Count;

            Assert.AreEqual(4, tabCount);
            Assert.AreEqual(1, Data.serverTabs.Count);
        }

        [TestMethod]
        public void TestHandleStatusMessageTwo()
        {
            FieldInfo fieldInfo = typeof(MainWindow).GetField("cluster_TabControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TabControl control = (TabControl)fieldInfo.GetValue(window);

            MethodInfo methodInfo = typeof(CommunicationClient).GetMethod("handleStatusMessage", BindingFlags.NonPublic | BindingFlags.Instance);

            ServerStatus serverStatus = new ServerStatus(true, 1000);
            Status status = new Status(States.StatusType.Send, "0", serverStatus);

            methodInfo.Invoke(client, new Object[] { status });

            int tabCount = control.Items.Count;

            Assert.AreEqual(4, tabCount);
            Assert.AreEqual(1, Data.serverTabs.Count);

            status.ServerID = "21";
            methodInfo.Invoke(client, new Object[] { status });

            tabCount = control.Items.Count;

            Assert.AreEqual(5, tabCount);
            Assert.AreEqual(2, Data.serverTabs.Count);
        }
    
        /*
        [TestMethod]
        public void TestHandleNewWindow()
        {
            MethodInfo methodInfo = typeof(CommunicationClient).GetMethod("handleNewWindow", BindingFlags.NonPublic | BindingFlags.Instance);

            String message = "[{\"TYPE\":\"Send\",\"ServerID\":\"hello\",\"serverStatus\":{\"isRunning\":false,\"uptime\":0,\"settings\":{\"enable_rcon\":false,\"white_list\":false,\"spawn_protection\":0,\"max_tick_time\":0,\"generator_settings\":null,\"force_gamemode\":false,\"allow_nether\":false,\"gamemode\":0,\"enable_query\":false,\"player_idle_timeout\":0,\"difficulty\":0,\"spawn_monsters\":false,\"op_permission_level\":0,\"resource_pack_hash\":null,\"announce_player_achievements\":false,\"pvp\":false,\"snooper_enabled\":false,\"level_type\":null,\"hardcore\":false,\"enable_command_block\":false,\"max_players\":0,\"network_compression_threshold\":0,\"max_world_size\":0,\"server_port\":0,\"server_ip\":null,\"spawn_npcs\":false,\"allow_flight\":false,\"level_name\":null,\"view_distance\":0,\"spawn_animals\":false,\"generate_structures\":false,\"online_mode\":false,\"max_build_height\":0,\"level_seed\":null,\"motd\":null}}}]";
            Message msg = new Message(States.MessageTYPE.Login | States.MessageTYPE.Error, message);

            Assert.AreEqual(0, Data.serverTabs.Count);
            methodInfo.Invoke(client, new Object[] { msg });
            Assert.AreNotEqual(0, Data.serverTabs.Count);
        }
        */

        [TestCleanup]
        public void tearDown()
        {
            client = null;
            window = null;
        }
    }
}
