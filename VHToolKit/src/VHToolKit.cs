using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.IO;
using BepInEx.Configuration;

namespace VHToolKit
{
    [BepInPlugin(GUID, NAME , VERSION)]
    [HarmonyPatch]
    public class VHToolKit : BaseUnityPlugin
    {
        public const string GUID = "com.github.pedrobraga.vhtoolkit";
        public const string NAME = "VHToolKit";
        public const string VERSION = "0.4.1";

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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "IsEncumbered")]
        static void Player_IsEncumbered_PostPatch(ref bool __result)
        {
            if(Settings.ToolKit.Enabled.Value && Settings.ToolKit.NotEncumbered.Value)
                __result = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Skills), "OnDeath")]
        static bool Skills_OnDeath_PrefixPatch()
        {
            if (Settings.ToolKit.PreserveSkills.Value)
                return false;
            return true;
        }


    }
}
