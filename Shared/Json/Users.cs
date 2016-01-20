using MineService_JSON;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MineService_JSON
{
    public class Users : Message
    {
        public States.StatusType statusType;


        public Users(States.StatusType statusType)
        {
            this.statusType = statusType;
        }
    }
}