using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NameTag.Behaviours
{
    internal class NameTag : MonoBehaviour
    {
        internal VRRig Rig;
        private Player player;
        private Text text;
        private GameObject Panel;
        private void Start()
        {
            player = Rig.GetComponent<PhotonView>().Owner;
            if (player.IsLocal)
                Destroy(gameObject);
            text = GetComponentInChildren<Text>();
            Panel = GetComponentInChildren<Image>().gameObject;
            StartCoroutine(InfrequentUpdate());
        }

        private void Update()
        {
            if (Rig == null || player == null)
                return;
            Panel.SetActive(Main.Instance.enabled);

            // Face towards to local player
            Vector3 PlayerPosition = GorillaLocomotion.Player.Instance.transform.position;
            transform.LookAt(new Vector3(PlayerPosition.x, transform.position.y, PlayerPosition.z));

            // Hover over the targetted VRRig
            transform.position = Rig.transform.position + Vector3.up * Main.YOffset.Value;
        }
        private void OnDestroy()
        {
            StopCoroutine(InfrequentUpdate());
        }

        private IEnumerator InfrequentUpdate()
        {
            for (; ; )
            {
                if (Panel.activeSelf)
                {
                    // Check for deletion
                    if (!Main.RoomValid || !PhotonNetwork.PlayerList.Contains(player) || Rig == null)
                        Destroy(gameObject);

                    if (text.color != Rig.materialsToChangeTo[0].color)
                        text.color = Rig.materialsToChangeTo[0].color;
                    if (text.text != player.NickName)
                        text.text = player.NickName;
                }
                yield return new WaitForSeconds(Mathf.Clamp(Main.Buffer.Value, 1, 10));
            }
        }
    }
}