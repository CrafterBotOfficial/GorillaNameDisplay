using BepInEx;
using HarmonyLib;
using Photon.Pun;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace NameTag
{
    [BepInPlugin(GUID, Name, Version)]
    //[BepInDependency("Crafterbot.MonkeStatistics")]
    [BepInDependency("org.legoandmars.gorillatag.utilla")]

    [System.ComponentModel.Description("HauntedModMenu")]
    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.nametag",
            Name = "NameTag",
            Version = "1.0.1";
        internal static bool Enabled;
        private void Awake()
        {
            Logger.LogInfo("Init : " + Name);

            //MonkeStatistics.API.Registry.AddAssembly();
            new Harmony(GUID).PatchAll(Assembly.GetExecutingAssembly());
        }

        #region Enable/Disable
        public void OnEnable()
        {
            Enabled = true;
            foreach (VRRig vrRig in GameObject.FindObjectsOfType<VRRig>())
            {
                try
                {
                    if (vrRig.GetComponent<Core.NameTag>() != null || vrRig.GetComponent<PhotonView>().Owner.IsLocal)
                        return;
                    vrRig.gameObject.AddComponent<Core.NameTag>();
                }
                catch
                {
                    /* Do nothing */
                }
            }
            // if disabled, the NameTag component will handle deleting itself.
        }
        public void OnDisable()
        {
            Enabled = false;
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
