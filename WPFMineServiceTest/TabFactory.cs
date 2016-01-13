using MineService_JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MineService_Client.Tabs
{
    public class TabFactory
    {
        private Dictionary<String, String> map;

        public TabFactory()
        {
            OverviewData overview = new OverviewData(null, States.StatusType.Request);
            Message msg = new Message(States.MessageTYPE.OverviewData, JsonConvert.SerializeObject(overview));
            map.Add("overview_TabItem", JsonConvert.SerializeObject(msg));

            FTPData FTP = new FTPData(States.StatusType.Request);
            msg = new Message(States.MessageTYPE.FTPData, JsonConvert.SerializeObject(overview));
            map.Add("FTP_TabItem", JsonConvert.SerializeObject(msg));

            Settings settings = new Settings(States.StatusType.Request);
            msg = new Message(States.MessageTYPE.Settings, JsonConvert.SerializeObject(overview));
            map.Add("settings_TabItem", JsonConvert.SerializeObject(msg));

            Users users = new Users(States.StatusType.Request);
            msg = new Message(States.MessageTYPE.Users, JsonConvert.SerializeObject(overview));
            map.Add("users_TabItem", JsonConvert.SerializeObject(msg));
        }

        public String createRequestDataMsg(String name)
        {
            return map[name];
        }
    }
}
