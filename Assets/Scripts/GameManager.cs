using System.Collections.Generic;
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
    [SerializeField] private SendBackJoinTeamValue sendBackJoinTeamValue;


    [Header("GameEvent")]
    [Space]
    [SerializeField] private List<GameEvent> level1;
    [SerializeField] private List<GameEvent> level2;
    [SerializeField] private List<GameEvent> level3;

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
                if (playerIndex == 1)
                    RamdomLevel(level1).Raise(this, -979);
                else if (playerIndex == 2)
                    RamdomLevel(level2).Raise(this, -979);
                else
                    RamdomLevel(level3).Raise(this, -979);
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

    private GameEvent RamdomLevel(List<GameEvent> _gameEvents)
    {
        return _gameEvents[UnityEngine.Random.Range(0, _gameEvents.Count)];
    }









    public void ReciveJoinTeamStatus(SendBackJoinTeam _sendBackJoinTeam)
    {
        if (_sendBackJoinTeam.status)
        {
            playerIndex = _sendBackJoinTeam.playerIndex;
            StartState(GameState.Wait);
        }
    }





    public void SetPlayerIndex()
    {
        playerIndex = int.Parse(tMP_InputField.text);
    }
}
