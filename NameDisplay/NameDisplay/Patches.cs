using HarmonyLib;
using UnityEngine;

namespace NameDisplay
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(GorillaLocomotion.Player), "Awake"), HarmonyPostfix]
        private static void Hook_GorillaLocomotionAwake()
        {
            Main.Instance.manualLogSource.LogInfo("Creating object pool manager...");
            new ObjectPoolManager();
            // Main.Instance.manualLogSource.LogInfo("Created object pool manager!");
        }

        [HarmonyPatch(typeof(VRRig), "Start"), HarmonyPostfix, HarmonyWrapSafe]
        private static void VRRig_Started(VRRig __instance)
        {
            if (__instance.isOfflineVRRig || __instance.photonView.IsMine || !Main.Instance.InModded)
                return;

            // PhotonView view = Traverse.Create(__instance).Field("photonView").GetValue<PhotonView>();
            /*if (view is object && *//*!view.IsMine && *//*Main.Instance.InModded)
            {*/
            if (ObjectPoolManager.Instance == null)
                return; // well... shit

            Main.Instance.manualLogSource.LogInfo("Creating nametag for client!");

            // Spawn nametag
            GameObject nameTagObj = ObjectPoolManager.Instance.PullObjectFromPool();
            nameTagObj.AddComponent<Behaviours.NameTag>().Rig = __instance;
            // }
        }
        /*[HarmonyPatch(typeof(VRRig), "OnEnable"), HarmonyPostfix, HarmonyWrapSafe]
        private static async void VRRig_Enabled(VRRig __instance)
        {
            await Task.Delay(2000);

            PhotonView view = Traverse.Create(__instance).Field("photonView").GetValue<PhotonView>();
            if (view is object && !view.IsMine && Main.Instance.InModded)
            {
                if (ObjectPoolManager.Instance == null)
                    return; // well... shit

                Main.Instance.manualLogSource.LogInfo("Creating nametag for client!");

                // Spawn nametag
                GameObject nameTagObj = ObjectPoolManager.Instance.PullObjectFromPool();
                nameTagObj.AddComponent<Behaviours.NameTag>().Rig = __instance;
            }
        }*/
    }
}
