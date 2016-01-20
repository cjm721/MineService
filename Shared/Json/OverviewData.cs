using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_JSON
{
    public class OverviewData : Message
    {
        public ServerStatus[] statuses;
        [JsonConverter(typeof(StringEnumConverter))]
        public States.StatusType statusType;

        public OverviewData(ServerStatus[] sts, States.StatusType typ)
        {
            statuses = sts;
            statusType = typ;
        }
    }
}