using Photon.Pun;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public static SpawnBall Instance;
    [SerializeField] private GameObject prefap;
    [SerializeField] private Transform spawnPosition;
    private PhotonView photonView;
    private bool isSpawn = false;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        photonView = GetComponent<PhotonView>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            isSpawn = true;
            Spawn();
        }
    }
    public void Spawn()
    {
        object[] data = new object[] { GenerateCode.GenerateRandomCode(8) };
         
        var ball = PhotonNetwork.Instantiate(prefap.name, spawnPosition.position, Quaternion.identity,0,data);
        // GameManager.Instance.ball = ball.GetComponentInChildren<Ball>();
       // photonView.RPC("RPC_SpawnOther", RpcTarget.Others);
    }
    [PunRPC]
    private void RPC_SpawnOther()
    {
        var ball = PhotonNetwork.Instantiate(prefap.name, spawnPosition.position, Quaternion.identity);
    }

}
