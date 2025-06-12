using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private int maxPlayer = 3;

    //UI
    [Space]
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private TMP_InputField playerName;


   
    public void CreateRoom()
    {
        //  PhotonNetwork.JoinOrCreateRoom("Mine", null, null);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        PhotonNetwork.CreateRoom(roomName.text.ToLower(), roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text.ToLower());
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
        Debug.Log("Join Room");

    }
}
