using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using NameTag.Util;
using UnityEngine;
using Utilla;

namespace NameTag
{
    [BepInPlugin(GUID, NAME, VERSION), BepInDependency("org.legoandmars.gorillatag.utilla")]
    [System.ComponentModel.Description("HauntedModMenu")]
    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.nametag",
            NAME = "NameDisplay",
            VERSION = "1.0.3";
        internal static Main Instance;
        internal ManualLogSource manualLogSource;
        internal GameObject NameTagPrefab;

        #region Config
        internal static ConfigEntry<float> YOffset;
        internal static ConfigEntry<float> Buffer;
        #endregion

        private void Start()
        {
            Instance = this;
            manualLogSource = Logger;
            $"Init : {GUID} + {VERSION}".Log();

            #region Config
            YOffset = Config.Bind("General", nameof(YOffset), 0.5f, "How high the nametag will be from the player.");
            Buffer = Config.Bind("General", nameof(Buffer), 0.5f, "How frequently the nametag will check to see if the data on it is up-to-date. (Max:10|Min:1)(Seconds)");
            #endregion

            NameTagPrefab = AssetLoader.GetAsset("TextObj") as GameObject;
            new Harmony(GUID).PatchAll();
        }

        #region Enable/Disable
        public void OnEnable()
        {
        }
        public void OnDisable()
        {
        }
        #endregion
        #region Modded gamemode
        internal static bool RoomValid;
        [ModdedGamemodeJoin]
        private void RoomJoin() =>
            RoomValid = true;
        [ModdedGamemodeLeave]
        private void RoomLeft() =>
            RoomValid = false;
        #endregion
    }
}
