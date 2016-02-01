using System;

namespace MineService_Client
{
    public interface IMessageHandler
    {
        void handleMessage(String line);
    }
}
