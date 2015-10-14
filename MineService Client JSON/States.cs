using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client_JSON
{
    public class States
    {
        public enum MCCommandTYPE { Start, Stop, Restart, Raw };
        public enum MessageTYPE { Command, Login, Status, Console };
        public enum StatusType { Request, Send};
    }
}
