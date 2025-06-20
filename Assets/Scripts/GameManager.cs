using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;



public enum GameState
{
    None,
    EnterName,
    Wait,
    SetGame,
    Play,
    Over
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Public")]

    [SerializeField] private GameState gameState = GameState.None;

    [SerializeField] private GameObject bottomLevel;

    public int playerIndex = 0;
    public TMP_InputField tMP_InputField;
   
    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private GameSetting gameSetting;

    private PhotonView photonView;
    [Header("Value")]

    [SerializeField] private SendBackJoinTeamValue sendBackJoinTeamValue;
    [SerializeField] private BoolValue gameStart;
    [SerializeField] private IntValue score;
    [SerializeField] private FloatValue timer;
    [SerializeField] private BoolValue iamAdmin;



    [Header("GameEvent")]

    [SerializeField] private GameEvent enterName;
    [SerializeField] private GameEvent wait;
    [SerializeField] private GameEvent gameOver;
    [SerializeField] private GameEvent setGame;
    [Header("Leavel")]

    [SerializeField] private List<GameEvent> level1;
    [SerializeField] private List<GameEvent> level2;
    [SerializeField] private List<GameEvent> level3;


    [Header("Test")]
    [SerializeField] private TMP_Text ui_playerIndex;


    public void StartState(GameState _gameState)
    {
        gameState = _gameState;
        Debug.Log($"New State {gameState}");
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.EnterName:
                enterName.Raise(this);
                break;
            case GameState.SetGame:
                setGame.Raise(this);
                break;
            case GameState.Wait:
                gameStart.Value = false;
                if (playerIndex == 1)
                    RamdomLevel(level1).Raise(this);
                else if (playerIndex == 2)
                    RamdomLevel(level2).Raise(this);
                else
                    RamdomLevel(level3).Raise(this);
                wait.Raise(this);

                break;
            case GameState.Play:
                gameStart.Value = true;
                gameTimer.SetTime(gameSetting.gameTime);
                gameTimer.StartTimer();
                //   RamdomLevel(level1).Raise(this, -979);
                break;
            case GameState.Over:
                gameOver.Raise(this);
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
                // }.
                if (gameTimer.timer <= 0)
                {
                    StartState(GameState.Over);
                }
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
        if (!PhotonNetwork.IsMessageQueueRunning)
            PhotonNetwork.IsMessageQueueRunning = true;

        if (!iamAdmin.Value)
            StartState(GameState.EnterName);
        else
            StartState(GameState.SetGame);
        sendBackJoinTeamValue.OnValueChange += ReciveJoinTeamStatus;
        //   StartState(GameState.Play);
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
        return _gameEvents[UnityEngine.Random.Range(0, _gameEvents.Count - 1)];
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


    public void AddScore(int _score = 1)
    {
        photonView.RPC("RPC_AddScore", RpcTarget.MasterClient, _score);
    }
    [PunRPC]
    private void RPC_AddScore(int _score)
    {
        score.Value += _score;
        photonView.RPC("RPC_ReciveSCore", RpcTarget.Others, score.Value);
    }
    [PunRPC]
    private void RPC_ReciveSCore(int _score)
    {
        score.Value += _score;
    }


    public void NewRoom()
    {
        RoomManager.Instance.NewRoom();
      
    }

    public void GameStart()
    {
        StartState(GameState.Play);
    }
}
