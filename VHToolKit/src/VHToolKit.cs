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
        public const string GUID = "net.pedrobraga.vhtoolkit";
        public const string NAME = "VHToolKit";
        public const string VERSION = "1.1.0";
        void Awake()
        {   
            Logger.LogInfo("Starting VHToolKit");

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "UseStamina")]
        static bool Player_UseStamina_PrefixPatch(ref Player __instance)
        {
            return false;
        }


    }
}
