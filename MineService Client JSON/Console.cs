﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client_JSON
{
    public class Console
    {
        public String ServerID;
        public String[] messages;

        public Console(String ServerID, String[] messages)
        {
            this.ServerID = ServerID;
            this.messages = messages;
        }
    }
}
