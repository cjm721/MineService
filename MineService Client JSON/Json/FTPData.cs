using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_Client.Tabs
{
    public class FTPData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.StatusType type;

        public FTPData(States.StatusType typ)
        {
            type = typ;
        }
    }
}