﻿using System;
using System.Diagnostics;
using System.Timers;

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
        Process getStartProcess();
        void onConsoleMessage(object sender, DataReceivedEventArgs e);
        void onServerStoped(object sender, EventArgs e);
        void timerElapsed(Object source, ElapsedEventArgs e);
    }
}
