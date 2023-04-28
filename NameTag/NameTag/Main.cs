using BepInEx;
using HarmonyLib;
using System.Reflection;
using Utilla;

namespace NameTag
{
    [BepInPlugin(GUID, Name, Version)]
    [BepInDependency("Crafterbot.MonkeStatistics")]
    [BepInDependency("org.legoandmars.gorillatag.utilla")]

    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        public const string
            GUID = "crafterbot.nametag",
            Name = "NameTag",
            Version = "1.0.0";
        internal static bool Enabled;
        private void Awake()
        {
            Logger.LogInfo("Init : " + Name);

            MonkeStatistics.API.Registry.AddAssembly();
            new Harmony(GUID).PatchAll(Assembly.GetExecutingAssembly());
        }

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
