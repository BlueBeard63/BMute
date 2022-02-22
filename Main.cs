using B.MedicalSystem.Models;
using B.MedicalSystem.Simulate;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace B.MedicalSystem
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }
        public DownedSimulation DownedSimulation { get; private set; }
        public Dictionary<CSteamID, DownedModel> DownedPlayers = new Dictionary<CSteamID, DownedModel>();

        protected override void Load()
        {
            Instance = this;
            DownedSimulation = new DownedSimulation(this);

            UnturnedPlayerEvents.OnPlayerUpdateHealth += OnPlayerHealthUpdate;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerDead += OnPlayerDead;
            ItemManager.onTakeItemRequested += OnTakeItemRequested;
            U.Events.OnPlayerConnected += OnPlayerJoin;
            VehicleManager.onEnterVehicleRequested += OnEntervehicleRequested;
            PlayerStance.OnStanceChanged_Global += OnStanceChanged;
        }

        private void OnStanceChanged(PlayerStance obj)
        {
            var player = UnturnedPlayer.FromPlayer(obj.player);
            if (DownedPlayers.ContainsKey(player.CSteamID))
            {
                player.Player.channel.send("tellStance", player.CSteamID, (ESteamPacket)15, new object[1]
                {
                    (object)5
                });
            }
        }

        private void OnPlayerJoin(UnturnedPlayer player)
        {
            player.Player.equipment.onEquipRequested += OnEquip;
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (DownedPlayers.ContainsKey(player.CSteamID))
            {
                DownedPlayers.Remove(player.CSteamID);
                player.Suicide();
                DownedPlayers.Remove(player.CSteamID);
            }

            player.Player.equipment.onEquipRequested -= OnEquip;
        }

        private void OnEquip(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow)
        {
            if (DownedPlayers.ContainsKey(UnturnedPlayer.FromPlayer(equipment.player).CSteamID))
            {
                shouldAllow = false;
            }
        }

        private void OnEntervehicleRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            if (DownedPlayers.ContainsKey(UnturnedPlayer.FromPlayer(player).CSteamID))
            {
                shouldAllow = false;
            }
        }

        private void OnTakeItemRequested(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            if (DownedPlayers.ContainsKey(UnturnedPlayer.FromPlayer(player).CSteamID))
            {
                shouldAllow = false;
            }
        }

        private void OnPlayerDead(UnturnedPlayer player, Vector3 position)
        {
            DownedPlayers.Remove(player.CSteamID);
            DownedPlayers.Remove(player.CSteamID);
            player.Player.movement.sendPluginJumpMultiplier(1);
            player.Player.movement.sendPluginSpeedMultiplier(1);
        }

        private void OnPlayerHealthUpdate(UnturnedPlayer player, byte health)
        {
            DownedSimulation.DownedSim(player, health);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerJoin;
            UnturnedPlayerEvents.OnPlayerUpdateHealth -= OnPlayerHealthUpdate;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerDead -= OnPlayerDead;
            ItemManager.onTakeItemRequested -= OnTakeItemRequested;
            VehicleManager.onEnterVehicleRequested -= OnEntervehicleRequested;
            PlayerStance.OnStanceChanged_Global -= OnStanceChanged;
        }
    }
}
