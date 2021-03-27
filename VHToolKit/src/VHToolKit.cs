using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Scripting;
using System.IO;
using BepInEx.Configuration;
using System.Reflection;
using System.Collections.Generic;

namespace VHToolKit
{
    [BepInPlugin(GUID, NAME , VERSION)]
    [HarmonyPatch]
    public class VHToolKit : BaseUnityPlugin
    {

		[HarmonyPatch(typeof(Character), "UpdateWater")]
		public static class Swim_Patch
		{
			private static void Postfix(Character __instance, ZSyncAnimation ___m_zanim, Rigidbody ___m_body, float ___m_waterLevel, Vector3 ___m_moveDir)
			{
				if (!Settings.ToolKit.Enabled.Value || !Settings.ToolKit.WaterWalker.Value || !__instance.IsPlayer() || !(Player.m_localPlayer == __instance) || __instance.IsDead())
				{
					return;
				}
				ZSyncAnimation value = Traverse.Create((object)__instance).Field("m_zanim").GetValue<ZSyncAnimation>();
				if (___m_waterLevel + 0.08f > __instance.transform.position.y)
				{
					IsInWaterWalk = true;
					__instance.m_flying = true;
					float x = __instance.transform.transform.position.x;
					float z = __instance.transform.transform.position.z;
					__instance.transform.transform.position = new Vector3(x, ___m_waterLevel, z);
					if (___m_moveDir.magnitude > 0.1f)
					{
						__instance.transform.rotation = Quaternion.LookRotation(__instance.GetMoveDir());
					}
					if (Input.GetKey(KeyCode.Space) && IsInWaterWalk)
					{
						IsInWaterWalk = false;
						__instance.m_flying = false;
						__instance.transform.transform.position = new Vector3(x, ___m_waterLevel + 0.2f, z);
						Traverse.Create((Character)Player.m_localPlayer).Field("m_lastGroundTouch").SetValue(0f);
						Player.m_localPlayer.Jump();
					}
				}
				else
				{
					IsInWaterWalk = false;
					__instance.m_flying = false;
				}
			}
		}

		[HarmonyPatch(typeof(Character), "UpdateFlying")]
		public static class UpdateFlying_Patch
		{
			private static void Postfix(Character __instance, ZSyncAnimation ___m_zanim)
			{
				if (IsInWaterWalk)
				{
					___m_zanim.SetBool((int)typeof(Character).GetField("onGround", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null), value: true);
				}
			}
		}

        public const string GUID = "com.github.pedrobraga.vhtoolkit";
        public const string NAME = "VHToolKit";
        public const string VERSION = "0.6.0";
		
		public static bool IsInWaterWalk;

		public static Dictionary<string, KeyCode> map = new Dictionary<string, KeyCode>()
		{
			{ "f1", KeyCode.F1 },
			{ "f2", KeyCode.F2 },
			{ "f3", KeyCode.F3 },
			{ "f4", KeyCode.F4 },
		};

		void Awake()
        {   
            Logger.LogInfo("VHToolkit loaded");
            Settings.Init(Config);
			IsInWaterWalk = false;

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

		private void Update()
		{
			Player localPlayer = Player.m_localPlayer;
			if (Input.GetKeyDown(map[Settings.ToolKit.VHToolkit_Hotkey.Value.ToLower()]))
			{
				Settings.ToolKit.Enabled.Value = !Settings.ToolKit.Enabled.Value;
				if (Settings.ToolKit.Enabled.Value)
				{
					localPlayer.Message(MessageHud.MessageType.TopLeft, "VHToolkit is ON");
				}
				else
				{
					localPlayer.Message(MessageHud.MessageType.TopLeft, "VHToolkit is OFF");
				}
				if (!Settings.ToolKit.WaterWalker.Value)
				{
					localPlayer.m_flying = false;
					IsInWaterWalk = false;
				}
			}
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
