using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Timers;

namespace MineService_Server
{
    public abstract class AbstractServer : IServer
    {
        public Process pross;
        public String folderDir;
        public String serverID;
        public Timer timer;
        public bool forcedStop;

        public int processID;

        public String ServerID
        {
            get
            {
                return serverID;
            }
        }

        public String FolderDir
        {
            get
            {
                return folderDir;
            }
        }

        public AbstractServer(String ServerID, String FolderDir)
        {
            this.serverID = ServerID;
            this.folderDir = FolderDir;

            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += timerElapsed;
        }

        public void delete(bool diskAlso)
        {
            if (pross != null && !pross.HasExited)
            {
                pross.Kill();
            }

            Data.mcServers.Remove(this.ServerID);

            if (diskAlso)
            {
                Directory.Delete(this.FolderDir, true);
            }
        }

        public bool isRunning()
        {
            try
            {
                Process process = Process.GetProcessById(pross.Id);
                return process != null;
            }
            catch
            {
                return false;
            }
        }

        public void kill()
        {
            if (pross != null)
                pross.Kill();
        }

        public void restart()
        {
            stop();
            start();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void start()
        {
            if (isRunning())
            {
                return;
            }
            forcedStop = false;

            pross = getStartProcess();

            pross.Exited += onServerStoped;
            pross.OutputDataReceived += onConsoleMessage;

            pross.Start();
            pross.BeginOutputReadLine();
            processID = pross.Id;

            timer.Start();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void stop()
        {
            if (pross != null && !pross.HasExited)
            {
                forcedStop = true;
                pross.StandardInput.WriteLine("stop");
            }
        }

        public virtual void onServerStoped(object sender, EventArgs e)
        {
            timer.Stop();
        }

        public abstract Process getStartProcess();
        public abstract void onConsoleMessage(object sender, DataReceivedEventArgs e);
        public abstract void timerElapsed(object source, ElapsedEventArgs e);
    }
}
