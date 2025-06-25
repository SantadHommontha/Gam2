using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Photon.Pun.Demo.Cockpit;
using System.Linq;



[System.Serializable]
public class GameDataWapper
{
    public bool gameStart;
    public float gameTime;
}

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
    public bool isPlayer;
    public static GameManager Instance;
    [Header("Public")]

    [SerializeField] private GameState gameState = GameState.None;

    [SerializeField] private GameObject bottomLevel;

    //  public int playerIndex = 0;
    public TMP_InputField tMP_InputField;

    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private GameSetting gameSetting;

    private PhotonView photonView;
    [Header("Value")]

    [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;
    [SerializeField] private BoolValue gameStart;
    [SerializeField] private IntValue score;
    [SerializeField] private FloatValue timer;
    [SerializeField] private BoolValue iamAdmin;
    // [SerializeField] private PlayerDisplayerValue myPlayerdata;



    [Header("GameEvent")]

    [SerializeField] private GameEvent enterName;
    [SerializeField] private GameEvent wait;
    [SerializeField] private GameEvent gameOver;
    [SerializeField] private GameEvent setGame;
    [Header("Leavel")]

    [SerializeField] private List<GameEvent> level1Player;
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
                int playerCount = TeamManager.Instance.playerCount;
                Debug.Log($"PlayerCount: {playerCount}");
                if (playerCount == 1)
                {
                    RamdomLevel(level1Player).Raise(this);
                }
                else if (playerCount == 2)
                {
                    if (myPlayerDataInfo.Value.playerIndex == 1)
                        RamdomLevel(level1).Raise(this);
                    else
                        RamdomLevel(level3).Raise(this);
                }
                else
                {
                    if (myPlayerDataInfo.Value.playerIndex == 1)
                        RamdomLevel(level1).Raise(this);
                    else if (myPlayerDataInfo.Value.playerIndex == 2)
                        RamdomLevel(level2).Raise(this);
                    else
                        RamdomLevel(level3).Raise(this);
                }

                wait.Raise(this);

                break;
            case GameState.Play:
                gameStart.Value = true;
                gameTimer.SetTime(gameSetting.gameTime);
                gameTimer.StartTimer();
                if (PhotonNetwork.IsMasterClient)
                    SendGameData();
                //   RamdomLevel(level1).Raise(this, -979);
                break;
            case GameState.Over:
                gameTimer.StopTimer();
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
        if (!PhotonNetwork.InRoom)
            SceneManager.LoadScene("Loading");
        if (!PhotonNetwork.IsMessageQueueRunning)
            PhotonNetwork.IsMessageQueueRunning = true;

#if UNITY_EDITOR

        if (isPlayer)
        {
            StartState(GameState.EnterName);
        }
        else
        {
            StartState(GameState.SetGame);
        }

#else
            if (!iamAdmin.Value)
            StartState(GameState.EnterName);
        else
            StartState(GameState.SetGame);

        
#endif

        // if (isPlayer)
        // {
        //     StartState(GameState.EnterName);
        // }
        // else
        // {
        //     if (!iamAdmin.Value)
        //         StartState(GameState.EnterName);
        //     else
        //         StartState(GameState.SetGame);
        // }

        myPlayerDataInfo.OnValueChange += ReciveJoinTeamStatus;
        //   StartState(GameState.Play);
    }

    // Update is called once per frame
    void Update()
    {
        ui_playerIndex.text = myPlayerDataInfo.Value.playerIndex.ToString();
        // if (playerIndex > 1)
        //     bottomLevel.SetActive(false);
        UpdateState();
    }

    private GameEvent RamdomLevel(List<GameEvent> _gameEvents)
    {
        return _gameEvents[UnityEngine.Random.Range(0, _gameEvents.Count - 1)];
    }


    public void ReciveJoinTeamStatus(MyPlayerDataInfo _myPlayerDataInfo)
    {
        if (_myPlayerDataInfo.status)
        {
            myPlayerDataInfo.Value.playerIndex = _myPlayerDataInfo.playerIndex;
            StartState(GameState.Wait);
        }
    }

    public void SetPlayerIndex()
    {
        myPlayerDataInfo.Value.playerIndex = int.Parse(tMP_InputField.text);
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
    private void SendGameData()
    {
        GameDataWapper gameDataWapper = new GameDataWapper();
        gameDataWapper.gameStart = gameStart.Value;
        gameDataWapper.gameTime = gameSetting.gameTime;

        string dataJson = JsonUtility.ToJson(gameDataWapper);

        photonView.RPC("RPC_GameData", RpcTarget.Others, dataJson);
    }
    [PunRPC]
    private void RPC_GameData(string _dataJson)
    {
        GameDataWapper gameDataWapper = JsonUtility.FromJson<GameDataWapper>(_dataJson);
        if (gameDataWapper.gameStart)
        {
            gameSetting.gameTime = gameDataWapper.gameTime;
            StartState(GameState.Play);

        }
    }


    public void SetPlayerIndex(int _playerIndex, int _newIndex)
    {
        var player = TeamManager.Instance.team.GetPlayerByIndex(_playerIndex);

        photonView.RPC("RPC_SetPlayerIndex", player.info.Sender, _newIndex);
    }

    [PunRPC]
    private void RPC_SetPlayerIndex(int _newIndex)
    {

        myPlayerDataInfo.Value.playerIndex = _newIndex;
        StartState(GameState.Wait);
    }


    public void SetMyLevel(int _level, int _subLevel)
    {
        Debug.Log("SetMyLevel");
        switch (_level)
        {
            case 0:
                level1Player[_subLevel].Raise(this);
                break;
            case 1:
                level1[_subLevel].Raise(this);
                break;
            case 2:
                level2[_subLevel].Raise(this);
                break;
            case 3:
                level3[_subLevel].Raise(this);

                break;
        }

    }
}
