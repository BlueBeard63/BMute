using B.Mute.Database;
using B.Mute.Helper;
using B.Mute.Models;
using B.Mute.Patches;
using HarmonyLib;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
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
using Logger = Rocket.Core.Logging.Logger;

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
            { "MuteAnnouncement", "{0} was muted by {1} for {2} for {3}!" },
            { "MutedMessage", "{0} you cannot speak as you are muted!"}
        };

        protected override void Load()
        {
            Instance = this;
            Manager = new DatabaseManager(this);
            Manager.InitializeTables();

            Messager = new DiscordMessager(this);

            var harmony = new HarmonyLib.Harmony("bluebeard.mute");
            harmony.PatchAll();

            UnturnedPlayerEvents.OnPlayerChatted += OnChat;
            PlayerVoicePatch.onHandle += OnVoiceChat;

            Timer = new Timer(Math.Max(Configuration.Instance.RefreshTime, 3000));
            Timer.Elapsed += ProcessAllMutes;
            Timer.Start();
        }

        private bool OnVoiceChat(PlayerVoice speaker, PlayerVoice listener)
        {
            var player = UnturnedPlayer.FromPlayer(speaker.player);
            if (Manager.GetAllMutes().Where(x => !x.SendFlag && x.Length != null).Any(x => x.PlayerID == player.CSteamID.m_SteamID))
            {
                UnturnedChat.Say(player, Translate("MutedMessage", player.DisplayName));
                return false;
            }
            return true;
        }

        private void OnChat(UnturnedPlayer player, ref Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            if(Manager.GetAllMutes().Where(x => !x.SendFlag && x.Length != null).Any(x => x.PlayerID == player.CSteamID.m_SteamID))
            {
                cancel = true;
                UnturnedChat.Say(player, Translate("MutedMessage", player.DisplayName));
            }
        }

        private void ProcessAllMutes(object sender, ElapsedEventArgs e)
        {
            foreach (MuteModel mute in Manager.GetAllMutes().Where(x => x.SendFlag == false && x.Length != null))
            {
                if (mute.IsExpired)
                {
                    ChatManager.serverSendMessage(Translate("UnMuteAnnouncement", mute.PlayerName), Color.green, null, null, EChatMode.GLOBAL, null, true);
                    Messager.SendMessage(EMessageType.Unmute, mute.PlayerName, mute.PlayerID.ToString(), mute.PunisherName, mute.ReasonString, mute.DurationString);
                    Manager.SetFlag(mute.MuteID);
                }
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerChatted -= OnChat;
            PlayerVoicePatch.onHandle -= OnVoiceChat;

            Timer.Elapsed -= ProcessAllMutes;
            Timer.Dispose();
        }
    }
}
