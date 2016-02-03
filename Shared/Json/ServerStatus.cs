using System;

namespace MineService_JSON
{
    public class ServerStatus : Message
    {
        public bool isRunning;
        public TimeSpan uptime;
        public MCServerSettings serverSettings;

        public ServerStatus(bool isRunning, TimeSpan uptime)
        {
            this.isRunning = isRunning;
            this.uptime = uptime;
        }
    }
}