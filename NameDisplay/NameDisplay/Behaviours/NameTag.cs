using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NameDisplay.Behaviours
{
    internal class NameTag : MonoBehaviour
    {
        internal static Dictionary<string, NameTag> ActiveNameTags;

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

            // Add self to list of nametags
            ActiveNameTags.Add(player.UserId, this);
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
                if (Text.text != player.NickName)
                    Text.text = player.NickName;
                if (Text.color != Rig.materialsToChangeTo[0].color)
                    Text.color = Rig.materialsToChangeTo[0].color;
            }
            // Update position
            transform.position = Rig.transform.position + Vector3.up * 0.5f;
            // Update Rotation
            Vector3 LocalPlayerPosition = GorillaLocomotion.Player.Instance.transform.position;
            transform.LookAt(new Vector3(LocalPlayerPosition.x, transform.position.y, LocalPlayerPosition.z));
        }

        /* Disabling methods */

        private void OnDestroy()
        {
            if (ActiveNameTags != null)
                ActiveNameTags.Remove(player.UserId);
        }
    }
}
