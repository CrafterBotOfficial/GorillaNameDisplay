using HarmonyLib;
using UnityEngine;

namespace NameDisplay
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(VRRig), "Start"), HarmonyPostfix]
        private static void VRRig_Start_Postfix(VRRig __instance)
        {
            if (Main.Instance.NameTagPrefab is object && !__instance.isOfflineVRRig && !__instance.isMyPlayer)
            {
                GameObject.Instantiate(Main.Instance.NameTagPrefab).AddComponent<Behaviours.NameTag>().Rig = __instance;
            }
        }

        [HarmonyPatch(typeof(VRRig), nameof(VRRig.InitializeNoobMaterialLocal)), HarmonyPostfix]
        private static void VRRig_InitializeNoobMaterialLocal_Postfix(VRRig __instance)
        {
            Main.Instance.NameTags[__instance].RigNoobDataInitialized();
        }
    }
}
