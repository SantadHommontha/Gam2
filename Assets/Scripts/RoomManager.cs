using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    private PhotonView photonView;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
        photonView = GetComponent<PhotonView>();
    }



    


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Loading");
    }
}
