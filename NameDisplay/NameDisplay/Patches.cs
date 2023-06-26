using HarmonyLib;
using Photon.Pun;
using System.Threading.Tasks;
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
            Main.Instance.manualLogSource.LogInfo("Created object pool manager!");
        }
        [HarmonyPatch(typeof(VRRig), "OnEnable"), HarmonyPostfix, HarmonyWrapSafe]
        private static async void VRRig_Enabled(VRRig __instance)
        {
            await Task.Delay(1000);

            PhotonView view = Traverse.Create(__instance).Field("photonView").GetValue<PhotonView>();
            if (view is object && !view.Owner.IsLocal && Main.Instance.InModded)
            {
                if (ObjectPoolManager.Instance == null)
                    return; // well... shit
                Main.Instance.manualLogSource.LogInfo("Creating nametag for client!");

                // Check for rig spammer
                if (Behaviours.NameTag.ActiveNameTags != null)
                {
                    bool AlreadyCreatedNameTag = Behaviours.NameTag.ActiveNameTags.ContainsKey(view.Owner.UserId);
                    if (AlreadyCreatedNameTag)
                        ObjectPoolManager.Instance.ReturnObjectToPool(Behaviours.NameTag.ActiveNameTags[view.Owner.UserId].gameObject);
                }

                // Spawn nametag
                GameObject nameTagObj = ObjectPoolManager.Instance.PullObjectFromPool();
                nameTagObj.AddComponent<Behaviours.NameTag>().Rig = __instance;
            }
        }
    }
}
