using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public static SpawnBall Instance;
    [SerializeField] private GameObject prefap;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform targetr;
    [SerializeField] private float distanceToMaxForec = 0.5f;
    [SerializeField] private float maxForce = 10f;
    private float force;
    private PhotonView photonView;

    private bool isClick;

    public List<BallHandle> ballList = new List<BallHandle>();

    private Vector3 direction;
    private float distance;

    [Header("Value")]
    [SerializeField] private GameDataValue gameData;
    [SerializeField] private GameInfoValue gameInfo;
    void Awake()
    {

        Instance = this;

        photonView = GetComponent<PhotonView>();
    }
    void OnEnable()
    {
        Instance = this;
    }
    void Start()
    {
        targetr.gameObject.SetActive(false);
    }
    void OnMouseDown()
    {
        if (!gameInfo.Value.isPlayer) return;
        if (!gameData.Value.gamestart) return;
        isClick = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameInfo.Value.isPlayer) return;
        if (!gameData.Value.gamestart) return;
        if (Input.GetKeyDown(KeyCode.S))
        {

            Spawn();



        }
        //  Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonUp(0) && isClick)
        {
            //shot = true;
            isClick = false;
            Spawn();
            targetr.gameObject.SetActive(false);
        }

        if (isClick)
        {
            direction = CalculateOpposite.CalculateOpposite2D(transform.position, out distance);
            distance = UnityEngine.Mathf.Clamp(distance / distanceToMaxForec, 0, 1);
            force = UnityEngine.Mathf.Clamp(maxForce * distance, 0, maxForce);
            Debug.Log($"Dis:{distance} - {force} ");
            targetr.gameObject.SetActive(true);
        }


        targetr.position = transform.position + direction;
    }
    public void Spawn()
    {
        object[] data = new object[] { GenerateCode.GenerateRandomCode(8) };
        GameObject ball;
        if (PhotonNetwork.InRoom)
        {
            ball = PhotonNetwork.Instantiate(prefap.name, spawnPosition.position, Quaternion.identity, 0, data);
        }
        else
        {
            ball = Instantiate(prefap, spawnPosition.position, Quaternion.identity);
        }
        var ballHandle = ball.GetComponent<BallHandle>();
        ballList.Add(ballHandle);
        ballHandle.AddForce(direction, force);
        ballHandle.GotShoot();




    }

    public void RemoveAllBall()
    {
        var ballcount = ballList.Count;

        for (int i = 0; i < ballcount; i++)
        {
            if (PhotonNetwork.InRoom)
            {
                ballList[0].TranferOwner();
                PhotonNetwork.Destroy(ballList[0].gameObject);

            }
            else
            {
                Destroy(ballList[0].gameObject);
            }

            ballList.RemoveAt(0);
        }
    }

}
