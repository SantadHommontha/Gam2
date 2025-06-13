using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ShowConnect : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Color yes = Color.green;
    [SerializeField] private Color no = Color.red;

    [Header("Image")]
    [SerializeField] private Image master;
    [SerializeField] private Image server;
    private void Server()
    {
        if (server)
        {
            if (PhotonNetwork.IsConnected)
                server.color = yes;
            else
                server.color = no;
        }
    }
    private void Master()
    {
        if (master)
        {
            if (PhotonNetwork.IsMasterClient)
                master.color = yes;
            else
                master.color = no;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Master();
        Server();
    }
}
