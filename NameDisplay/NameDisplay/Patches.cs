using HarmonyLib;
using UnityEngine;

namespace NameDisplay
{
    [HarmonyPatch]
    internal class Patches
    {
        internal static async void RigCache_RigSpawned_Postfix()
        {
            Transform RigParent = GameObject.Find("Rig Parent").transform;
            VRRig Rig = RigParent.GetChild(RigParent.childCount - 1).GetComponent<VRRig>();
            if (Rig.isMyPlayer || Rig.isOfflineVRRig)
                return;
            
            Main.Instance.manualLogSource.LogInfo("Creating nametag for Rig");
            GameObject.Instantiate(await Main.Instance.LoadAsset("TextObj"), Rig.transform).AddComponent<Behaviours.NameTag>().Rig = Rig;
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
