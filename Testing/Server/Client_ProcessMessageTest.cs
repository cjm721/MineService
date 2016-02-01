using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1.Mock_Fake;

namespace UnitTestProject1.Server
{
    [TestClass]
    public class Client_ProcessMessageTest
    {
        [TestMethod]
        public void TestCreateExistingServer()
        {
            MineService_Server.Client client = new FakeClient();
            MethodInfo methodInfo = typeof(MineService_Server.Client).GetMethod("create", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(client, new Object[] { new MCServer("test", "test"), null });

            Assert.IsTrue(((FakeClient)client).message.Contains("Server already Exists"));
        }

        [TestMethod]
        public void TestCreateServerWithoutArgs()
        {
            MineService_Server.Client client = new FakeClient();
            MethodInfo methodInfo = typeof(MineService_Server.Client).GetMethod("create", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(client, new Object[] { null, new MineService_JSON.MCCommand(MineService_JSON.States.MCCommandTYPE.Create, "server", "") });
            
            Assert.IsTrue(((FakeClient)client).message.Contains("Need folder name"));
        }

        [TestMethod]
        public void TestCreateServerWithoutName()
        {
            MineService_Server.Client client = new FakeClient();
            MethodInfo methodInfo = typeof(MineService_Server.Client).GetMethod("create", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(client, new Object[] { null, new MineService_JSON.MCCommand(MineService_JSON.States.MCCommandTYPE.Create, "", "arg") });

            Assert.IsTrue(((FakeClient)client).message.Contains("Need server name"));
        }

        [TestMethod]
        public void TestCreateServerWithoutNameAndArgs()
        {
            MineService_Server.Client client = new FakeClient();
            MethodInfo methodInfo = typeof(MineService_Server.Client).GetMethod("create", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo.Invoke(client, new Object[] { null, new MineService_JSON.MCCommand(MineService_JSON.States.MCCommandTYPE.Create, "", "") });

            Assert.IsTrue(((FakeClient)client).message.Contains("Need server and folder name"));
        }
    }
}
