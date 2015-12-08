using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Server
{
    public static class Data
    {
        public static Dictionary<String, MCServer> mcServers = new Dictionary<string, MCServer>();
        public static List<Client> connectedClients;
    }
}
