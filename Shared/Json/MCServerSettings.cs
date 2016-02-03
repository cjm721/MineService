using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_JSON
{
    public class MCServerSettings : Message
    {
        // Int Properties:
        public int spawn_protection;
        public int max_tick_time;
        public int gamemode;
        public int player_idle_timeout;
        public int difficulty;
        public int op_permission_level;
        public int max_players;
        public int network_compression_threshold;
        public int max_world_size;
        public int server_port;
        public int view_distance;
        public int max_build_height;

        // Boolean Properties:
        public bool enable_rcon;
        public bool white_list;
        public bool force_gamemode;
        public bool allow_nether;
        public bool enable_query;
        public bool spawn_monsters;
        public bool announce_player_achievements;
        public bool pvp;
        public bool snooper_enabled;
        public bool hardcore;
        public bool enable_command_block;
        public bool spawn_npcs;
        public bool allow_flight;
        public bool spawn_animals;
        public bool generate_structures;
        public bool online_mode;


        // String Properties:
        public string generator_settings;
        public string resource_pack_hash;
        public string level_type;
        public string server_ip;
        public string level_name;
        public string level_seed;
        public string motd;
    }
}
