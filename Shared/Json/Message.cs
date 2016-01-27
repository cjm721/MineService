using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MineService_JSON
{
    public class Message
    {
        public static readonly JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public String toJsonString()
        {   
            return JsonConvert.SerializeObject(this, settings);
        }

        public static Message fromJsonString(String msg)
        {
            return JsonConvert.DeserializeObject<Message>(msg, settings);
        }
    }
}
