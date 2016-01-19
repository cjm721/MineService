using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_JSON
{
    public class MCServerSettings : Message
    { 
        public bool enable_rcon;
        public bool white_list;
        public int spawn_protection;
        public int max_tick_time;
        public string generator_settings;
        public bool force_gamemode;
        public bool allow_nether;
        public int gamemode;
        public bool enable_query;
        public int player_idle_timeout;
        public int difficulty;
        public bool spawn_monsters;
        public int op_permission_level;
        public string resource_pack_hash;
        public bool announce_player_achievements;
        public bool pvp;
        public bool snooper_enabled;
        public string level_type;
        public bool hardcore;
        public bool enable_command_block;
        public int max_players;
        public int network_compression_threshold;
        public int max_world_size;
        public int server_port;
        public string server_ip;
        public bool spawn_npcs;
        public bool allow_flight;
        public string level_name;
        public int view_distance;
        public bool spawn_animals;
        public bool generate_structures;
        public bool online_mode;
        public int max_build_height;
        public string level_seed;
        public string motd;

    }
}
