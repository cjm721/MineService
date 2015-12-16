using System;
using System.Collections.Generic;

namespace MineService_Server
{
    public static class Data
    {
        public static Dictionary<String, MCServer> mcServers = new Dictionary<string, MCServer>();
        public static List<Client> connectedClients;
    }
}
