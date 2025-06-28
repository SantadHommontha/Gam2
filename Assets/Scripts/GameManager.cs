using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class GameDataWapper
{
    public bool gameStart;
    public float gameTime;
}

[System.Serializable]
public class GameData
{
    public int gamescore;
    public float gametimer;
    public float usetime;
    public bool gamestart;
    public bool iamAdmin;
    public string roomCode;
    public bool spacetator;

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
    [SerializeField] private GameObject gamgeControl;

    //  public int playerIndex = 0;
    public TMP_InputField tMP_InputField;

    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private GameSetting gameSetting;

    private PhotonView photonView;
    [Header("Value")]
    [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;
    [SerializeField] private GameDataValue gameData;



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
    [SerializeField] private Toggle toggle;

    public void StartState(GameState _gameState)
    {
        EndState();
        gameState = _gameState;
        Debug.Log($"New State {gameState}");
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.EnterName:

                // Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                // Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);
                // Scene_Game_All_UI.Instance.playerIndex.SetActive(false);
                // Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(false);
                // sce
                Scene_Game_All_UI.Instance.HideAll();

                Scene_Game_All_UI.Instance.Panel_enterName.SetActive(true);
                //  enterName.Raise(this);
                break;
            case GameState.SetGame:

                Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);

                Scene_Game_All_UI.Instance.playerIndex.SetActive(true);
                Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(true);



                gameData.Value.gamestart = false;
                gameData.Value.gamescore = 0;
                gameData.Value.gametimer = gameSetting.gameTime;
                gameData.Value.usetime = 0;
                setGame.Raise(this);

                break;
            case GameState.Wait:
                Scene_Game_All_UI.Instance.HideAll();
                if (PhotonNetwork.IsMasterClient)
                {
                    Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                    Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);
                }
                else
                {
                    Scene_Game_All_UI.Instance.openMenuBTN.SetActive(true);
                    Scene_Game_All_UI.Instance.openControlBTN.SetActive(false);
                }
                Scene_Game_All_UI.Instance.playerIndex.SetActive(true);
                Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(true);

                gameData.Value.gamestart = false;
                gameData.Value.gamescore = 0;
                gameData.Value.gametimer = gameSetting.gameTime;
                gameData.Value.usetime = 0;

                int playerCount = TeamManager.Instance.playerCount;






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

                Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(true);


                gameData.Value.gamestart = true;
                gameTimer.SetTime(gameSetting.gameTime);
                gameTimer.StartTimer();
                if (PhotonNetwork.IsMasterClient)
                    SendGameData();
                //   RamdomLevel(level1).Raise(this, -979);
                break;
            case GameState.Over:
                Scene_Game_All_UI.Instance.openControlBTN.SetActive(false);
                Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                Scene_Game_All_UI.Instance.playerIndex.SetActive(false);

                Scene_Game_All_UI.Instance.backbtn.SetActive(true);
                if (PhotonNetwork.IsMasterClient)
                {
                    Scene_Game_All_UI.Instance.backBtn_text.text = "Back";
                }
                else
                {
                    Scene_Game_All_UI.Instance.backBtn_text.text = "Leave";
                }

                SpawnBall.Instance.RemoveAllBall();
                gameTimer.StopTimer();
                gameData.Value.gamestart = false;


                gameOver.Raise(this);
                break;
        }
    }

    private void EndState()
    {
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.Wait:
                Scene_Game_All_UI.Instance.openMenuBTN.SetActive(true);
                Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);
                break;
            case GameState.Play:
                Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(false);
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

                if (gameData.Value.gametimer <= 0)
                {
                    StartState(GameState.Over);
                }

                if (gameData.Value.gamescore >= gameSetting.scoreToWin)
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
         
 if (!gameData.Value.iamAdmin)
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
        ui_playerIndex.text = $"Player {myPlayerDataInfo.Value.playerIndex.ToString()} :: {gameData.Value.iamAdmin.ToString()}";
        
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
        gameData.Value.gamescore += _score;
        photonView.RPC("RPC_ReciveSCore", RpcTarget.Others, gameData.Value.gamescore);
    }
    [PunRPC]
    private void RPC_ReciveSCore(int _score)
    {
        gameData.Value.gamescore += _score;
    }


    public void NewRoom()
    {
        RoomManager.Instance.NewRoom();

    }
    public void LeaveBTN()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartState(GameState.SetGame);
        }
        else
        {
            RoomManager.Instance.LeaveRoomBTN();
        }
    }
    public void GameStart()
    {
        StartState(GameState.Play);
        if (gameData.Value.spacetator)
        {
            gamgeControl.SetActive(false);
        }
    }
    public void ResetGame()
    {
        StartState(GameState.Wait);
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("RPC_ResetGame", RpcTarget.Others);
    }
    [PunRPC]
    private void RPC_ResetGame()
    {
        StartState(GameState.Wait);
    }
    private void SendGameData()
    {
        GameDataWapper gameDataWapper = new GameDataWapper();
        gameDataWapper.gameStart = gameData.Value.gamestart;
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
            gameData.Value.gamestart = true;

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
