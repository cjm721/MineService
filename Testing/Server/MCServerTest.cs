using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_JSON;
using MineService_Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestProject1.Server
{
    [TestClass]
    public class MCServerTest
    {
        [TestMethod]
        public void startThreadInalizationTestFileNotFound()
        {
            MCServer server = new MCServer("UnitTestServer","UnitTestFolderFail");

            try {
                server.start();
                Assert.Fail("Did not cause a FileNotFoundException");
            } catch (FileNotFoundException)
            {
                Assert.IsTrue(true);
            }
        }

        /**
        *   Buggy Test due to how Process works. Mocking might be right solution to this issue.
        */
        [TestMethod]
        public void startThreadInalizationTest()
        {
            MCServer server = new MCServer("UnitTestServer", "TestFolder");

            server.start();
            Thread.Sleep(1000);

            Assert.IsTrue(server.isRunning());

            server.kill();
            server.pross.WaitForExit();
            Assert.IsFalse(server.isRunning());
        }

        [TestMethod]
        public void testDeleteRemoveFiles()
        {
            MCServer server = new MCServer("UnitTestServer", "UnitTestFolderFail");
            Data.mcServers.Add(server.ServerID,server);


            String folder = server.FolderDir;
            Assert.IsTrue(Directory.Exists(folder));

            server.delete(true);

            Assert.IsFalse(Directory.Exists(folder));
            Assert.IsFalse(Data.mcServers.ContainsKey(server.ServerID));
        }

        [TestMethod]
        public void testDeleteKeepFiles()
        {
            MCServer server = new MCServer("UnitTestServer", "UnitTestFolderFail");
            Data.mcServers.Add(server.ServerID, server);


            String folder = server.FolderDir;
            Assert.IsTrue(Directory.Exists(folder));

            server.delete(false);

            Assert.IsTrue(Directory.Exists(folder));
            Assert.IsFalse(Data.mcServers.ContainsKey(server.ServerID));
        }

        [TestMethod]
        public void testGetServerSettings()
        {
            MCServer server = new MCServer("UnitTestServer", "UnitTestFolderFail");
            MCServerSettings settings = server.getServerSettings();
            Assert.IsNotNull(settings);

            // Int Properties:
            Assert.AreEqual(settings.spawn_protection, 16);
            Assert.AreEqual(settings.max_tick_time, 60000);
            Assert.AreEqual(settings.gamemode, 0);
            Assert.AreEqual(settings.player_idle_timeout, 0);
            Assert.AreEqual(settings.difficulty, 1);
            Assert.AreEqual(settings.op_permission_level, 4);
            Assert.AreEqual(settings.max_players, 20);
            Assert.AreEqual(settings.network_compression_threshold, 256);
            Assert.AreEqual(settings.max_world_size, 29999984);
            Assert.AreEqual(settings.server_port, 25565);
            Assert.AreEqual(settings.view_distance, 10);
            Assert.AreEqual(settings.max_build_height, 256);

            // Boolean Properties:
            Assert.AreEqual(settings.force_gamemode, false);
            Assert.AreEqual(settings.allow_nether, true);
            Assert.AreEqual(settings.enable_query, false);
            Assert.AreEqual(settings.spawn_monsters, true);
            Assert.AreEqual(settings.announce_player_achievements, true);
            Assert.AreEqual(settings.pvp, true);
            Assert.AreEqual(settings.snooper_enabled, true);
            Assert.AreEqual(settings.hardcore, false);
            Assert.AreEqual(settings.enable_command_block, false);
            Assert.AreEqual(settings.spawn_npcs, true);
            Assert.AreEqual(settings.allow_flight, false);
            Assert.AreEqual(settings.spawn_animals, true);
            Assert.AreEqual(settings.white_list, false);
            Assert.AreEqual(settings.generate_structures, true);
            Assert.AreEqual(settings.online_mode, true);
            Assert.AreEqual(settings.enable_rcon, false);

            // String Properties:
            Assert.AreEqual(settings.generator_settings, "");
            Assert.AreEqual(settings.resource_pack_hash, "");
            Assert.AreEqual(settings.level_type, "DEFAULT");
            Assert.AreEqual(settings.server_ip, "");
            Assert.AreEqual(settings.level_name, "world");
            Assert.AreEqual(settings.level_seed, "");
            Assert.AreEqual(settings.motd, "A Minecraft Server");
        }

        [TestMethod]
        public void testGetBaseStatus()
        {
            MCServer server = new MCServer("UnitTestServer", "UnitTestFolderFail");
            Assert.IsNotNull(server.getBaseStatus());
        }

        [TestMethod]
        public void testCreation()
        {
            String ServerID = "UnitTestServer";
            String FolderDir = "UnitTestFolderFail";
            MCServer server = new MCServer(ServerID, FolderDir);
            Assert.IsNotNull(server);

            Assert.AreEqual(ServerID, server.ServerID);
            Assert.AreEqual(FolderDir, server.FolderDir);

            Assert.IsTrue(Directory.Exists(FolderDir));
        }

        /* Will need to use a mocking library to mock Process without running into large amount of errors
        *
        [TestMethod]
        public void testStop()
        {
            MCServer server = new MCServer("UnitTestServer", "UnitTestFolderFail");

            MockProcess pross = new MockProcess();
            pross.HasExited = false;

            server.pross = pross;
            
            server.stop();

            Assert.AreEqual(1, pross.CloseCalled);
        }

        [TestMethod]
        public void testKill()
        {
            MCServer server = new MCServer("UnitTestServer", "UnitTestFolderFail");

            MockProcess pross = new MockProcess();
            pross.HasExited = false;

            server.pross = pross;

            server.kill();

            Assert.AreEqual(1, pross.KillCalled);
        }
        */
    }
}
