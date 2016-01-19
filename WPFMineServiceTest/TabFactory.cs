using MineService_JSON;
using System;
using System.Collections.Generic;

namespace MineService_Client.Tabs
{
    public class TabFactory
    {
        private Dictionary<String, String> map;

        public TabFactory()
        {
            map = new Dictionary<string, string>();

            OverviewData overview = new OverviewData(null, States.StatusType.Request);
            map.Add("overview_TabItem", overview.toJsonString());

            FTPData FTP = new FTPData(States.StatusType.Request);
            map.Add("FTP_TabItem", FTP.toJsonString());

            Settings settings = new Settings(States.StatusType.Request);
            map.Add("settings_TabItem", settings.toJsonString());

            Users users = new Users(States.StatusType.Request);
            map.Add("users_TabItem", users.toJsonString());
        }

        public String createRequestDataMsg(String name)
        {
            return map[name];
        }
    }
}
