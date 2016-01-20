namespace MineService_JSON
{
    public class ServerStatus : Message
    {
        public bool isRunning;
        public long uptime;
        public MCServerSettings serverSettings;

        public ServerStatus(bool isRunning, long uptime)
        {
            this.isRunning = isRunning;
            this.uptime = uptime;
        }
    }
}