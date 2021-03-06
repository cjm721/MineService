﻿using MineService_JSON;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MineService_Client
{
    /// <summary>
    /// Interaction logic for ServerTabItem.xaml
    /// </summary>
    public partial class ServerTabItem : UserControl
    {
        public String ServerID;

        public ServerTabItem(String ServerID) : this()
        {
            this.ServerID = ServerID;

            this.server_name_TextBlock.Text = ServerID;
        }

        public ServerTabItem()
        {
            InitializeComponent();

            this.start_stop_button.Click += Start_stop_button_Click;
        }

        private void Start_stop_button_Click(object sender, RoutedEventArgs e)
        {
            States.MCCommandTYPE state = States.MCCommandTYPE.Start;

            if (start_stop_button.Content.ToString().Contains("Stop"))
            {
                state = States.MCCommandTYPE.Stop;
            }
            else if (start_stop_button.Content.ToString().Contains("Start"))
            {
                state = States.MCCommandTYPE.Start;
            }

            MCCommand command = new MCCommand(state, this.ServerID, "");
            System.Console.WriteLine("command: " + command.commandType);
            String js = command.toJsonString();
            System.Console.WriteLine("before sending to server, js: " + js.ToString());
            CommunicationClient.INSTANCE.sendToServer(js);
            System.Console.WriteLine("after send to server");

            start_stop_button.Content = "Pending";
            start_stop_button.IsEnabled = false;
        }

        public void UpdateTab(ServerStatus sStatus)
        {
            start_stop_button.Content = (sStatus.isRunning) ? "Stop Server" : "Start Server";
            start_stop_button.IsEnabled = true;

            if (sStatus.isRunning)
            {
                if (sStatus.uptime.Equals(TimeSpan.Zero))
                {
                    aliveTimeText.Text = "Starting";
                }
                else
                {
                    // @"dd\D\ hh\H\ mm\M\ ss\S"
                    aliveTimeText.Text = sStatus.uptime.ToString(@"dd\D\ hh\H\ mm\M\ ss\S");
                }
            }
            else
            {
                aliveTimeText.Text = "Offline";
            }

            if (sStatus.serverSettings != null)
            {
                MCServerSettings sSet = sStatus.serverSettings; //For the settings tab, need to display the values
                populate_settings(sSet);
            }
        }

        private void populate_settings(MCServerSettings sSet)
        {
            this.enable_rcon.IsChecked = sSet.enable_rcon;
            this.white_list.IsChecked = sSet.white_list;
            this.spawn_protection.Text = sSet.spawn_protection.ToString();
            this.max_tick_time.Text = sSet.max_tick_time.ToString();
            this.spawn_protection.Text = sSet.spawn_protection.ToString();
            this.max_tick_time.Text = sSet.max_tick_time.ToString();
            this.generator_settings.Text = sSet.generator_settings;
            this.force_gamemode.IsChecked = sSet.force_gamemode;
            this.allow_nether.IsChecked = sSet.allow_nether;
            this.gamemode.Text = sSet.gamemode.ToString();
            this.enable_query.IsChecked = sSet.enable_query;
            this.player_idle_timeout.Text = sSet.player_idle_timeout.ToString();
            this.difficulty.Text = sSet.difficulty.ToString();
            this.spawn_monsters.IsChecked = sSet.spawn_monsters;
            this.op_permission_level.Text = sSet.op_permission_level.ToString();
            this.resource_pack_hash.Text = sSet.resource_pack_hash;
            this.announce_player_achievements.IsChecked = sSet.announce_player_achievements;
            this.pvp.IsChecked = sSet.pvp;
            this.snooper_enabled.IsChecked = sSet.snooper_enabled;
            this.level_type.Text = sSet.level_type;
            this.hardcore.IsChecked = sSet.hardcore;
            this.enable_command_block.IsChecked = sSet.enable_command_block;
            this.max_players.Text = sSet.max_players.ToString();
            this.network_compression_threshold.Text = sSet.network_compression_threshold.ToString();
            this.max_world_size.Text = sSet.max_world_size.ToString();
            this.server_port.Text = sSet.server_port.ToString();
            this.server_ip.Text = sSet.server_ip;
            this.spawn_npcs.IsChecked = sSet.spawn_npcs;
            this.allow_flight.IsChecked = sSet.allow_flight;
            this.level_name.Text = sSet.level_name;
            this.view_distance.Text = sSet.view_distance.ToString();
            this.spawn_animals.IsChecked = sSet.spawn_animals;
            this.generate_structures.IsChecked = sSet.generate_structures;
            this.online_mode.IsChecked = sSet.online_mode;
            this.max_build_height.Text = sSet.max_build_height.ToString();
            this.level_seed.Text = sSet.level_seed;
            this.motd.Text = sSet.motd;
        }

        private void consoleSendButton_Click(object sender, RoutedEventArgs e)
        {
            MCCommand mcCommand = new MCCommand(States.MCCommandTYPE.Raw, this.ServerID, consoleTextInputBox.Text + "\n");
            consoleTextInputBox.Text = "";

            CommunicationClient.INSTANCE.sendToServer(mcCommand.toJsonString());
        }
    }
}
