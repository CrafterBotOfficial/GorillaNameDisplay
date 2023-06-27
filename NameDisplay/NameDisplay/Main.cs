using BepInEx;
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
            Version = "1.0.6";
        internal static Main Instance;

        internal ManualLogSource manualLogSource;
        internal bool InModded;

        internal Main()
        {
            Instance = this;
            manualLogSource = base.Logger;
            manualLogSource.LogInfo($"Loaded {Name}");

            new HarmonyLib.Harmony(Id).PatchAll();
        }

        #region Utilla callbacks
        [ModdedGamemodeJoin]
        private void OnJoin() =>
            InModded = true;
        [ModdedGamemodeLeave]
        private void OnLeave() =>
            InModded = false;
        #endregion

        #region Enable/Disable
        public void OnEnable()
        {
            manualLogSource.LogInfo("Enabled");
        }
        public void OnDisable()
        {
            manualLogSource.LogInfo("Disabled");
        }
        #endregion
    }
}
