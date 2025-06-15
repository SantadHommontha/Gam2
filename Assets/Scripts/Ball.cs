using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class BallDataWapper
{
    public int playerSendIndex;
    public int nextPLayerIndex;
    public bool up;
    public float xPosition;
    public float yPosition;
    public float xVelocity;
    public float yVelocity;
    public string ballID;
}


public class Ball : MonoBehaviour, IPunInstantiateMagicCallback
{
    [Header("Public")]
    [SerializeField] private float maxForce = 10;
    [SerializeField] private float maxHeightScreen = 8.2f;
    [SerializeField] private float distanceToMaxForec = 2f;
    public Rigidbody2D rb;
    public Transform TARGET;
    [SerializeField] private bool isClick = false;
    [SerializeField] private bool shot = false;
    private float force = 0;
    // Mouse
    private Vector2 startMousePosition;
    private Vector2 currentMousePosition;

    public bool canTrigger = true;

    // Touch
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 dragDirection;
    [SerializeField] private Vector2 oppositeDirection;

    private bool isDragging = false;
    private int touchId = -1;
    //  private Ball ball;

    public Vector2 DragDirection => dragDirection;
    public Vector2 OppositeDirection => oppositeDirection;
    public string ballID;
    public BallHandle ballHandle;
    private PhotonView photonView;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = ballHandle.photonView;
    }
    void Start()
    {
        //  GameManager.Instance.ball = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (isClick)
        {
            CalculateOpposite();
            rb.simulated = false;
        }

        if (Input.GetMouseButtonUp(0) && isClick)
        {
            shot = true;
            isClick = false;
        }

        // if (BallOnScreen())
        // {
        //     Debug.Log("FFFF");
        //     // TakeBvall();
        // }

    }
    private void AddForce(Vector2 _direction, ForceMode2D _forceMode2D = ForceMode2D.Impulse)
    {
        AddForce(_direction, force, _forceMode2D);
    }
    private void AddForce(Vector2 _direction, float _force, ForceMode2D _forceMode2D = ForceMode2D.Impulse)
    {
        Debug.Log("Force");
        rb.simulated = true;
        Vector2 forceVector = _direction * _force;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(forceVector, _forceMode2D);
    }
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            // วนลูปผ่าน Touch ทั้งหมดที่กำลังใช้งานอยู่
            foreach (Touch touch in Input.touches)
            {

                if (!isDragging && touch.phase == TouchPhase.Began)
                {

                    Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero);


                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        isDragging = true;
                        touchId = touch.fingerId;
                        startTouchPosition = touchWorldPos;
                        Debug.Log("Touch Began on Sprite at: " + startTouchPosition);
                        break;
                    }
                }

                else if (isDragging && touch.fingerId == touchId && touch.phase == TouchPhase.Moved)
                {
                    currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    dragDirection = currentTouchPosition - startTouchPosition;
                    oppositeDirection = -dragDirection;

                    Debug.Log("Drag Direction: " + dragDirection);
                    Debug.Log("Opposite Direction: " + oppositeDirection);


                }

                else if (isDragging && touch.fingerId == touchId && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    Debug.Log("Touch Ended. Final Opposite Direction: " + oppositeDirection);
                    isDragging = false;
                    touchId = -1;
                    dragDirection = Vector2.zero;
                    oppositeDirection = Vector2.zero;
                    break;
                }
            }
        }

    }
    void FixedUpdate()
    {
        if (shot)
        {
            shot = false;
            AddForce(oppositeDirection, force);
        }
    }
    void OnMouseDrag()
    {
        if (isClick)
        {
            CalculateOpposite();
        }
    }

    private void CalculateOpposite()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        Vector3 vectorToMouse = mouseWorldPosition - transform.position;

        Vector3 oppositeDirection = -vectorToMouse;

        if (vectorToMouse.magnitude > 0.01f)
        {
            oppositeDirection.Normalize();
        }
        else
        {
            oppositeDirection = Vector3.zero;
        }

        this.oppositeDirection = oppositeDirection;

        float distance = UnityEngine.Mathf.Clamp(vectorToMouse.magnitude / distanceToMaxForec, 0, distanceToMaxForec);

        this.force = UnityEngine.Mathf.Clamp(maxForce * distance, 0, maxForce);

        TARGET.position = transform.position + oppositeDirection;
    }
    void OnMouseDown()
    {
        isClick = true;
    }








    // private void TakeBvall(bool _up)
    // {
    //     BallDataWapper ballDataWapper = new BallDataWapper();
    //     ballDataWapper.playerSendIndex = GameManager.Instance.playerIndex;
    //     ballDataWapper.nextPLayerIndex = _up ? GameManager.Instance.playerIndex + 1 : GameManager.Instance.playerIndex - 1;
    //     ballDataWapper.xPosition = transform.position.x;
    //     ballDataWapper.yPosition = transform.position.y;
    //     ballDataWapper.xVelocity = rb.linearVelocityX;
    //     ballDataWapper.yVelocity = rb.linearVelocityY;

    //     string ballDataJson = JsonUtility.ToJson(ballDataWapper);
    //     Debug.Log("Send Ball");
    //     gameObject.SetActive(false);

    //     photonView.RPC("RPC_TakeBall", RpcTarget.Others, ballDataJson);
    // }

    // [PunRPC]
    // private void RPC_TakeBall(string _BallDataJson)
    // {
    //     Debug.Log("Recive Ball");
    //     BallDataWapper ballDataWapper = JsonUtility.FromJson<BallDataWapper>(_BallDataJson);
    //     if (ballDataWapper.nextPLayerIndex == GameManager.Instance.playerIndex)
    //     {

    //         gameObject.SetActive(true);
    //         transform.position = new Vector3(ballDataWapper.xPosition, -4.3f, 0);
    //         rb.linearVelocity = new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity);
    //     }

    // }


    public void SetUP(Vector2 _position, Vector2 _rbVelocity)
    {
        transform.position = new Vector3(_position.x, -4.3f, 0);
        rb.linearVelocity = _rbVelocity;
    }


    private bool BallOnScreen()
    {
        if (transform.position.y >= maxHeightScreen)
            return true;

        return false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger: " + collision.gameObject.name);
        if (!canTrigger) return;
        if (collision.TryGetComponent<PassWay>(out var way))
        {
            if (way.up)
            {
                ballHandle.TakeBvall(true);
            }
            else
            {
                ballHandle.TakeBvall(false);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
         Debug.Log($"Collision: " + collision.gameObject.name);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.InstantiationData != null && info.photonView.InstantiationData.Length > 0)
        {
            string id = (string)info.photonView.InstantiationData[0];
            ballID = id;
        }
    }
}
