using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

namespace NameDisplay.Behaviours
{
    internal class Callbacks : MonoBehaviourPunCallbacks
    {
        public override async void OnJoinedRoom()
        {
            if (!Main.Instance.InModded)
                return;
            await Task.Yield(); // Rigs should be set by now

            Player[] players = Main.Instance.ShowNametagForLocal.Value ? PhotonNetwork.PlayerList : PhotonNetwork.PlayerListOthers;
            foreach (Player player in players)
            {
                InitNametagForPlayer(player);
            }
        }

        public override async void OnPlayerEnteredRoom(Player newPlayer)
        {
            await Task.Yield(); // Rig should be set by now
            if (Main.Instance.InModded)
                InitNametagForPlayer(newPlayer);
        }

        private void InitNametagForPlayer(Player player)
        {
            VRRig Rig = GorillaGameManager.StaticFindRigForPlayer(player);
            if (Rig != null)
            {
                NameTag tag = ObjectPoolManager.Instance.Pull();
                tag.Initialize(Rig);
                return;
            }
            Main.Log("Failed to find VRRig for player " + player.NickName, BepInEx.Logging.LogLevel.Error);
        }
    }
}
