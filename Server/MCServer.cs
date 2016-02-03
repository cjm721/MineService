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

            // Int Properties:
            toReturn.spawn_protection = parseInt(properties, "spawn-protection", "16");
            toReturn.max_tick_time = parseInt(properties, "max-tick-time", "60000");
            toReturn.gamemode = parseInt(properties, "gamemode", "0");
            toReturn.player_idle_timeout = parseInt(properties, "player-idle-timeout", "0");
            toReturn.difficulty = parseInt(properties, "difficulty", "1");
            toReturn.op_permission_level = parseInt(properties, "op-permission-level", "4");
            toReturn.max_players = parseInt(properties, "max-players", "20");
            toReturn.network_compression_threshold = parseInt(properties, "network-compression-threshold", "256");
            toReturn.max_world_size = parseInt(properties, "max-world-size", "29999984");
            toReturn.server_port = parseInt(properties, "server-port", "25565");
            toReturn.view_distance = parseInt(properties, "view-distance", "10");
            toReturn.max_build_height = parseInt(properties, "max-build-height", "256");

            // Boolean Properties:
            toReturn.force_gamemode = parseBoolean(properties, "force-gamemode", "false");
            toReturn.allow_nether = parseBoolean(properties, "allow-nether", "true");
            toReturn.enable_query = parseBoolean(properties, "enable-query", "false");
            toReturn.spawn_monsters = parseBoolean(properties, "spawn-monsters", "true");
            toReturn.announce_player_achievements = parseBoolean(properties, "announce-player-achievements", "true");
            toReturn.pvp = parseBoolean(properties, "pvp", "true");
            toReturn.snooper_enabled = parseBoolean(properties, "snooper-enabled", "true");
            toReturn.hardcore = parseBoolean(properties, "hardcore", "false");
            toReturn.enable_command_block = parseBoolean(properties, "enable-command-block", "false");
            toReturn.spawn_npcs = parseBoolean(properties, "spawn-npcs", "true");
            toReturn.allow_flight = parseBoolean(properties, "allow-flight", "false");
            toReturn.spawn_animals = parseBoolean(properties, "spawn-animals", "true");
            toReturn.white_list = parseBoolean(properties, "white-list", "false");
            toReturn.generate_structures = parseBoolean(properties, "generate-structures", "true");
            toReturn.online_mode = parseBoolean(properties, "online-mode", "true");
            toReturn.enable_rcon = parseBoolean(properties, "enable-rcon", "false");

            // String Properties:
            toReturn.generator_settings = properties.GetProperty("generator-settings", "");
            toReturn.resource_pack_hash = properties.GetProperty("resource-pack-hash", "");
            toReturn.level_type = properties.GetProperty("level-type", "DEFAULT");
            toReturn.server_ip = properties.GetProperty("server-ip", "");
            toReturn.level_name = properties.GetProperty("level-name", "world");
            toReturn.resource_pack_hash = properties.GetProperty("resource-pack", "");
            toReturn.level_seed = properties.GetProperty("level-seed", "");
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

        private int parseInt(JavaProperties properties, String key, String defaultValue)
        {
            return int.Parse(properties.GetProperty(key, defaultValue));
        }

        private Boolean parseBoolean(JavaProperties properties, String key, String defaultValue)
        {
            return Boolean.Parse(properties.GetProperty(key, defaultValue));
        }
    } 
}
