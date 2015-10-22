using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MineService_Client_JSON
{
    public class Message
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.MessageTYPE type;
        public String message;

        public Message(States.MessageTYPE type, String message)
        {
            this.type = type;
            this.message = message;
        }

    }
}
