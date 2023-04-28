using HarmonyLib;
using Photon.Pun;

namespace NameTag.Patches
{
    [HarmonyPatch(typeof(VRRig), "Awake", MethodType.Normal)]
    internal class VRRigPatch
    {
        [HarmonyPostfix]
        private static void Hook(VRRig __instance)
        {
            if (Main.RoomValid && Main.Enabled)
            {
                try
                {
                    if (__instance.GetComponent<PhotonView>().Owner.IsLocal)
                        return;
                    __instance.gameObject.AddComponent<Core.NameTag>();
                }
                catch
                {
                    /* Do nothing */
                }
            }
        }
    }
}
