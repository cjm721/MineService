using MineService_JSON;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using Kajabity.Tools.Java;
using System.Timers;

namespace MineService_Server
{
    public class MCServer : AbstractServer
    {
        public ConcurrentQueue<String> consoleLines;


        public MCMSSettings msSettings;

        public TimeSpan uptime
        {
            get
            {
                if (isRunning())
                    return DateTime.Now - pross.StartTime;
                else
                {
                    return TimeSpan.Zero;
                }
            }
        }

        public MCServer(String ServerID, String FolderDir) : base(ServerID, FolderDir)
        {
            consoleLines = new ConcurrentQueue<String>();


            Directory.CreateDirectory(this.FolderDir);
            this.msSettings = LoadSettings();

            this.msSettings.saveSetting(this.FolderDir);
        }

        public override void onConsoleMessage(object sender, DataReceivedEventArgs e)
        {
            if(this.consoleLines.Count >= 100)
            {
                String temp;
                this.consoleLines.TryDequeue(out temp);
            }

            this.consoleLines.Enqueue(e.Data);
            System.Diagnostics.Debug.WriteLine("Console Output: " + e.Data);

            // TODO: Send to Clients
            MineService_JSON.Console toSend = new MineService_JSON.Console(this.serverID, new String[] { e.Data } );

            Client.SendMessageToAll(toSend.toJsonString());
        }

        public override void onServerStoped(object sender, EventArgs e)
        {
            base.onServerStoped(sender, e);

            // TODO: Send Update Message to Client
            Status status = new Status(States.StatusType.Send, ServerID, new ServerStatus(false, TimeSpan.Zero));
            Client.SendMessageToAll(status.toJsonString());


            if(!forcedStop && msSettings.RESTART_ON_CRASH)
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

            return (MCMSSettings) Message.fromJsonString(json);
        }

        public void rawCommand(string command)
        {
            if(isRunning())
                pross.StandardInput.Write(command);
        }

        public Status getBaseStatus()
        {
            ServerStatus serverStatus = new ServerStatus( isRunning(), this.uptime);
            Status status = new Status(States.StatusType.Send, this.ServerID, serverStatus);

            return status;
        }

        public MCServerSettings getServerSettings()
        {
            MCServerSettings toReturn = new MCServerSettings();


            // TODO: Implement rest;
            JavaProperties properties = new JavaProperties();

            String file = this.folderDir + "/server.properties";
            if (File.Exists(file))
            {
                try
                {
                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    properties.Load(fs);
                    fs.Close();
                }
                catch (IOException)
                {
                    System.Console.WriteLine("Unable to load " + this.serverID + "'s server.properties.");
                }
            }

            toReturn.spawn_protection = int.Parse(properties.GetProperty("spawn-protection", "16"));
            toReturn.max_tick_time = int.Parse(properties.GetProperty("max-tick-time", "60000"));
            toReturn.generator_settings = properties.GetProperty("generator-settings", "");
            toReturn.force_gamemode = Boolean.Parse(properties.GetProperty("force-gamemode", "false"));
            toReturn.allow_nether = Boolean.Parse(properties.GetProperty("allow-nether", "true"));
            toReturn.gamemode = int.Parse(properties.GetProperty("gamemode", "0"));
            toReturn.enable_query = Boolean.Parse(properties.GetProperty("enable-query", "false"));
            toReturn.player_idle_timeout = int.Parse(properties.GetProperty("player-idle-timeout", "0"));
            toReturn.difficulty = int.Parse(properties.GetProperty("difficulty", "1"));
            toReturn.spawn_monsters = Boolean.Parse(properties.GetProperty("spawn-monsters", "true"));
            toReturn.op_permission_level = int.Parse(properties.GetProperty("op-permission-level", "4"));
            toReturn.resource_pack_hash = properties.GetProperty("resource-pack-hash", "");
            toReturn.announce_player_achievements = Boolean.Parse(properties.GetProperty("announce-player-achievements", "true"));
            toReturn.pvp = Boolean.Parse(properties.GetProperty("pvp", "true"));
            toReturn.snooper_enabled = Boolean.Parse(properties.GetProperty("snooper-enabled", "true"));
            toReturn.level_type = properties.GetProperty("level-type", "DEFAULT");
            toReturn.hardcore = Boolean.Parse(properties.GetProperty("hardcore", "false"));
            toReturn.enable_command_block = Boolean.Parse(properties.GetProperty("enable-command-block", "false"));
            toReturn.max_players = int.Parse(properties.GetProperty("max-players", "20"));
            toReturn.network_compression_threshold = int.Parse(properties.GetProperty("network-compression-threshold", "256"));
            toReturn.max_world_size = int.Parse(properties.GetProperty("max-world-size", "29999984"));
            toReturn.server_port = int.Parse(properties.GetProperty("server-port", "25565"));
            toReturn.server_ip = properties.GetProperty("server-ip", "");
            toReturn.spawn_npcs = Boolean.Parse(properties.GetProperty("spawn-npcs", "true"));
            toReturn.allow_flight = Boolean.Parse(properties.GetProperty("allow-flight", "false"));
            toReturn.level_name = properties.GetProperty("level-name", "world");
            toReturn.view_distance = int.Parse(properties.GetProperty("view-distance", "10"));
            toReturn.resource_pack_hash = properties.GetProperty("resource-pack", "");
            toReturn.spawn_animals = Boolean.Parse(properties.GetProperty("spawn-animals", "true"));
            toReturn.white_list = Boolean.Parse(properties.GetProperty("white-list", "false"));
            toReturn.generate_structures = Boolean.Parse(properties.GetProperty("generate-structures", "true"));
            toReturn.online_mode = Boolean.Parse(properties.GetProperty("online-mode", "true"));
            toReturn.max_build_height = int.Parse(properties.GetProperty("max-build-height", "256"));
            toReturn.level_seed = properties.GetProperty("level-seed", "");
            toReturn.enable_rcon = Boolean.Parse(properties.GetProperty("enable-rcon", "false"));
            toReturn.motd = properties.GetProperty("motd", "A Minecraft Server");

            return toReturn;
        }

        public override Process getStartProcess()
        {
            if (!File.Exists(this.FolderDir + Path.DirectorySeparatorChar + msSettings.SERVER_JAR))
            {
                throw new FileNotFoundException("The server jar was not found.", this.FolderDir + Path.DirectorySeparatorChar + msSettings.SERVER_JAR);
            }

            Process pross = new Process();
            pross.StartInfo.WorkingDirectory = this.FolderDir;
            pross.StartInfo.UseShellExecute = false;
            pross.StartInfo.FileName = msSettings.JAVA_PATH;
            pross.StartInfo.Arguments = msSettings.getStartArgs();
            pross.StartInfo.RedirectStandardOutput = true;
            pross.StartInfo.RedirectStandardInput = true;
            pross.EnableRaisingEvents = true;
          //  pross.StandardOutput = new StreamReader();

            return pross;
        }

        public override void timerElapsed(object source, ElapsedEventArgs e)
        {
            TimeSpan difference = TimeSpan.Zero;
            if (isRunning())
            {
                difference = DateTime.Now - pross.StartTime;
            }

            Status stauts = new Status(States.StatusType.Send, ServerID, new ServerStatus(true, difference));

            Client.SendMessageToAll(stauts.toJsonString());
        }
    } 
}
