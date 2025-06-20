using Photon.Pun;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public static SpawnBall Instance;
    [SerializeField] private GameObject prefap;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform targetr;
    [SerializeField] private float distanceToMaxForec = 1;
    [SerializeField] private float maxForce = 5f;
    private float force;
    private PhotonView photonView;

    private bool isClick;


    private Vector3 direction;
    private float distance;

    [Header("Value")]
    [SerializeField] private BoolValue gameStart;
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
    void OnMouseDown()
    {
        if (!gameStart.Value) return;
        isClick = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStart.Value) return;
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
        }

        if (isClick)
        {
            direction = CalculateOpposite.CalculateOpposite2D(transform.position, out distance);
            distance = UnityEngine.Mathf.Clamp(distance / distanceToMaxForec, 0, distanceToMaxForec);
            force = UnityEngine.Mathf.Clamp(maxForce * distance, 0, maxForce);
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

        ballHandle.AddForce(direction, force);



    }



}
