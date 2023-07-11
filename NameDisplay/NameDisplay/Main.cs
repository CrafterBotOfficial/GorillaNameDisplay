using BepInEx;
using BepInEx.Configuration;
using Utilla;

namespace NameDisplay
{
    [BepInPlugin("crafterbot.nametag", "NameTag", "1.1.1"), BepInDependency("org.legoandmars.gorillatag.utilla")]
    [ModdedGamemode, System.ComponentModel.Description("HauntedModMenu")]
    internal class Main : BaseUnityPlugin
    {
        internal static Main Instance;
        
        internal bool InModded;
        internal ConfigEntry<bool> ShowNametagForLocal;

        internal Main()
        {
            Instance = this;
            Log("Initializing NameTag");

            Utilla.Events.GameInitialized += (obj, obj1) =>
            {
                Log("Game Initialized");
                new ObjectPoolManager();
                new UnityEngine.GameObject("Callbacks").AddComponent<Behaviours.Callbacks>();
            };

            ShowNametagForLocal = Config.Bind("General", "ShowNametagForLocal", true, "Show nametag for local player");
            new HarmonyLib.Harmony(Info.Metadata.GUID).PatchAll(typeof(Patches));
        }

        #region Utlla & Enable/Disable callbacks

        [ModdedGamemodeJoin]
        private void OnJoin()
        {
            InModded = true;
        }

        [ModdedGamemodeLeave]
        private void OnLeave()
        {
            InModded = false;
            if (ShowNametagForLocal.Value && enabled)
                ObjectPoolManager.Instance.Push(Behaviours.NameTag.NameTags[GorillaTagger.Instance.offlineVRRig]);
        }

        /* Enable/disable placeholders */

        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
        #endregion

        internal static void Log(object message, BepInEx.Logging.LogLevel logLevel = BepInEx.Logging.LogLevel.Info)
        {
            Instance.Logger.Log(logLevel, message);
        }
    }
}
