using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_Client.Tabs
{
    public class Settings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.StatusType type;

        public Settings(States.StatusType typ)
        {
            type = typ;
        }
    }
}