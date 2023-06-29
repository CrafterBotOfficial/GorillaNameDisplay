using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
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
            Version = "1.0.8";
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

        private AssetBundle _assetBundle;
        internal Task<GameObject> LoadAsset(string Name)
        {
            if (_assetBundle == null)
                using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("NameDisplay.Resources.text"))
                {
                    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromStreamAsync(stream);
                    new WaitUntil(() => assetBundleCreateRequest.isDone);
                    _assetBundle = assetBundleCreateRequest.assetBundle;
                }
            AssetBundleRequest assetBundleRequest = _assetBundle.LoadAssetAsync<GameObject>(Name);
            new WaitUntil(() => assetBundleRequest.isDone);
            return Task.FromResult(assetBundleRequest.asset as GameObject);
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
