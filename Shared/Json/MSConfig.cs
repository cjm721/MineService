using MineService_JSON;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MineService_Shared.Json
{
    public class MSConfig : Message
    {
        public String fromEmail = "noreplymineservice@gmail.com";
        public String password = "juniorproject";

        public int PORT = 56552;
        public String[,] ServerIDFolders = new String[,] { };
    }
}
