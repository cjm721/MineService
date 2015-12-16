namespace MineService_JSON
{
    public class ServerStatus
    {
        public bool isRunning;
        public long uptime;
        public MCServerSettings settings;

        public ServerStatus(bool isRunning, long uptime)
        {
            this.isRunning = isRunning;
            this.uptime = uptime;
        }
    }
}