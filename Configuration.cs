using B.Mute.Models;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace B.Mute
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string DatabaseConnection { get; set; }
        public List<Webhook> Webhooks { get; set; }
        public double RefreshTime { get; set; }
        public bool UseUTC { get; set; }
        public bool RequireReason { get; set; }
        public bool MuteVoice { get; set; }
        public bool BroadcastMuteAndUnMute { get; set; }

        public void LoadDefaults()
        {
            DatabaseConnection = "Server=127.0.0.1;Database=unturned;Uid=root;Password=Password123;";
            Webhooks = new List<Webhook>()
            {
                new Webhook("Mute", "", "#00FFFF", "Name: {name}, SteamID: {steamid}, Punisher: {punisher}, Duration: {duration}, Reason: {reason}"),
                new Webhook("Unmute", "", "#808080", "Name: {name}, SteamID: {steamid}, Staff Member: {punisher}, Reason: {reason}")
            };
            RefreshTime = 50000;
            UseUTC = false;
            RequireReason = false;
            MuteVoice = false;
            BroadcastMuteAndUnMute = true;
        }
    }
}
