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
    [SerializeField] private GameObject bottomLevel;
    public Ball ball;
    public int playerIndex = 0;
    public TMP_InputField tMP_InputField;
    public bool iamEndPlayer = false;
    private bool isSend = false;

    private PhotonView photonView;
    [Header("Value")]
    [SerializeField] private Vector3Value ballPosition;

    [Header("Test")]
    [SerializeField] private TMP_Text ui_playerIndex;

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
        ui_playerIndex.text = playerIndex.ToString();
        if (playerIndex > 1)
            bottomLevel.SetActive(false);
        UpdateState();
    }

    
    public void SetPlayerIndex()
    {
        playerIndex = int.Parse(tMP_InputField.text);
    }
}
