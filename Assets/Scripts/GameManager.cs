using Photon.Pun;
using TMPro;
using UnityEngine;




public enum GameState
{
    None,
    Wait,
    Play,
    Over
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Public")]

    [SerializeField] private GameState gameState = GameState.None;
    [SerializeField] private float maxHeightScreen = 8.2f;
    [SerializeField] private float minHeightScreen = -1f;

    public Ball ball;
    public int playerIndex = 0;
    public TMP_InputField tMP_InputField;
    public bool iamEndPlayer = false;
    private bool isSend = false;

    private PhotonView photonView;
    [Header("Value")]
    [SerializeField] private Vector3Value ballPosition;



    public void StartState(GameState _gameState)
    {
        gameState = _gameState;
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.Wait:
                break;
            case GameState.Play:
                break;
            case GameState.Over:
                break;
        }
    }

    private void UpdateState()
    {
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.Wait:
                break;
            case GameState.Play:
                // if (BallOnScreen() && !iamEndPlayer && !isSend)
                // {
                //     TakeBvall();
                // }
                break;
            case GameState.Over:
                break;
        }
    }

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
        StartState(GameState.Play);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void TakeBvall()
    {
        BallDataWapper ballDataWapper = new BallDataWapper();
        ballDataWapper.playerSendIndex = playerIndex;
        ballDataWapper.nextPLayerIndex = playerIndex + 1;
        ballDataWapper.xPosition = ball.transform.position.x;
        ballDataWapper.yPosition = ball.transform.position.y;
        ballDataWapper.xVelocity = ball.rb.linearVelocityX;
        ballDataWapper.yVelocity = ball.rb.linearVelocityY;

        string ballDataJson = JsonUtility.ToJson(ballDataWapper);
        Debug.Log("Send Ball");
        ball.gameObject.SetActive(false);
        isSend = true;
        photonView.RPC("RPC_TakeBall", RpcTarget.Others, ballDataJson);
    }

    [PunRPC]
    private void RPC_TakeBall(string _BallDataJson)
    {
         Debug.Log("Recive Ball");
        BallDataWapper ballDataWapper = JsonUtility.FromJson<BallDataWapper>(_BallDataJson);
        if (ballDataWapper.nextPLayerIndex == playerIndex)
        {
            ball.gameObject.SetActive(true);
            ball.transform.position = new Vector3(ballDataWapper.xPosition, -1.5f, 0);
            ball.rb.linearVelocity = new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity);
        }

    }

    private bool BallOnScreen()
    {
        if (ball.gameObject.transform.position.y >= maxHeightScreen)
            return true;

        return false;
    }
    public void SetPlayerIndex()
    {
        playerIndex = int.Parse(tMP_InputField.text);
    }
}
