using HarmonyLib;
using UnityEngine;

namespace NameDisplay
{
    [HarmonyPatch]
    internal class Patches
    {
        internal static async void VRRigCache_SpawnRig_Postfix(object __result)
        {
            if (Main.Instance.MasterObject == null)
                Main.Instance.MasterObject = new GameObject("NameTags").transform;

            VRRig Rig = ((MonoBehaviour)__result).GetComponent<VRRig>();
            GameObject NameTag = GameObject.Instantiate(await Main.Instance.LoadAsset("TextObj"), Main.Instance.MasterObject);
            NameTag.AddComponent<Behaviours.NameTag>().Rig = Rig;
        }

        [HarmonyPatch(typeof(VRRig), nameof(VRRig.InitializeNoobMaterialLocal)), HarmonyPostfix, HarmonyWrapSafe]
        private static void VRRig_InitializeNoobMaterialLocal_Postfix(VRRig __instance)
        {
            if (!__instance.isMyPlayer && !__instance.isOfflineVRRig)
                Main.Instance.NameTags[__instance].RigNoobDataInitialized();
        }
    }
}