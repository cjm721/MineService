using MineService_JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MineService_Server
{
    public class MCServer
    {
        public String ServerID;
        public String FolderDir;

        public ConcurrentQueue<String> consoleLines;

        public Process pross;

        public MCMSSettings msSettings;

        public StreamWriter ServerInput;

        public MCServer(String ServerID, String FolderDir)
        {
            this.ServerID = ServerID;
            this.FolderDir = FolderDir;

            consoleLines = new ConcurrentQueue<String>();


            Directory.CreateDirectory(this.FolderDir);
            this.msSettings = LoadSettings();

            this.msSettings.saveSetting(this.FolderDir);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void start()
        {
            if(!File.Exists(this.FolderDir + Path.DirectorySeparatorChar + msSettings.SERVER_JAR))
            {
                throw new FileNotFoundException("The server jar was not found.", this.FolderDir + Path.DirectorySeparatorChar + msSettings.SERVER_JAR);
            }

            Directory.SetCurrentDirectory(this.FolderDir);
            pross = new Process();
            pross.StartInfo.UseShellExecute = false;
            pross.StartInfo.FileName = msSettings.JAVA_PATH;
            pross.StartInfo.Arguments = msSettings.getStartArgs();
            pross.StartInfo.RedirectStandardOutput = true;
            pross.StartInfo.RedirectStandardInput = true;

            pross.Start();

            pross.Exited += onServerStoped;
            pross.OutputDataReceived += onConsoleMessage;

            ServerInput = this.pross.StandardInput;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void tryStop()
        {
            this.ServerInput.WriteLine("stop");

            // TODO Update Client
        }

        private void onConsoleMessage(object sender, DataReceivedEventArgs e)
        {
            if(this.consoleLines.Count >= 100)
            {
                String temp;
                this.consoleLines.TryDequeue(out temp);
            }

            this.consoleLines.Enqueue(e.Data);
            System.Diagnostics.Debug.WriteLine(e.Data);

            // TODO: Send to Clients


        }

        private void onServerStoped(object sender, EventArgs e)
        {
            // TODO: Send Update Message to Client

            if(msSettings.RESTART_ON_CRASH)
            {
                start();
            }
        }

        public MCMSSettings LoadSettings()
        {
            String path = FolderDir + Path.DirectorySeparatorChar + "MSMC.json";
            if (!File.Exists(path))
            {
                return new MCMSSettings();
            }

            String json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<MCMSSettings>(json);
        }

        public void handleCommand(MCCommand command)
        {
            switch(command.type)
            {
                case States.MCCommandTYPE.Start:
                    start();
                    break;

                case States.MCCommandTYPE.Stop:
                    tryStop();
                    break;
            }
        }
    } 
}
