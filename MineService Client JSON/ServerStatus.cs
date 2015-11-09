namespace MineService_Client_JSON
{
    public class ServerStatus
    {
        public bool isRunning;
        public long uptime;
        public ServerSettings settings;

        public ServerStatus(bool isRunning, long uptime)
        {
            this.isRunning = isRunning;
            this.uptime = uptime;
        }
    }
}