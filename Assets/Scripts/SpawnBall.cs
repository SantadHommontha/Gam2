using Photon.Pun;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    [SerializeField] private GameObject prefap;
    [SerializeField] private Transform spawnPosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.S))
        {
            Spawn();
        }
    }
    public void Spawn()
    {
        
        var ball = PhotonNetwork.Instantiate(prefap.name, spawnPosition.position, Quaternion.identity);
    }
}
