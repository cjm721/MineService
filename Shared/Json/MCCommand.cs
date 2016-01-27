using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_JSON
{
    public class MCCommand : Message
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.MCCommandTYPE commandType;
        public String Server;
        public String args;

        public MCCommand(States.MCCommandTYPE type, String Server, String args)
        {
            this.commandType = type;
            this.Server = Server;
            this.args = args;
        }
    }
}
