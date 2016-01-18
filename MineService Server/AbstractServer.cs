using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Server
{
    public class AbstractServer : IServer
    {
        Process pross;
        String folderDir;
        String serverID;

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

        public void start()
        {
            throw new NotImplementedException();
        }

        public void stop()
        {
            if (pross != null && !pross.HasExited)
                pross.Close();
        }
    }
}
