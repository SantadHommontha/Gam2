using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [ContextMenu("Create Room")]
    public void CreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("Mine", null, null);
    }

    public override void OnJoinedLobby()
    {
        CreateRoom();
    }
}
