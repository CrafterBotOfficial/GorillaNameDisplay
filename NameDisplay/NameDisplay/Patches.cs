using HarmonyLib;

namespace NameDisplay
{
    internal class Patches
    {
        [HarmonyPatch(typeof(VRRig), nameof(VRRig.InitializeNoobMaterialLocal)), HarmonyPostfix, HarmonyWrapSafe]
        private static void VRRig_InitializeNoobMaterialLocal_Postfix(VRRig __instance)
        {
            if (Main.Instance.InModded)
                Behaviours.NameTag.NameTags[__instance].RigInitializeNoobData();
        }
    }
}
