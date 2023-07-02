using HarmonyLib;
using UnityEngine;

namespace NameDisplay
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(VRRig), "Start"), HarmonyPostfix]
        internal static async void VRRig_Start(VRRig __instance)
        {
            if (__instance.isMyPlayer || __instance.isOfflineVRRig)
                return;
            
            Main.Instance.manualLogSource.LogInfo("Creating nametag for Rig");
            GameObject.Instantiate(await Main.Instance.LoadAsset("TextObj"), __instance.transform).AddComponent<Behaviours.NameTag>().Rig = __instance;
        }

        [HarmonyPatch(typeof(VRRig), nameof(VRRig.InitializeNoobMaterialLocal)), HarmonyPostfix]
        private static void VRRig_InitializeNoobMaterialLocal_Postfix(VRRig __instance)
        {
            if (!__instance.isMyPlayer && !__instance.isOfflineVRRig && Main.Instance.InModded)
            {
                Main.Instance.manualLogSource.LogInfo("RigNoobDataInitialized Updating nametag data");
                Main.Instance.NameTags[__instance].RigNoobDataInitialized();
            }
        }
    }
}
