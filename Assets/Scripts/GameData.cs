using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class GameData
{
    public int gamescore;
    public float gameTime;
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
    public static void SetToDefualtValue(GameData _data)
    {
        _data.gamescore = 0;
        _data.gameTime = 0;
        _data.gametimer = 0;
        _data.usetime = 0;
        _data.gamestart = false;
        _data.isAdmin = false;
        _data.roomCode = "";
        _data.spacetator = false;
        _data.isPlayer = false;
        _data.gameState = "";
    }
    public void SetData(GameData _gameData)
    {
        gamescore = _gameData.gamescore;
        gameTime = _gameData.gameTime;
        gametimer = _gameData.gametimer;
        usetime = _gameData.usetime;
        gamestart = _gameData.gamestart;
        isAdmin = _gameData.isAdmin;
        roomCode = _gameData.roomCode;
        spacetator = _gameData.spacetator;
        isPlayer = _gameData.isPlayer;
        gameState = _gameData.gameState;
    }
}
