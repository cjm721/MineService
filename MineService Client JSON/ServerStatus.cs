namespace MineService_Client_JSON
{
    public class ServerStatus
    {
        public bool isRunning;
        public ServerSettings settings;

        public ServerStatus(bool isRunning)
        {
            this.isRunning = isRunning;
        }
    }
}