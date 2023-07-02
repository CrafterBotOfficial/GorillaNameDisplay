using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace NameDisplay.Behaviours
{
    internal class NameTag : MonoBehaviour
    {
        internal VRRig Rig;
        private Traverse traverse;

        private void Start()
        {
            Main.Instance.NameTags.Add(Rig, this);

            traverse = Traverse.Create(Rig);
            traverse.Field("photonView");
        }

        private void LateUpdate()
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

        private bool SetState()
        {
            bool IsInactive = !Rig.gameObject.activeSelf  || !Main.Instance.InModded;
            if (gameObject.activeSelf != IsInactive)
                gameObject.SetActive(IsInactive);
            return IsInactive;
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
