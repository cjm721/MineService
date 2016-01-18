using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Server
{
    public interface IServer
    {
        void start();
        void stop();
        void restart();
        bool isRunning();
        void kill();
        void delete(bool diskAlso);
    }
}
