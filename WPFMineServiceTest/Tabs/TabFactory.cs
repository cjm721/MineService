using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client.Tabs
{
    public class TabFactory
    {
        private Dictionary<String, TabData> map;

        public TabFactory()
        {
            map.Add("overview_TabItem", new OverviewTabData());
            map.Add("FTP_TabItem", new FTPTabData());
            map.Add("users_TabItem", new UsersTabData());
            map.Add("settings_TabItem", new SettingsTabData());
            map.Add("Status_TabItem", new StatusTabData());
            map.Add("Console_TabItem", new ConsoleTabData());
            map.Add("Schedule_TabItem", new ScheduleTabData());
        }

        public TabData createTabData(String name)
        {
            return map[name];
        }
    }
}
