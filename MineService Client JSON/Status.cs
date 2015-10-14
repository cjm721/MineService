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
    }
}
