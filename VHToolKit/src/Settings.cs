using BepInEx.Configuration;
namespace VHToolKit
{
    class Settings
    {
        public static void Init(ConfigFile config)
        {
            ToolKit.Init(config);
        }

        public static class ToolKit
        {
            public static ConfigEntry<bool> Enabled { get; private set; }
            public static ConfigEntry<bool> InfiniteStamina { get; private set; } 
            public static ConfigEntry<bool> NotEncumbered { get; private set; }
            public static ConfigEntry<bool> PreserveSkills { get; private set; }
            public static ConfigEntry<bool> WaterWalker { get; set; }
            public static ConfigEntry<string> VHToolkit_Hotkey { get; set; }

            public static void Init(ConfigFile config)
            {
                const string name = "ToolKit";
                Enabled = config.Bind(name, "Enabled", true, "Activate the VHToolKit");
                InfiniteStamina = config.Bind(name, "InfiniteStamina", true, "Inifinite Stamina");
                NotEncumbered = config.Bind(name, "NotEncumbered", true, "Never Encumbered");
                PreserveSkills = config.Bind(name, "PreserveSkills", true, "Preserve skills after death");
                WaterWalker = config.Bind(name, "WaterWalk", true, "Walking on water");
                VHToolkit_Hotkey = config.Bind(name, "VHToolkit_Hotkey", "F4", "VHToolkit hotkey");
            }
        }
    }
}
