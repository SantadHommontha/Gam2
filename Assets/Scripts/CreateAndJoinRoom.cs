using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Collections;
using UnityEngine.SceneManagement;


public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private int maxPlayer = 3;
    [SerializeField] private TMP_Text meassge;
    //UI
    [Space]
    [SerializeField] private TMP_InputField roomName;

    [Header("Value")]
   // [SerializeField] private GameDataValue gameData;
    [SerializeField] private GameInfoValue gameInfo;
  //  [SerializeField] private BoolValue isPlayer;
    //[SerializeField] private StringValue roomname_value;
    // [SerializeField] private BoolValue iamAdmin;
    [SerializeField] private StringValue adminCode;

    void Awake()
    {
        ChangeMeassge();
    }

    public void CreateRoom()
    {
        //  PhotonNetwork.JoinOrCreateRoom("Mine", null, null);
        //  Debug.Log("CCC");
        ChangeMeassge("Create Room");
        gameInfo.Value.roomCode = GenerateCode.GenerateRandomCode().ToLower();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        gameInfo.Value.isAdmin = true;
        PhotonNetwork.CreateRoom(gameInfo.Value.roomCode, roomOptions, TypedLobby.Default);
    }
    private void ChangeMeassge(string _text = "")
    {
        if (meassge)
            meassge.text = _text;
    }
    public void JoinRoom()
    {
        ChangeMeassge("Join Room");
        gameInfo.Value.isPlayer = true;
        PhotonNetwork.JoinRoom(roomName.text.ToLower());
    }

    public void JoinBTN()
    {
        if (roomName.text.ToLower() == adminCode.Value.ToLower())
        {
            CreateRoom();
        }
        else if (roomName.text.ToLower() == "onionplayer")
        {
            gameInfo.Value.isPlayer = true;
         //   isPlayer.Value = true;
            CreateRoom();
        }
        else
        {
            JoinRoom();
        }



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
    public void BackBtn()
    {


        SceneManager.LoadScene("Loading");



    }
}
