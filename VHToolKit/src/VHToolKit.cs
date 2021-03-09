using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;

namespace VHToolKit
{
    [BepInPlugin(GUID, NAME , VERSION)]
    [HarmonyPatch]
    public class VHToolKit : BaseUnityPlugin
    {
        public const string GUID = "com.github.valheimmods.pedrobraga.vhtoolkit";
        public const string NAME = "VHToolKit";
        public const string VERSION = "0.2.0";

        void Awake()
        {   
            Logger.LogInfo("Starting VHToolKit");
            Settings.Init(Config);

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "UseStamina")]
        static bool Player_UseStamina_PrefixPatch(ref Player __instance)
        {
            if (Settings.ToolKit.Enabled.Value && Settings.ToolKit.InfiniteStamina.Value)
                return false;
            return true;
        }

    }
}
