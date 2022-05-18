using HarmonyLib;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.Mute.Patches
{
    [HarmonyPatch(typeof(PlayerVoice), "handleRelayVoiceCulling_Proximity")]
    internal class PlayerVoicePatch
    {
        internal static Handle onHandle;

        [HarmonyPrefix]
        private static bool handler(PlayerVoice speaker, PlayerVoice listener) => onHandle.Invoke(speaker, listener);

        internal delegate bool Handle(PlayerVoice speaker, PlayerVoice listener);
    }
}
