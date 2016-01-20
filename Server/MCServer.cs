using MineService_JSON;
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
    public class MCServer : AbstractServer
    {
        public ConcurrentQueue<String> consoleLines;


        public MCMSSettings msSettings;

        public long uptime;

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
            System.Diagnostics.Debug.WriteLine(e.Data);

            // TODO: Send to Clients


        }

        public override void onServerStoped(object sender, EventArgs e)
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

            return (MCMSSettings) Message.fromJsonString(json);
        }

        public void rawCommand(string command)
        {
            ServerInput.WriteLine(command);
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
            return toReturn;
            // TODO: Implement rest;
            /*
            Properties properties = new Properties();

            File settingsFile = new File(this.serverDir.getAbsolutePath() + File.separator + "server.properties");
            if (settingsFile.exists())
            {
                try
                {
                    properties.load(new FileInputStream(settingsFile));
                }
                catch (IOException e)
                {
                    System.out.println("Unable to load " + this.name + "'s server.properties.");
                }
            }

            toReturn.spawn_protection = Integer.parseInt(properties.getProperty("spawn-protection", "16"));
            toReturn.max_tick_time = Integer.parseInt(properties.getProperty("max-tick-time", "60000"));
            toReturn.generator_settings = properties.getProperty("generator-settings", "");
            toReturn.force_gamemode = Boolean.parseBoolean(properties.getProperty("force-gamemode", "false"));
            toReturn.allow_nether = Boolean.parseBoolean(properties.getProperty("allow-nether", "true"));
            toReturn.gamemode = Integer.parseInt(properties.getProperty("gamemode", "0"));
            toReturn.enable_query = Boolean.parseBoolean(properties.getProperty("enable-query", "false"));
            toReturn.player_idle_timeout = Integer.parseInt(properties.getProperty("player-idle-timeout", "0"));
            toReturn.difficulty = Integer.parseInt(properties.getProperty("difficulty", "1"));
            toReturn.spawn_monsters = Boolean.parseBoolean(properties.getProperty("spawn-monsters", "true"));
            toReturn.op_permission_level = Integer.parseInt(properties.getProperty("op-permission-level", "4"));
            toReturn.resource_pack_hash = properties.getProperty("resource-pack-hash", "");
            toReturn.announce_player_achievements = Boolean.parseBoolean(properties.getProperty("announce-player-achievements", "true"));
            toReturn.pvp = Boolean.parseBoolean(properties.getProperty("pvp", "true"));
            toReturn.snooper_enabled = Boolean.parseBoolean(properties.getProperty("snooper-enabled", "true"));
            toReturn.level_type = properties.getProperty("level-type", "DEFAULT");
            toReturn.hardcore = Boolean.parseBoolean(properties.getProperty("hardcore", "false"));
            toReturn.enable_command_block = Boolean.parseBoolean(properties.getProperty("enable-command-block", "false"));
            toReturn.max_players = Integer.parseInt(properties.getProperty("max-players", "20"));
            toReturn.network_compression_threshold = Integer.parseInt(properties.getProperty("network-compression-threshold", "256"));
            toReturn.max_world_size = Integer.parseInt(properties.getProperty("max-world-size", "29999984"));
            toReturn.server_port = Integer.parseInt(properties.getProperty("server-port", "25565"));
            toReturn.server_ip = properties.getProperty("server-ip", "");
            toReturn.spawn_npcs = Boolean.parseBoolean(properties.getProperty("spawn-npcs", "true"));
            toReturn.allow_flight = Boolean.parseBoolean(properties.getProperty("allow-flight", "false"));
            toReturn.level_name = properties.getProperty("level-name", "world");
            toReturn.view_distance = Integer.parseInt(properties.getProperty("view-distance", "10"));
            toReturn.resource_pack_hash = properties.getProperty("resource-pack", "");
            toReturn.spawn_animals = Boolean.parseBoolean(properties.getProperty("spawn-animals", "true"));
            toReturn.white_list = Boolean.parseBoolean(properties.getProperty("white-list", "false"));
            toReturn.generate_structures = Boolean.parseBoolean(properties.getProperty("generate-structures", "true"));
            toReturn.online_mode = Boolean.parseBoolean(properties.getProperty("online-mode", "true"));
            toReturn.max_build_height = Integer.parseInt(properties.getProperty("max-build-height", "256"));
            toReturn.level_seed = properties.getProperty("level-seed", "");
            toReturn.enable_rcon = Boolean.parseBoolean(properties.getProperty("enable-rcon", "false"));
            toReturn.motd = properties.getProperty("motd", "A Minecraft Server");

            return toReturn;
            */
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

            return pross;
        }
    } 
}
