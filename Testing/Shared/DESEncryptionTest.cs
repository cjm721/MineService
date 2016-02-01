using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineService_Shared;
using System;
using System.IO;
using System.Net.Sockets;

namespace UnitTestProject1.Shared
{
    [TestClass]
    public class DESEncryptionTest
    {
        [TestMethod]
        public void testEncryptionChainUsingTcp()
        {
            TcpListener server = new TcpListener(System.Net.IPAddress.Loopback, 55555);
            server.Start();
            TcpClient clientClient = new TcpClient();
            clientClient.ConnectAsync(System.Net.IPAddress.Loopback.ToString(), 55555);
            TcpClient serverClient = server.AcceptTcpClient();


            String testMessage = "This is a test message to send though the encrypter";
            IMessageControl control = new DESMessageControl();

            control.sendMessage(serverClient.GetStream(), testMessage);
            String returend = control.getMessage(clientClient.GetStream());

            Assert.AreEqual(testMessage, returend);
        }

        [TestMethod]
        public void testEncryptionMemory()
        {
            MemoryStream memory = new MemoryStream();
            
            String testMessage = "This is a test message to send though the encrypter";
            IMessageControl control = new DESMessageControl();

            control.sendMessage(memory, testMessage);

            memory.Position = 0; 

            String returend = control.getMessage(memory);

            Assert.AreEqual(testMessage, returend);
        }
    }
}
