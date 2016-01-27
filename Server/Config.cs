using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Server
{
    [Serializable]
    public class Config
    {
        public static Config INSTANCE;

        const String fromEmail = "noreplymineservice@gmail.com";
        const String password = "juniorproject";

        private const long serialVersionUID = -1551183367719668451L;

        public int PORT = 56552;
        public String[,] ServerIDFolders = new String[,] { };

        public static void loadConfig()
        {
            Console.WriteLine("Loading Config.");

            if (!File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config.mineS"))
            {
                INSTANCE = new Config();
                System.Console.WriteLine("Generating Default Config");
                INSTANCE.saveConfig();
                return;
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config.mineS",
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            Config config = (Config)formatter.Deserialize(stream);
            stream.Close();


            INSTANCE = config;

            for(int i = 0; i < config.ServerIDFolders.GetLength(0); i++)
            {
                String ID = config.ServerIDFolders[i,0];
                String Folder = config.ServerIDFolders[i,1];

                Data.mcServers.Add(ID, new MCServer(ID, Folder));

            }
        }

        public void saveConfig()
        {
            updateServerIDFolders();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config.mineS",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();
        }

        public void updateServerIDFolders()
        {
            ServerIDFolders = new String[Data.mcServers.Count,2];

            int index = 0;
            foreach (String key in Data.mcServers.Keys)
            {
                ServerIDFolders[index,0] = key;
                ServerIDFolders[index,1] = Data.mcServers[key].FolderDir;
                index++;
            }
        }
    }
}
