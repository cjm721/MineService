using MineService_Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace UnitTestProject1
{
    public class FakeMessageControl : IMessageControl
    {
        public String messageSent;
        public Stream stream;

        public void sendMessage(Stream stream, string message)
        {
            messageSent = message;
            this.stream = stream;
        }

        public string getMessage(Stream stream)
        {
            return "hello";
            //TODO: implement this
            //FIXME: what should i do for this?
        }
    }
}
