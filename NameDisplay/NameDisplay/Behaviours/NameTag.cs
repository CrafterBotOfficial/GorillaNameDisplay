using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NameDisplay.Behaviours
{
    internal class NameTag : MonoBehaviour
    {
        internal static Dictionary<VRRig, NameTag> NameTags = new Dictionary<VRRig, NameTag>();

        internal VRRig Rig;
        private Transform _rigTransform;
        private Text _text;
        private GameObject _panel;

        private void Awake()
        {
            Main.Log("NameTag Awake");
            _text = GetComponentInChildren<Text>();
            _text.resizeTextForBestFit = false; // Made a mistake in the prefab
            _panel = transform.GetChild(0).gameObject;
        }

        private void LateUpdate()
        {
            if (!Rig.gameObject.activeSelf)
                ObjectPoolManager.Instance.Push(this);
            if (SetAcivity())
                return;

            transform.position = _rigTransform.position + Vector3.up * .5f;
            transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
        }

        /// <returns>True if the nametag is disabled, and movement should be haulted</returns>
        private bool SetAcivity()
        {
            bool active = Main.Instance.enabled;
            if (active != _panel.activeSelf)
                _panel.SetActive(active);
            return !active;
        }

        /* Internal */

        internal void Initialize(VRRig rig)
        {
            Rig = rig;
            _rigTransform = rig.transform;
            NameTags.Add(rig, this);

            RigInitializeNoobData();
        }

        internal void RigInitializeNoobData()
        {
            _text.color = Rig.materialsToChangeTo[0].color;
            _text.text = Rig.playerText.text;
        }
    }
}
