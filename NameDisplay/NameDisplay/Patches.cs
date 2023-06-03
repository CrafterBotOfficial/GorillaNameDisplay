using HarmonyLib;
using Photon.Pun;
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
        [HarmonyPatch(typeof(VRRig), "Start"), HarmonyPostfix, HarmonyWrapSafe]
        private static void Hook_VRRigAwake(VRRig __instance)
        {
            Main.Instance.manualLogSource.LogInfo(PhotonNetwork.CurrentRoom.CustomProperties.ToString());

            Main.Instance.manualLogSource.LogInfo("VRRig created, checking it now;)");
            if (__instance.TryGetComponent(out PhotonView component) && !component.Owner.IsLocal && Main.Instance.InModded)
            {
                if (ObjectPoolManager.Instance == null)
                    return; // well... shit
                Main.Instance.manualLogSource.LogInfo("Creating nametag for client!");

                // Check for rig spammer
                if (Behaviours.NameTag.ActiveNameTags != null)
                {
                    bool AlreadyCreatedNameTag = Behaviours.NameTag.ActiveNameTags.ContainsKey(component.Owner.UserId);
                    if (AlreadyCreatedNameTag)
                        ObjectPoolManager.Instance.ReturnObjectToPool(Behaviours.NameTag.ActiveNameTags[component.Owner.UserId].gameObject);
                }

                // Spawn nametag
                GameObject nameTagObj = ObjectPoolManager.Instance.PullObjectFromPool();
                nameTagObj.AddComponent<Behaviours.NameTag>().Rig = __instance;
            }
        }
    }
}
