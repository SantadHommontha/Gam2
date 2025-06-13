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

    [Header("Value")]
    [SerializeField] private StringValue roomname_value;
    [SerializeField] private BoolValue iamAdmin;
    [SerializeField] private StringValue adminCode_value;

    public void CreateRoom()
    {
        //  PhotonNetwork.JoinOrCreateRoom("Mine", null, null);

        roomname_value.Value = GenerateCode.GenerateRandomCode().ToLower();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        iamAdmin.Value = true;
        PhotonNetwork.CreateRoom(roomname_value.Value);
    }

    public void JoinRoom()
    {
        Debug.Log("FFFFFFFFFFFF");
       // PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.JoinRoom(roomName.text.ToLower());

    }

    public void JoinBTN()
    {
        if (roomName.text.ToLower() == adminCode_value.Value.ToLower())
            CreateRoom();
        else
            JoinRoom();

    }
    public override void OnCreatedRoom()
    {

        Debug.Log("Create Room");
        PhotonNetwork.LoadLevel("Game");

    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room");
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {

        Debug.LogError($"Join Room Fail COde {returnCode} , Message {message}");
    }
}
