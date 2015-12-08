using MineService_Client_JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Server
{
    partial class Client
    {
        public void handleMCCOmmand(MCCommand command)
        {
            if (Data.mcServers.ContainsKey(command.Server))
            {
                MCServer server = Data.mcServers[command.Server];

                server.handleCommand(command);
            }

                

            
        }

        public void handleLogin(Login login)
        {

        }

    }
}
