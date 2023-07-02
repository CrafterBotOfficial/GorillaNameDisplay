using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace NameDisplay.Behaviours
{
    internal class NameTag : MonoBehaviour
    {
        private GameObject PanelObj;

        internal VRRig Rig;
        private Traverse traverse;

        private void Start()
        {
            Main.Instance.NameTags.Add(Rig, this);

            traverse = Traverse.Create(Rig);
            traverse.Field("photonView");

            PanelObj = transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            if (SetState())
                return;

            // Update position
            transform.position = Rig.transform.position + Vector3.up * 0.5f;

            // Update Rotation
            Vector3 LocalPlayerPosition = Camera.main.transform.position;
            transform.LookAt(new Vector3(LocalPlayerPosition.x, transform.position.y, LocalPlayerPosition.z));
        }

        internal void RigNoobDataInitialized()
        {
            Text text = GetComponentInChildren<Text>();
            text.text = NormalizeName(traverse.GetValue<PhotonView>().Owner.NickName);
            text.color = Rig.materialsToChangeTo[0].color;
        }

        /// <returns>True if the script needs to be haulted due to the rig being inactive. (or not in a modded room)</returns>
        private bool SetState()
        {
            bool Active = Rig.gameObject.activeSelf && Main.Instance.InModded;
            if (PanelObj.activeSelf != Active)
            {
                Main.Instance.manualLogSource.LogInfo($"Setting nametag state to {Active}");
                PanelObj.SetActive(Active);
            }
            return !Active;
        }

        private string NormalizeName(string Name)
        {
            if (!GorillaNetworking.GorillaComputer.instance.CheckAutoBanListForName(Name))
            {
                return Main.Instance.HideBadNames.Value ? "[HIDDED NAME]" : Name; // I am leaving this as a config option so people can report these people. Now ofc its going to trigger the anti cheat so it doesn't really matter :P
            }
            return Name.ToUpper().Replace(" ", "");
        }
    }
}
