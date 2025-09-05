using UnityEngine;

public class GameInfo
{
    public bool isAdmin;
    public bool isPlayer;
    public string roomCode;
    public int playerCount;
    public int myPlayerIndex;
    public int myLevel;
    public int mySubLevel;
    public string playerID;

     public GameInfo() { }
    public GameInfo(GameInfo _gameInfo)
    {
        SetData(_gameInfo);
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
    }

}
