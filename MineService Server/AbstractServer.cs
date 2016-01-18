using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Server
{
    public abstract class AbstractServer : IServer
    {
        public Process pross;
        public String folderDir;
        public String serverID;

        public int processID;
        public StreamWriter ServerInput;

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
            pross = getStartProcess();

            pross.Exited += onServerStoped;
            pross.OutputDataReceived += onConsoleMessage;

            pross.Start();
            processID = pross.Id;

            ServerInput = pross.StandardInput;
        }

        public void stop()
        {
            if (pross != null && !pross.HasExited)
                pross.Close();
        }

        public abstract Process getStartProcess();
        public abstract void onConsoleMessage(object sender, DataReceivedEventArgs e);
        public abstract void onServerStoped(object sender, EventArgs e);
    }
}
