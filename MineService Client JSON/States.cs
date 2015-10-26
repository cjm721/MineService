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
        [Flags]
        public enum MessageTYPE { MCCommand, Login, Status, Console };
        [Flags]
        public enum StatusType { Request, Send};
    }
}
