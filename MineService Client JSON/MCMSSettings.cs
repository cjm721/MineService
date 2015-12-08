using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client_JSON
{
    public class MCMSSettings
    {
        public String JAVA_PATH = "java";
        public String HEAP_MAX = "1G";
        public String HEAP_MIN = "1G";
        public String SERVER_JAR = "minecraft_server.jar";
        public String ADDITIONAL_ARGS = "";
        public bool RESTART_ON_CRASH = true;


        public String getStartArgs()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("-Xms" + HEAP_MIN + " ");
            sb.Append("-Xmx" + HEAP_MAX + " ");
            if(!String.IsNullOrWhiteSpace(ADDITIONAL_ARGS))
                sb.Append(ADDITIONAL_ARGS + " ");

            sb.Append("-jar " + SERVER_JAR);

            return sb.ToString();
        }

        public void saveSetting(String folder)
        {
            File.WriteAllText(folder + Path.DirectorySeparatorChar + "MSMC.json", JsonConvert.SerializeObject(this));
        }
    }
}
