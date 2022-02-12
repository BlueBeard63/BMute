using B.MedicalSystem.Models;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B.MedicalSystem.Simulate
{
    public class DownedSimulation
    {
        private Main Instance;

        public DownedSimulation(Main instance)
        {
            this.Instance = instance;
        }

        [Obsolete]
        public void DownedSim(UnturnedPlayer player, byte health)
        {
            if (player != null && health <= Main.Instance.Configuration.Instance.DownedHealth)
            {
                if (player.IsInVehicle)
                {
                    player.CurrentVehicle.forceRemovePlayer(out byte seat, player.CSteamID, out Vector3 pos, out byte angle);
                    VehicleManager.sendExitVehicle(player.CurrentVehicle, seat, pos, angle, true);
                }
                if (player.Player.stance.stance != EPlayerStance.PRONE)
                    player.Player.channel.send("tellStance", player.CSteamID, (ESteamPacket)15, new object[1]
                    {
                                (object) 5
                    });
                player.Player.equipment.dequip();
                player.Player.movement.sendPluginJumpMultiplier(0);
                player.Player.movement.sendPluginSpeedMultiplier(0);
                Main.Instance.DownedPlayers.Add(player.CSteamID, new DownedModel(DateTime.Now, Main.Instance.Configuration.Instance.DownedDuration));
            }
        }
    }
}
