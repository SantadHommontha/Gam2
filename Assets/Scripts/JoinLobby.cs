using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;


public class JoinLobby : MonoBehaviourPunCallbacks, IScreenDown
{
    public float loadPercen = 0;

    void Start()
    {
        // PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
        loadPercen = 0;
        if (PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Lobby 1");
        }
        else
        {
            Debug.Log("Connecting To Server");
            PhotonNetwork.ConnectUsingSettings();
             loadPercen = 0.3f;
        }
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Join Server");
        PhotonNetwork.JoinLobby();
        loadPercen = 0.6f;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join lobby");
        //  
        loadPercen = 0.8f;
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1.5f);
        loadPercen = 1f;
    }

    public void TapScreen()
    {
        SceneManager.LoadScene("Lobby 1");
    }



}
