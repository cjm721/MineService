using MineService_JSON;
using MineService_Shared.Json;
using System;
using System.IO;

namespace MineService_Server
{
    public class Config
    {
        public static Config INSTANCE;

        private MSConfig jsonConfig;

        public static String FileLocation
        {
            get
            {
                return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config.mineS";
            }
        }

        public static void loadConfig()
        {
            System.Console.WriteLine("Loading Config.");
            INSTANCE = new Config();


            if (!File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config.mineS"))
            {
                System.Console.WriteLine("Generating Default Config");
                INSTANCE.jsonConfig = new MSConfig();
                INSTANCE.saveConfig();
                return;
            }else
            {
                INSTANCE.jsonConfig = (MSConfig) Message.loadFromFile(FileLocation);

            }


            for(int i = 0; i < INSTANCE.jsonConfig.ServerIDFolders.GetLength(0); i++)
            {
                String ID = INSTANCE.jsonConfig.ServerIDFolders[i,0];
                String Folder = INSTANCE.jsonConfig.ServerIDFolders[i,1];

                Data.mcServers.Add(ID, new MCServer(ID, Folder));

            }

        }

        public void saveConfig()
        {
            updateServerIDFolders();

            jsonConfig.saveToFile(FileLocation);
        }

        public void updateServerIDFolders()
        {
            jsonConfig.ServerIDFolders = new String[Data.mcServers.Count,2];

            int index = 0;
            foreach (String key in Data.mcServers.Keys)
            {
                jsonConfig.ServerIDFolders[index,0] = key;
                jsonConfig.ServerIDFolders[index,1] = Data.mcServers[key].FolderDir;
                index++;
            }
        }
    }
}
