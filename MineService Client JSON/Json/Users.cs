using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_Client.Tabs
{
    public class Users
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.StatusType type;

        public Users(States.StatusType typ)
        {
            type = typ;
        }
    }
}