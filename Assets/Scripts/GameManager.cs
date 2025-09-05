using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class SetLevelWarpper
{
    public SetLevel[] setLevels;
}

[System.Serializable]
public class SetLevel
{
    public int targetIndex;
    public int level;
    public int subLevel;
}


[System.Serializable]
public class GameDataWapper
{
    public bool gameStart;
    public float gameTime;


}

// [System.Serializable]
// public class GameData
// {

//     public int gamescore;
//     public float gametimer;
//     public float usetime;
//     public bool gamestart;
//     public bool isAdmin;
//     public bool isPlayer;
//     public string roomCode;
//     public bool spacetator;
//     public string gameState;
    
//     public GameData() { }
//     public GameData(GameData _gameData)
//     {
//         SetData(_gameData);
//     }
//     public void SetData(GameData _gameData)
//     {
//         gamescore = _gameData.gamescore;
//         gametimer = _gameData.gametimer;
//         usetime = _gameData.usetime;
//         gamestart = _gameData.gamestart;
//         isAdmin = _gameData.isAdmin;
//         roomCode = _gameData.roomCode;
//         spacetator = _gameData.spacetator;
//         isPlayer = _gameData.isPlayer;
//     }
// }

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

    [SerializeField] private string gameState = GameState.None.ToString();
    [SerializeField] private GameObject gamgeControl;
    private int currentBallIndex;

    //  public int playerIndex = 0;
    public TMP_InputField tMP_InputField;

    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private GameSetting gameSetting;

    private PhotonView photonView;
    [Header("Value")]
   // [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;
    [SerializeField] private GameDataValue gameData;
    [SerializeField] private GameInfoValue gameInfo;
    // [SerializeField] private BoolValue isPlayerValue;



    [Header("GameEvent")]

    [SerializeField] private GameEvent enterName;
    [SerializeField] private GameEvent wait;
    [SerializeField] private GameEvent gameOver;
    [SerializeField] private GameEvent setGame;
    [SerializeField] private GameEvent playGame;
    [Header("Leavel")]

    [SerializeField] private List<GameEvent> level1Player;

    [SerializeField] private List<GameEvent> level1;
    [SerializeField] private List<GameEvent> level2;
    [SerializeField] private List<GameEvent> level3;
    private int solo_index = 0;
    private int level1_index = 0;
    private int level2_index = 0;
    private int level3_index = 0;

    [Header("Test")]
    [SerializeField] private TMP_Text ui_playerIndex;
    [SerializeField] private Toggle toggle;

    #region GameState
    private void StartState(string _newState)
    {
        EndState();
        gameState = _newState;
        //   Debug.Log($"New State {gameState}");
        switch (gameState)
        {
            case "None":
                break;
            case "EnterName":

                // Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                // Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);
                // Scene_Game_All_UI.Instance.playerIndex.SetActive(false);
                // Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(false);
                // sce
                // Scene_Game_All_UI.Instance.HideAll();

                // Scene_Game_All_UI.Instance.Panel_enterName.SetActive(true);
                enterName.Raise(this);
                break;
            case "SetGame":

                // Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                // Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);

                // Scene_Game_All_UI.Instance.playerIndex.SetActive(true);
                // Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(true);



                gameData.Value.gamestart = false;
                gameData.Value.gamescore = 0;
                gameData.Value.gametimer = gameSetting.gameTime;
                gameData.Value.usetime = 0;
                setGame.Raise(this);

                break;
            case "Wait":
                //  Scene_Game_All_UI.Instance.HideAll();
                // if (PhotonNetwork.IsMasterClient)
                // {
                //     Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                //     Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);
                // }
                // else
                // {
                //  /   Scene_Game_All_UI.Instance.openMenuBTN.SetActive(true);
                //   //  Scene_Game_All_UI.Instance.openControlBTN.SetActive(false);
                // }
                //   Scene_Game_All_UI.Instance.playerIndex.SetActive(true);
                //   Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(true);

                gameData.Value.gamestart = false;
                gameData.Value.gamescore = 0;
                gameData.Value.gametimer = gameSetting.gameTime;
                gameData.Value.usetime = 0;

                int playerCount = TeamManager.Instance.playerCount;






                // if (playerCount == 1)
                // {
                //     RamdomLevel(level1Player, out level1_index).Raise(this);
                //     SentMyLevelIndex(myPlayerDataInfo.Value.playerIndex, level1_index);
                // }
                // else if (playerCount == 2)
                // {
                //     if (myPlayerDataInfo.Value.playerIndex == 1)
                //     {
                //         RamdomLevel(level1, out level1_index).Raise(this);
                //         SentMyLevelIndex(myPlayerDataInfo.Value.playerIndex, level1_index);
                //     }
                //     else
                //     {
                //         RamdomLevel(level3, out level3_index).Raise(this);
                //         SentMyLevelIndex(myPlayerDataInfo.Value.playerIndex, level3_index);
                //     }
                // }
                // else
                // {
                //     if (myPlayerDataInfo.Value.playerIndex == 1)
                //     {
                //         RamdomLevel(level1, out level1_index).Raise(this);
                //         SentMyLevelIndex(myPlayerDataInfo.Value.playerIndex, level1_index);
                //     }
                //     else if (myPlayerDataInfo.Value.playerIndex == 2)
                //     {
                //         RamdomLevel(level2, out level2_index).Raise(this);
                //         SentMyLevelIndex(myPlayerDataInfo.Value.playerIndex, level2_index);
                //     }
                //     else
                //     {
                //         RamdomLevel(level3, out level3_index).Raise(this);
                //         SentMyLevelIndex(myPlayerDataInfo.Value.playerIndex, level3_index);
                //     }
                // }

                wait.Raise(this);

                break;
            case "Play":

                // Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(true);


                gameData.Value.gamestart = true;
                gameTimer.SetTime(gameSetting.gameTime);
                gameTimer.StartTimer();
                if (PhotonNetwork.IsMasterClient)
                {
                    SendGameData(gameData.Value);
                    ChangeObserverSceneToCurrentBall(1);
                }

                playGame.Raise(this);

                //   RamdomLevel(level1).Raise(this, -979);
                break;
            case "Over":
                //  Scene_Game_All_UI.Instance.openControlBTN.SetActive(false);
                //   Scene_Game_All_UI.Instance.openMenuBTN.SetActive(false);
                //   Scene_Game_All_UI.Instance.playerIndex.SetActive(false);

                //   Scene_Game_All_UI.Instance.backbtn.SetActive(true);
                gameOver.Raise(this);
                if (PhotonNetwork.IsMasterClient)
                {
                    //     Scene_Game_All_UI.Instance.backBtn_text.text = "Back";
                    SpawnBall.Instance.RemoveAllBall();
                }
                // else
                // {
                // //    Scene_Game_All_UI.Instance.backBtn_text.text = "Leave";
                // }


                gameTimer.StopTimer();
                gameData.Value.gamestart = false;


                break;
        }
    }
    public void StartState(GameState _gameState)
    {
        StartState(_gameState.ToString());
    }

    private void EndState()
    {
        switch (gameState)
        {
            case "None":
                break;
            case "Wait":
                //  Scene_Game_All_UI.Instance.openMenuBTN.SetActive(true);
                //   Scene_Game_All_UI.Instance.openControlBTN.SetActive(true);
                break;
            case "Play":
                //  Scene_Game_All_UI.Instance.timeAndSocreGroup.SetActive(false);
                break;
            case "Over":
                break;
        }
    }
    private void UpdateState()
    {
        switch (gameState)
        {
            case "None":
                break;
            case "Wait":
                break;
            case "Play":

                if (gameData.Value.gametimer <= 0)
                {
                    StartState(GameState.Over);
                }

                if (gameData.Value.gamescore >= gameSetting.scoreToWin)
                {
                    StartState(GameState.Over);
                }
                break;
            case "Over":
                break;
        }
    }
    #endregion

    #region Unity Function
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


        if (gameData.Value.isAdmin)
        {
            if (gameData.Value.isPlayer)
                StartState(GameState.EnterName);
            else
                StartState(GameState.SetGame);


        }
        else
        {
            StartState(GameState.EnterName);
        }
   //     myPlayerDataInfo.OnValueChange += ReciveJoinTeamStatus;
        //   StartState(GameState.Play);
    }

    // Update is called once per frame
    void Update()
    {
        ui_playerIndex.text = $"Player {gameInfo.Value.myPlayerIndex.ToString()}";

        // if (playerIndex > 1)
        //     bottomLevel.SetActive(false);
        UpdateState();
    }
    #endregion

    #region Random Level
    private int RamdomLevel(List<GameEvent> _gameEvents)
    {
        return RamdomLevel(_gameEvents, out int _index);
    }
    private int RamdomLevel(List<GameEvent> _gameEvents, out int _index)
    {
        _index = UnityEngine.Random.Range(0, _gameEvents.Count - 1);
        return _index;
    }

    public void RandomLevel(int _playerCount)
    {
        SetLevel[] setLevels = new SetLevel[_playerCount];

        Func<int, (int, int)> _level = (_index) =>
        {
            if (_index == 1) return (1, RamdomLevel(level1));
            else if (_index == 2) return (2, RamdomLevel(level2));
            else return (3, RamdomLevel(level3));
        };

        if (_playerCount == 1)
        {
            setLevels[0].targetIndex = 1;
            setLevels[0].level = 0;
            setLevels[0].subLevel = RamdomLevel(level1Player);
        }
        else if (_playerCount > 0)
        {
            for (int i = 0; i < _playerCount; i++)
            {
                setLevels[i] = new SetLevel();


                setLevels[i].targetIndex = i + 1;
                var lv = _level(i + 1);
                setLevels[i].level = lv.Item1;
                setLevels[i].subLevel = lv.Item2;

            }
        }

        SetLevelWarpper setLevelWarpper = new SetLevelWarpper();
        setLevelWarpper.setLevels = setLevels;
        string jsonData = JsonUtility.ToJson(setLevelWarpper);
        photonView.RPC("RPC_SetLevel", RpcTarget.Others, jsonData);
    }
    [PunRPC]
    private void RPC_SetLevel(string _jsonData)
    {
        //   Debug.Log($"Recive JsonData: {_jsonData}");
        SetLevelWarpper setLevelWarpper = JsonUtility.FromJson<SetLevelWarpper>(_jsonData);
        foreach (var T in setLevelWarpper.setLevels)
        {
            if (T.targetIndex == gameInfo.Value.myPlayerIndex)
            {
                Debug.Log($"SetLevel: {T.level} {T.subLevel}");
                GameManager.Instance.SetMyLevel(T.level, T.subLevel);
            }
        }
        // GameManager.Instance.StartState(GameState.Wait);
    }
    #endregion
    public void ReciveJoinTeamStatus(MyPlayerDataInfo _myPlayerDataInfo)
    {
        if (_myPlayerDataInfo.status)
        {
            gameInfo.Value.myPlayerIndex = _myPlayerDataInfo.playerIndex;
            StartState(GameState.Wait);
        }
    }

    public void SetPlayerIndex()
    {
        gameInfo.Value.myPlayerIndex = int.Parse(tMP_InputField.text);
    }

    #region Score
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
    #endregion
    #region Game Button
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
    #endregion



    [PunRPC]
    private void RPC_ResetGame()
    {
        StartState(GameState.Wait);
    }
    private void SendGameData(GameData _data)
    {
        GameData gameDataWapper = new GameData(_data);


        string dataJson = JsonUtility.ToJson(gameDataWapper);

        photonView.RPC("RPC_GameData", RpcTarget.Others, dataJson);
    }
    [PunRPC]
    private void RPC_GameData(string _dataJson)
    {
        GameData gameDataWapper = JsonUtility.FromJson<GameData>(_dataJson);
        this.gameData.Value.SetData(gameDataWapper);

        StartState(gameData.Value.gameState);

        // if (gameDataWapper.gameStart)
        // {
        //     gameSetting.gameTime = gameDataWapper.gameTime;
        //     StartState(GameState.Play);
        //     gameData.Value.gamestart = true;

        // }
    }

    #region Change player index
    public void SetPlayerIndex(int _playerIndex, int _newIndex)
    {
        var player = TeamManager.Instance.team.GetPlayerByIndex(_playerIndex);

        photonView.RPC("RPC_SetPlayerIndex", player.info.Sender, _newIndex);
    }

    [PunRPC]
    private void RPC_SetPlayerIndex(int _newIndex)
    {

        gameInfo.Value.myPlayerIndex = _newIndex;
        StartState(GameState.Wait);
    }
    #endregion

    public void SetMyLevel(int _level, int _subLevel)
    {
        Debug.Log("SetMyLevel");
        gameInfo.Value.myLevel = _level;
        gameInfo.Value.mySubLevel = _subLevel;
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

    public void ChangeGameDataSetting()
    {
        var gameData = new GameData(this.gameData.Value);

        var jsonData = JsonUtility.ToJson(gameData);
        photonView.RPC("RPC_ChangeGameData", RpcTarget.Others, jsonData);
    }

    [PunRPC]
    private void RPC_ChangeGameData(string _jsonData)
    {
        this.gameData.Value = JsonUtility.FromJson<GameData>(_jsonData);



    }

    public void SentMyLevelIndex(int _myIndex, int _levelIndex)
    {
        int[] index = { _myIndex, _levelIndex };
        photonView.RPC("RPC_SentMyLevelIndex", RpcTarget.MasterClient, index);
    }
    [PunRPC]
    private void RPC_SentMyLevelIndex(int[] _index)
    {
        int _myIndex = _index[0];
        int _levelIndex = _index[1];

        if (_myIndex == 1)
        {
            level1_index = _levelIndex;
        }
        else if (_myIndex == 2)
        {
            level2_index = _levelIndex;
        }
        else if (_myIndex == 3)
        {
            level3_index = _levelIndex;
        }


    }


    public void SetCurrentBallIndex(int _index)
    {
        Debug.Log("SetCurrentBallIndex : " + _index);
        photonView.RPC("RPC_SetCurrentBallIndex", RpcTarget.MasterClient, _index);
    }
    [PunRPC]
    private void RPC_SetCurrentBallIndex(int _index)
    {
        currentBallIndex = _index;
        //    Debug.Log("RPC_SetCurrentBallIndex : " + _index);
        ChangeObserverSceneToCurrentBall(currentBallIndex);
    }

    private void ChangeObserverSceneToCurrentBall(int _currentBall)
    {
        //  Debug.Log("ChangeObserverSceneToCurrentBall : " + _currentBall);
        if (_currentBall == 1)
        {
            if (TeamManager.Instance.playerCount == 1)
            {
                level1Player[level1_index].Raise(this);
            }
            else
            {

                level1[level1_index].Raise(this);
            }
        }
        else if (_currentBall == 2)
        {

            if (TeamManager.Instance.playerCount == 2)
            {
                level3[level2_index].Raise(this);
            }
            else
            {
                level2[level2_index].Raise(this);
            }

        }
        else if (_currentBall == 3)
        {
            level3[level3_index].Raise(this);
        }
    }






    private void GameOver()
    {

    }


}
