﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_JSON
{
    public class States
    {
        [Flags]
        public enum MCCommandTYPE { Start, Stop, Restart, Raw, Create, Delete };
        [Flags]
        public enum MessageTYPE { MCCommand, Login, Status, Console, Error, StatusArray };
        [Flags]
        public enum StatusType { Request, Send};
    }
}