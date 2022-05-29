using B.Mute.Database;
using B.Mute.Helper;
using B.Mute.Models;
using Rocket.API;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B.Mute.Commands
{
    public class UnMuteCommand : IRocketCommand
    {
        private Main pluginInstance => Main.Instance;

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "unmute";

        public string Help => String.Empty;

        public string Syntax => String.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length < 1)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("UnMuteInvalid"), Color.red);
                return;
            }

            UnturnedPlayer target = UnturnedPlayer.FromName(args[0]);
            if (target == null)
            {
                if (!ulong.TryParse(args[0], out ulong num))
                {
                    UnturnedChat.Say(caller, pluginInstance.Translate("TargetPlayerNotFound"), Color.red);
                    return;
                }
            }

            try
            {
                MuteModel mute = pluginInstance.Manager.GetMute(target.CSteamID.m_SteamID);

                string message;

                if (mute == null)
                {
                    message = pluginInstance.Translate("TargetPlayerNotFound");
                }
                else if (mute != null)
                {
                    pluginInstance.Manager.SetFlag(mute.MuteID);

                    message = pluginInstance.Translate("UnMuteAnnouncement", mute.PlayerName);
                    pluginInstance.Messager.SendMessage(EMessageType.Unmute, mute.PlayerName, mute.PlayerID.ToString(), mute.PunisherName, mute.ReasonString, mute.DurationString);
                }
                else
                {
                    message = pluginInstance.Translate("UnMuteFail", mute.PlayerName);
                }

                TaskDispatcher.QueueOnMainThread(() =>
                {
                    UnturnedChat.Say(caller, message);
                });
            }
            catch(Exception ex)
            {
                Rocket.Core.Logging.Logger.LogException(ex);
            }
        }
    }
}
