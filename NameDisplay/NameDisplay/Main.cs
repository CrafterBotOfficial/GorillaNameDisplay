using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
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
            Version = "1.1.0";
        internal static Main Instance;

        internal ManualLogSource manualLogSource => Logger;
        
        internal GameObject NameTagPrefab;
        internal Transform MasterObject;

        internal Dictionary<VRRig, Behaviours.NameTag> NameTags;
        internal bool InModded;

        internal ConfigEntry<bool> HideBadNames;

        internal Main()
        {
            Instance = this;
            manualLogSource.LogInfo($"Loaded {Name}");

            HideBadNames = Config.Bind("General", "HideBadNames", true, "Hide names that are on the auto-ban list");
            NameTags = new Dictionary<VRRig, Behaviours.NameTag>();

            var harmony = new HarmonyLib.Harmony(Id);
            harmony.PatchAll();

            Type VRRigCache = typeof(GorillaTagger).Assembly.GetType("VRRigCache");
            harmony.Patch(VRRigCache.GetMethod("SpawnRig", BindingFlags.NonPublic | BindingFlags.Instance), postfix: new HarmonyLib.HarmonyMethod(typeof(Patches), nameof(Patches.VRRigCache_SpawnRig_Postfix)));
        }

        private AssetBundle _assetBundle;
        internal Task<GameObject> LoadAsset(string Name)
        {
            if (_assetBundle == null)
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameDisplay.Resources.text"))
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

        // Only here to allow CI & HauntedModMenu to work
        public void OnEnable()
        {
            // manualLogSource.LogInfo("Enabled");
        }
        public void OnDisable()
        {
            // manualLogSource.LogInfo("Disabled");
        }
    }
}
