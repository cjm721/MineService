using MineService_JSON;

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
