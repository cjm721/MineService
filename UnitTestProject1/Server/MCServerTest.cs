using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Thread.Sleep(2000);

            Assert.IsTrue(server.isRunning());

            server.kill();
            Thread.Sleep(500);
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
            Assert.IsNotNull(server.getServerSettings());
        }

        [TestMethod]
        public void testGetBastStatus()
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
