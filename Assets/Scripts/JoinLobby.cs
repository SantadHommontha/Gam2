using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class JoinLobby : MonoBehaviourPunCallbacks,IScreenDown
{
   
    void Start()
    {
        // PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";

        if (PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.Log("Connecting To Server");
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Join Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join lobby");
        //  
    }
   
   

    public void TapScreen()
    {
       SceneManager.LoadScene("Lobby 1");
    }
}
