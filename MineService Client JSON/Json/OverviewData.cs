using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_Client.Tabs
{
    public class OverviewData
    {
        public ServerStatus[] statuses;
        [JsonConverter(typeof(StringEnumConverter))]
        public States.StatusType type;

        public OverviewData(ServerStatus[] sts, States.StatusType typ)
        {
            statuses = sts;
            type = typ;
        }
    }
}