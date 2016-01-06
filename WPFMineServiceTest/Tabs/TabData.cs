using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineService_JSON;

namespace MineService_Client.Tabs
{
    public abstract class TabData
    {
        public abstract States.MessageTYPE getMessageType();
        public abstract String getMessage();
    }
}
