using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Collections;


public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private int maxPlayer = 3;
    [SerializeField] private TMP_Text meassge;
    //UI
    [Space]
    [SerializeField] private TMP_InputField roomName;

    [Header("Value")]
    [SerializeField] private StringValue roomname_value;
    [SerializeField] private BoolValue iamAdmin;
    [SerializeField] private StringValue adminCode_value;

    void Awake()
    {
        ChangeMeassge();
    }

    public void CreateRoom()
    {
        //  PhotonNetwork.JoinOrCreateRoom("Mine", null, null);
        Debug.Log("CCC");
        ChangeMeassge("Create Room");
        roomname_value.Value = GenerateCode.GenerateRandomCode().ToLower();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        iamAdmin.Value = true;
        PhotonNetwork.CreateRoom(roomname_value.Value,roomOptions,TypedLobby.Default);
    }
    private void ChangeMeassge(string _text = "")
    {
        if (meassge)
            meassge.text = _text;
    }
    public void JoinRoom()
    {

        // PhotonNetwork.IsMessageQueueRunning = false;
        ChangeMeassge("Join Room");
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
        ChangeMeassge("Not Found Room");
        StartCoroutine(IE_Cooldown());

    }

    private IEnumerator IE_Cooldown()
    {
        yield return new WaitForSeconds(3f);
        ChangeMeassge();
    }
}
