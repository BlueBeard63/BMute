using B.Mute.Database;
using B.Mute.Helper;
using Rocket.API.Collections;
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

namespace B.Mute
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }
        public DatabaseManager Manager { get; private set; }
        public DiscordMessager Messager { get; private set; }
        private System.Timers.Timer Timer { get; set; }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "DurationPermanent", "permanent" },
            { "RequireReason", "You must specify a reason!" },
            { "MuteInvalid", "Invalid usage, use: /mute <name/steamid> [reason] [duration]" },
            { "ReasonUnkown", "unknown" },
            { "TargetPlayerNotFound", "Player not found" },
            { "UnMuteAnnouncement", "{0} was unmuted!" },
            { "MuteAnnouncement", "{0} was muted by {1} for {2} for {3}!" }
        };

        protected override void Load()
        {
            Instance = this;
            Manager = new DatabaseManager(this);
            Manager.InitializeTables();

            Messager = new DiscordMessager(this);

            Timer = new Timer(Math.Max(Configuration.Instance.RefreshTime, 3000));
            Timer.Elapsed += ProcessAllMutes;
            Timer.Start();
        }

        private void ProcessAllMutes(object sender, ElapsedEventArgs e)
        {
            foreach(Mute.Models.MuteModel mute in Manager.GetAllMutes().Where(x => x.IsExpired == false && x.Length != null))
            {
                if (mute.IsExpired)
                {
                    Messager.SendMessage(Translate("UnMuteAnnouncement", mute.PlayerName), EMessageType.UnMute);
                    Manager.SetFlag(mute.MuteID);
                }
            }
        }

        protected override void Unload()
        {
            Instance = null;
            Manager = null;

            Timer.Elapsed -= ProcessAllMutes;
            Timer.Dispose();
        }
    }
}
