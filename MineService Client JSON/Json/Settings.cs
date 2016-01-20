using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_JSON
{
    public class Settings : Message
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.StatusType statusType;

        public Settings(States.StatusType typ)
        {
            statusType = typ;
        }
    }
}