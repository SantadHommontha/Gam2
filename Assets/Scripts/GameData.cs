using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class GameData
{
    public int gamescore;
    public float gametimer;
    public float usetime;
    public bool gamestart;
    public bool isAdmin;
    public bool isPlayer;
    public string roomCode;
    public bool spacetator;
    public string gameState;

    public GameData() { }
    public GameData(GameData _gameData)
    {
        SetData(_gameData);
    }
    public void SetData(GameData _gameData)
    {
        gamescore = _gameData.gamescore;
        gametimer = _gameData.gametimer;
        usetime = _gameData.usetime;
        gamestart = _gameData.gamestart;
        isAdmin = _gameData.isAdmin;
        roomCode = _gameData.roomCode;
        spacetator = _gameData.spacetator;
        isPlayer = _gameData.isPlayer;
    }
}
