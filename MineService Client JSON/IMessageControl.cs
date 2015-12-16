using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Shared
{
    public interface IMessageControl
    {
        void sendMessage(NetworkStream stream, String message);
        String getMessage(NetworkStream stream);
    }
}
