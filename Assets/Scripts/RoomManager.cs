using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private CreateAndJoinRoom createAndJoinRoom;
    public static RoomManager Instance;
    // private PhotonView photonView;
    private bool isWaitingToCreateRoom = false;
    private bool isWaitingToJoinLobby = false;
    //  private bool isWantcreateNewRoom = false;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
        //   photonView = GetComponent<PhotonView>();
    }




    public void LeaveRoomBTN()
    {
        TeamManager.Instance.LeaveRoom(PhotonNetwork.LocalPlayer.UserId);
    }

    public void LeaveRoom()
    {

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Loading");
    }

    public void NewRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            TeamManager.Instance.OnNewRoom();
            PhotonNetwork.LeaveRoom();
            isWaitingToJoinLobby = true;
            isWaitingToCreateRoom = true;
        }

    }



    void Update()
    {
        //  Debug.Log(PhotonNetwork.IsConnectedAndReady);
        if (isWaitingToJoinLobby)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                // Debug.Log("Join======================================");
                PhotonNetwork.JoinLobby();
                isWaitingToJoinLobby = false;
            }
        }

    }
    public override void OnLeftRoom()
    {
        // if (isWantcreateNewRoom)
        // {
        //     isWaitingToCreateRoom = true;
        // }
        Debug.Log("Left Room");
        // if (!PhotonNetwork.InLobby)
        //     PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join Loloby Again");
        if (isWaitingToCreateRoom)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                createAndJoinRoom.CreateRoom();

            }
        }
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Create New Room");
        if (isWaitingToCreateRoom)
        {
            isWaitingToCreateRoom = false;
            isWaitingToJoinLobby = false;
            PhotonNetwork.LoadLevel("Game");
        }



    }
}
