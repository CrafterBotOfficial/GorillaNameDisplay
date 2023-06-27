using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace NameDisplay.Behaviours
{
    internal class NameTag : MonoBehaviour
    {
        private string _NickName;

        internal VRRig Rig;
        private Player player;

        private Text Text;
        private GameObject PanelObj;

        private void Start()
        {
            player = Traverse.Create(Rig).Field("photonView").GetValue<PhotonView>().Owner;
            Text = GetComponentInChildren<Text>();
            Text.resizeTextForBestFit = false; // fix for assetbundle issue
            PanelObj = transform.GetChild(0).gameObject;
        }

        private void LateUpdate()
        {
            if (!Rig.gameObject.activeSelf || player == null || !player.InRoom())
                ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);

            // Enable/Disable
            bool ShouldBeEnabled = Main.Instance.enabled;
            if (PanelObj.activeSelf != ShouldBeEnabled)
                PanelObj.SetActive(ShouldBeEnabled);

            // Update stats
            if (Time.frameCount % 50 == 0)
            {
                if (_NickName != player.NickName)
                {
                    Text.text = NormalizeName(player.NickName);
                    _NickName = player.NickName;
                }
                if (Text.color != Rig.materialsToChangeTo[0].color)
                    Text.color = Rig.materialsToChangeTo[0].color;
            }

            // Update position
            transform.position = Rig.transform.position + Vector3.up * 0.5f;
            // Update Rotation
            Vector3 LocalPlayerPosition = GorillaLocomotion.Player.Instance.transform.position;
            transform.LookAt(new Vector3(LocalPlayerPosition.x, transform.position.y, LocalPlayerPosition.z));
        }

        private string NormalizeName(string Name)
        {
            if (!GorillaNetworking.GorillaComputer.instance.CheckAutoBanListForName(Name))
            {
                return Main.Instance.HideBadNames.Value ? "[HIDDED NAME]" : Name; // I am leaving this as a config option so people can report these people. Now ofc its going to trigger the anti cheat so it doesn't really matter :P
            }
            return Name.ToUpper();
        }
    }
}
