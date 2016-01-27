using System;
using Newtonsoft.Json;
using System.IO;

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

        public void saveToFile(String s)
        {
            File.WriteAllText(s, this.toJsonString());
        }

        public static Message loadFromFile(String s)
        {
            return JsonConvert.DeserializeObject<Message>(File.ReadAllText(s), settings);
        }
    }
}
