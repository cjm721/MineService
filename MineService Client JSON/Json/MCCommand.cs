using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_JSON
{
    public class MCCommand
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.MCCommandTYPE type;
        public String Server;
        public String args;

        public MCCommand(States.MCCommandTYPE type, String Server, String args)
        {
            this.type = type;
            this.Server = Server;
            this.args = args;
        }
    }
}
