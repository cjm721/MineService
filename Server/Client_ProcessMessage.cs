﻿using MineService_JSON;
using MineService_Shared.Json;
using System;
using System.IO;

namespace MineService_Server
{
    partial class Client
    {
        public void handleMCCommand(MCCommand command)
        {
            MCServer server = Data.mcServers.ContainsKey(command.Server) ? Data.mcServers[command.Server] : null;

            switch (command.commandType)
            {
                case States.MCCommandTYPE.Start:
                    start(server);
                    break;
                case States.MCCommandTYPE.Restart:
                    if (server != null)
                    {
                        server.restart();
                    }
                    break;
                case States.MCCommandTYPE.Raw:
                    if (server != null)
                    {
                        server.rawCommand(command.args);
                    }
                    break;
                case States.MCCommandTYPE.Stop:
                    if (server != null)
                    {
                        server.stop();
                    }
                    break;
                case States.MCCommandTYPE.Create:
                    create(server, command);
                    break;
                case States.MCCommandTYPE.Delete:
                    delete(server);
                    break;

            }
        }

        private void handleConsole(MineService_JSON.Console console)
        {
            MCServer server = Data.mcServers[console.ServerID];

            if (server != null)
            {
                MineService_JSON.Console toSend = new MineService_JSON.Console(console.ServerID, server.consoleLines.ToArray());
                sendMessage(toSend.toJsonString());
            }
        }

        private void handleLogin(Login login)
        {
            /**
                Requires SQLite Setup Code to be complete
            **/
            Status[] statArray = new Status[Data.mcServers.Count];
            int count = 0;
            foreach (string key in Data.mcServers.Keys)
            {
                ServerStatus sStatus = new ServerStatus(Data.mcServers[key].isRunning(), Data.mcServers[key].uptime);
                sStatus.serverSettings = Data.mcServers[key].getServerSettings();
                Status stat = new Status(States.StatusType.Send, key, sStatus);
                statArray[count] = stat;
                count++;
            }
            this.authenticated = true;
            StatusArray sArray = new StatusArray(statArray);
            sendMessage(sArray.toJsonString());

            //bool exists = Data.database.checkForUser(login.Username, login.Password);
            //if (exists)
            //{
            //    Status[] statArray = new Status[Data.mcServers.size()];
            //    Set<String> keyset = Data.mcServers.keySet();
            //    int count = 0;
            //    for (String key : keyset)
            //    {
            //        ServerStatus sStatus = new ServerStatus(Data.mcServers.get(key).isRunning(), Data.mcServers.get(key).uptime);
            //        sStatus.settings = Data.mcServers.get(key).getServerSettings();
            //        Status stat = new Status(States.StatusType.Send, key, sStatus);
            //        statArray[count] = stat;
            //        count++;
            //    }
            //    Gson g = new Gson();
            //    String msg = g.toJson(statArray);
            //    Message toSend = new Message(MessageTYPE.StatusArray, msg);
            //    this.authenticated = true;
            //    SendMessage(g.toJson(toSend));
            //}
            //else
            //{
            //    Gson g = new Gson();
            //    String msg = g.toJson(new Message(MessageTYPE.Error, "User password combination doesn't exist"));
            //    SendMessage(msg);

            //    try
            //    {
            //        this.socket.close();
            //        Data.connectedClients.remove(this);
            //    }
            //    catch (IOException e)
            //    {
            //        System.out.println("Client Denied");
            //    }
            //}
        }

        private void handleStatus(Status status)
        {
            // TODO if it is a status request send info back.
        }

        private void start(MCServer server)
        {
            if (server != null)
            {
                // Check if server jar does not exist

                if (!File.Exists(server.FolderDir + Path.DirectorySeparatorChar + server.msSettings.SERVER_JAR))
                {
                    Message msg = new Error("Server jar file missing.");
                    sendMessage(msg.toJsonString());


                    Message toReply = server.getBaseStatus();
                    sendMessage(toReply.toJsonString());
                }
                else
                {
                    server.start();
                }
            }
        }

        private void create(MCServer server, MCCommand command)
        {
            if (server != null)
            {
                Message msg = new Error("Server already Exists");
                sendMessage(msg.toJsonString());
            }
            else
            {
                Message msg = null;

                switch (getMCCommandError(command))
                {
                    case 0:
                        MCServer newServer = new MCServer(command.Server, command.args);
                        Data.mcServers.Add(command.Server, newServer);
                        Config.INSTANCE.saveConfig();

                        Message newServerMsg = newServer.getBaseStatus();
                        SendMessageToAll(newServerMsg.toJsonString());
                        return;
                    case 1:
                        msg = new Error("Need server name");
                        break;
                    case 2:
                        msg = new Error("Need folder name");
                        break;
                    case 3:
                        msg = new Error("Need server and folder name");
                        break;
                }

                sendMessage(msg.toJsonString());
            }
        }

        /**
         * This method checks if the command server or args are null.
         * Bit shifts one of the values to the left and then performs bitwise or.
         * Returns 0, 1, 2, or 3 since only 2 bits are involved.
        */
        private int getMCCommandError(MCCommand cmd)
        {
            bool isServerNull = String.IsNullOrWhiteSpace(cmd.Server);
            bool isArgNull = String.IsNullOrWhiteSpace(cmd.args);

            return Convert.ToInt32(isServerNull) | ((Convert.ToInt32(isArgNull)) << 1);
        }

        private void delete(MCServer server)
        {
            if (server != null)
            {
                // TODO: Delete server, args is if to remove files also.
                server.delete(false);
                Config.INSTANCE.saveConfig();
            }
            else
            {
                // (Should never be able to hit this from the program unless two people delete at same time.
                Message msg = new Error("Cannot delete server does not exist");
                sendMessage(msg.toJsonString());
            }
        }
    }
}
