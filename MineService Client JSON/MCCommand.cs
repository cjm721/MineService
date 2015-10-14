using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client_JSON
{
    public class MCCommand
    { 
        public States.MCCommandTYPE type;
        public String Server;
        public String args;
    }
}
