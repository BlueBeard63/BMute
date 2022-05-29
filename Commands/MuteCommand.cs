using B.Mute.Database;
using B.Mute.Helper;
using B.Mute.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B.Mute.Commands
{
    public class MuteCommand : IRocketCommand
    {
        private Main pluginInstance => Main.Instance;

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "mute";

        public string Help => String.Empty;

        public string Syntax => String.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] args)
        {
            MuteModel mute = new MuteModel();

            if(args.Length < 1)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("MuteInvalid"), Color.red);
                return;
            }

            UnturnedPlayer target = UnturnedPlayer.FromName(args[0]);
            if(target == null)
            {
                if(ulong.TryParse(args[0], out ulong num))
                {
                    mute.PlayerID = num;
                }
                else
                {
                    UnturnedChat.Say(caller, pluginInstance.Translate("TargetNotFound"), Color.red);
                    return;
                }
            }
            else
            {
                mute.PlayerID = target.CSteamID.m_SteamID;
            }

            mute.PunisherID = caller is ConsolePlayer ? 0 : ulong.Parse(caller.Id);
            mute.PunisherName = caller is ConsolePlayer ? "System" : UnturnedPlayer.FromCSteamID(new Steamworks.CSteamID(ulong.Parse(caller.Id))).DisplayName;
            mute.PlayerName = target == null ? "Unknown" : target.DisplayName;
            mute.Reason = args.ElementAtOrDefault(1);
            mute.Length = CommandsHelper.ConvertToBanDuration(args.Skip(2));
            mute.MuteCreated = pluginInstance.Configuration.Instance.UseUTC ? DateTime.UtcNow : DateTime.Now;

            if (pluginInstance.Configuration.Instance.RequireReason && mute.Reason == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("RequireReason"), Color.red);
                return;
            }

            try
            {
                ChatManager.serverSendMessage(Main.Instance.Translate("MuteAnnouncement", mute.PlayerName, mute.PunisherName, mute.ReasonString, mute.DurationString), Color.green, null, null, EChatMode.GLOBAL, null, true);
                Main.Instance.Messager.SendMessage(EMessageType.Mute, mute.PlayerName, mute.PlayerID.ToString(), mute.PunisherName, mute.ReasonString, mute.DurationString);
                pluginInstance.Manager.InsertMute(mute);
            }
            catch(Exception ex)
            {
                Rocket.Core.Logging.Logger.LogException(ex, "An error occurated while processing mute");
            }
        }
    }
}
