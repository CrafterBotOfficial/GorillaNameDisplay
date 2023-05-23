using HarmonyLib;
using UnityEngine;

namespace NameTag.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    internal class VRRigPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void HookStart(VRRig __instance)
        {
            if (__instance != GorillaTagger.Instance.offlineVRRig && __instance != GorillaTagger.Instance.myVRRig && Main.RoomValid)
                GameObject.Instantiate(Main.Instance.NameTagPrefab).AddComponent<Behaviours.NameTag>().Rig = __instance;
        }
    }
}
