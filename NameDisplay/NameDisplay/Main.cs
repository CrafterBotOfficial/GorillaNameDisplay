using BepInEx;
using BepInEx.Logging;
using Utilla;

namespace NameDisplay
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [System.ComponentModel.Description("HauntedModMenu"), ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.nametag",
            NAME = "NameTag",
            VERSION = "1.0.5";
        internal static Main Instance;

        internal ManualLogSource manualLogSource => Logger;
        internal bool InModded;

        internal Main()
        {
            Instance = this;
            manualLogSource.LogInfo($"Loaded {NAME}");

            Behaviours.NameTag.ActiveNameTags = new System.Collections.Generic.Dictionary<string, Behaviours.NameTag>();
            new HarmonyLib.Harmony(GUID).PatchAll();
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
