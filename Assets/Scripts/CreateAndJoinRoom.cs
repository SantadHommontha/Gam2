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

    [SerializeField] private GameInfoValue gameInfo;

    [SerializeField] private StringValue adminCode;

    void Awake()
    {
        SetMeassge("");
    }
    #region  CreateRoom
    public void CreateRoom()
    {
        GameInfo.SetToDefualtValue(gameInfo.Value);
        ChangeMeassge("Create Room");
        gameInfo.Value.roomCode = GenerateCode.GenerateRandomCode().ToLower();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        gameInfo.Value.isAdmin = true;
        PhotonNetwork.CreateRoom(gameInfo.Value.roomCode, roomOptions, TypedLobby.Default);
    }
    #endregion
    private void ChangeMeassge(string _text)
    {
        StartCoroutine(IE_ClearMassage(_text));
    }
    private void SetMeassge(string _text)
    {
        if (meassge)
            meassge.text = _text;
    }
    #region JoinRoom
    public void JoinRoom()
    {
        ChangeMeassge("Join Room");
        gameInfo.Value.isPlayer = true;
        PhotonNetwork.JoinRoom(roomName.text.ToLower());
    }
    #endregion

    #region  UI Button
    public void JoinBTN()
    {
        if (roomName.text.ToLower() == adminCode.Value.ToLower())
        {
            CreateRoom();
        }
        else if (roomName.text.ToLower() == "onionplayer")
        {
            gameInfo.Value.isPlayer = true;
            CreateRoom();
        }
        else
        {
            JoinRoom();
        }

    }

    public void BackBtn()
    {
        SceneManager.LoadScene("Loading 1");
    }
    #endregion

    #region CallBack Function
    public override void OnCreatedRoom()
    {

        Debug.Log("Create Room");
        PhotonNetwork.LoadLevel("Game 1");

    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room");
        PhotonNetwork.LoadLevel("Game 1");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ChangeMeassge("Not Found Room");
        // StartCoroutine(IE_Cooldown());

    }
    #endregion
    private IEnumerator IE_ClearMassage(string _text)
    {
        SetMeassge(_text);
        yield return new WaitForSeconds(3f);
        SetMeassge("");
    }

}
