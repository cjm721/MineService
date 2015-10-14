using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client_JSON
{
    public class Message
    {
        public States.MessageTYPE type;
        public String message;

        public Message(States.MessageTYPE type, String message)
        {
            this.type = type;
            this.message = message;
        }

    }
}
