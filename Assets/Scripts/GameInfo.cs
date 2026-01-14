using UnityEngine;
[System.Serializable]
public class GameInfo
{
    public bool isAdmin;
    public bool isPlayer;
    public string playerName;
    public string roomCode;
    public int playerCount;
    public int myPlayerIndex;
    public int myLevel;
    public int mySubLevel;
    public string playerID;
    public string gameState;

    public GameInfo() { }
    public GameInfo(GameInfo _gameInfo)
    {
        SetData(_gameInfo);
    }
    public static void SetToDefualtValue(GameInfo _data)
    {
        _data.isAdmin = false;
        _data.isPlayer = false;
        _data.roomCode = "";
        _data.playerCount = 0;
        _data.myPlayerIndex = 0;
        _data.myLevel = 0;
        _data.mySubLevel = 0;
        _data.playerID = "";
        _data.playerName = "";
        _data.gameState = "";
    }
    public void SetData(GameInfo _gameInfo)
    {
        isAdmin = _gameInfo.isAdmin;
        isPlayer = _gameInfo.isPlayer;
        roomCode = _gameInfo.roomCode;
        playerCount = _gameInfo.playerCount;
        myPlayerIndex = _gameInfo.myPlayerIndex;
        myLevel = _gameInfo.myLevel;
        mySubLevel = _gameInfo.mySubLevel;
        playerID = _gameInfo.playerID;
        playerName = _gameInfo.playerName;
        gameState = _gameInfo.gameState;
    }

}
