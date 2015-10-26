using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client_JSON
{
    public class Status
    {
        public States.StatusType TYPE;
        public String ServerID;
        public ServerStatus serverStatus;

        public Status(States.StatusType TYPE, String ServerID, ServerStatus serverStatus)
        {
            this.TYPE = TYPE;
            this.ServerID = ServerID;
            this.serverStatus = serverStatus;
        }
    }
}
