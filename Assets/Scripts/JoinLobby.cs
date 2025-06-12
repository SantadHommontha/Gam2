using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class JoinLobby : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connect To Server");
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Join Server");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join lobby");
        SceneManager.LoadScene("Lobby");

    }



}
