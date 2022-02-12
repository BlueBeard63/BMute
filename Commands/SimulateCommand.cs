using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MedicalSystem.Commands
{
    public class SimulateCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "simulate";

        public string Help => String.Empty;

        public string Syntax => String.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Length < 1)
            {
                UnturnedChat.Say(caller, "simulate <down>");
                return;
            }
            else
            {
                var lowerCommand = command[0].ToLower();

                if(lowerCommand == "down")
                {
                    try
                    {
                        var uPlayer = caller as UnturnedPlayer;
                        Main.Instance.DownedSimulation.DownedSim(uPlayer, 9);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, "simulate <down>");
                    return;
                }
            }
        }
    }
}
