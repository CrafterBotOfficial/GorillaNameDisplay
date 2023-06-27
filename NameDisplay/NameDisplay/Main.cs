using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Utilla;

namespace NameDisplay
{
    [BepInPlugin(Id, Name, Version)]
    [System.ComponentModel.Description("HauntedModMenu"), ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            Id = "crafterbot.nametag",
            Name = "NameTag",
            Version = "1.0.7";
        internal static Main Instance;

        internal ManualLogSource manualLogSource => Logger;
        internal bool InModded;

        internal ConfigEntry<bool> HideBadNames;

        internal Main()
        {
            Instance = this;
            manualLogSource.LogInfo($"Loaded {Name}");

            HideBadNames = Config.Bind("General", "HideBadNames", true, "Hide names that are on the auto-ban list");

            new HarmonyLib.Harmony(Id).PatchAll();
        }

        /* Ugly code below :P (but tbh the whole program is ugly:( */

        [ModdedGamemodeJoin]
        private void OnJoin() =>
            InModded = true;
        [ModdedGamemodeLeave]
        private void OnLeave() =>
            InModded = false;

        public void OnEnable()
        {
            manualLogSource.LogInfo("Enabled");
        }
        public void OnDisable()
        {
            manualLogSource.LogInfo("Disabled");
        }
    }
}
