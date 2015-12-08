using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Server;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestServerStart()
        {
            MCServer server = new MCServer("TestServer","TestFolder");

            server.start();
        }
    }
}
