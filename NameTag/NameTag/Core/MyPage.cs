using MonkeStatistics.API;
using Photon.Pun;
using UnityEngine;

namespace NameTag.Core
{
    [DisplayInMainMenu("NameTag")]
    internal class MyPage : Page
    {
        public override void OnPageOpen()
        {
            base.OnPageOpen();
            SetTitle("NameTag");
            SetAuthor("By Crafterbot");

            if (Main.RoomValid)
                DrawLines();
            else
            {
                AddLine(1);
                AddLine("YOU MUST BE IN");
                AddLine("A MODDED LOBBY.");
            }

            SetLines();
        }

        private void DrawLines()
        {
            TextLines = new Line[0];
            AddLine("Enable", new ButtonInfo(OnEnablePress, 0, ButtonInfo.ButtonType.Toggle, Main.Enabled));
            AddLine(1);
        }

        private void OnEnablePress(object Sender, object[] Args)
        {
            bool ButtonPressed = (bool)Args[1];
            Main.Enabled = ButtonPressed;

            if (Main.Enabled)
            {
                foreach (VRRig vrRig in GameObject.FindObjectsOfType<VRRig>())
                {
                    try
                    {
                        if (vrRig.GetComponent<NameTag>() != null || vrRig.GetComponent<PhotonView>().Owner.IsLocal)
                            return;
                        vrRig.gameObject.AddComponent<NameTag>();
                    }
                    catch
                    {
                        /* Do nothing */
                    }
                }
            }
            // if disabled, the NameTag component will handle deleting itself.
        }
    }
}
