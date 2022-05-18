using B.Mute.Database;
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

        protected override void Load()
        {
            Instance = this;
            Manager = new DatabaseManager(this);
            Manager.InitializeTables();
        }

        protected override void Unload()
        {
            Instance = null;
        }
    }
}
