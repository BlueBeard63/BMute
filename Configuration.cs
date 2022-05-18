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
        public bool UseUTC { get; set; }
        public bool RequireReason { get; set; }
        public bool MuteVoice { get; set; }
        public bool MuteChat { get; set; }
        public string WebhookLink { get; set; }

        public void LoadDefaults()
        {
            DatabaseConnection = "Server=127.0.0.1;Database=unturned;Uid=root;Password=Password123;";
            UseUTC = false;
            RequireReason = false;
            MuteVoice = false;
            MuteChat = false;
            WebhookLink = "Link HERE";
        }
    }
}
