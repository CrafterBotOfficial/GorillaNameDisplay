using BepInEx;
using BepInEx.Logging;

namespace NameDisplay
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [System.ComponentModel.Description("HauntedModMenu")]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.nametag",
            NAME = "NameTag",
            VERSION = "1.0.2";
        internal static Main Instance;

        internal ManualLogSource manualLogSource => Logger;
        internal bool InModded => GorillaNetworking.GorillaComputer.instance.currentGameMode.Contains("modded".ToUpper());

        internal Main()
        {
            Instance = this;
            manualLogSource.LogInfo($"Loaded {NAME}");

            Behaviours.NameTag.ActiveNameTags = new System.Collections.Generic.Dictionary<string, Behaviours.NameTag>();
            new HarmonyLib.Harmony(GUID).PatchAll();
        }

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
