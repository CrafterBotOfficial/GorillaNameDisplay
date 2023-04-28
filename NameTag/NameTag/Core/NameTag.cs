using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NameTag.Core
{
    internal class NameTag : MonoBehaviour
    {
        private Player player;
        private VRRig vrRig;

        private Transform textObjTransform;
        private Text text;

        private string _Username;
        private Color _Color;

        public void Start()
        {
            player = GetComponent<PhotonView>().Owner;
            vrRig = GetComponent<VRRig>();

            GameObject TextObj = GameObject.Instantiate(AssetLoader.GetAsset("TextObj") as GameObject);
            textObjTransform = TextObj.transform;
            text = textObjTransform.GetChild(0).GetComponentInChildren<Text>();

            StartCoroutine(InfrequentUpdate());
        }

        private void Update()
        {
            if (!Main.Enabled)
            {
                Destroy(textObjTransform.gameObject);
                Destroy(this);
            }

            Vector3 TargetPosition = new Vector3(0, 0.5f, 0f) + vrRig.transform.position;
            textObjTransform.position = TargetPosition;

            // look at the player
            Vector3 TargetRotation = new Vector3(GorillaLocomotion.Player.Instance.headCollider.transform.position.x, transform.position.y, GorillaLocomotion.Player.Instance.headCollider.transform.position.z);
            textObjTransform.LookAt(TargetRotation);
        }

        private void OnDestroy()
        {
            Destroy(textObjTransform.gameObject);
        }

        private IEnumerator InfrequentUpdate()
        {
            _Color = Color.blue;
            for (; ; )
            {
                // Username changed
                if (_Username != player.NickName)
                {
                    _Username = player.NickName;
                    text.text = _Username;
                }

                // Color changed
                if (_Color != vrRig.materialsToChangeTo[0].color)
                {
                    _Color = vrRig.materialsToChangeTo[0].color;
                    text.color = _Color;
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
