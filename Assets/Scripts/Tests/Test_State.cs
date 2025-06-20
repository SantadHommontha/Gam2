using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Test_State : MonoBehaviour
{
    [SerializeField] private Image server;
    [SerializeField] private Image master;
    [SerializeField] private Image loby;
    [SerializeField] private Image room;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnected)
            server.color = Color.green;
        else
            server.color = Color.red;

        if (PhotonNetwork.IsMasterClient)
            master.color = Color.green;
        else
            master.color = Color.red;

        if (PhotonNetwork.InLobby)
            loby.color = Color.green;
        else
            loby.color = Color.red;

        if (PhotonNetwork.InRoom)
            room.color = Color.green;
        else
            room.color = Color.red;

    }
}
