using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_JSON
{
    public class FTPData : Message
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public States.StatusType statysType;

        public FTPData(States.StatusType typ)
        {
            statysType = typ;
        }
    }
}