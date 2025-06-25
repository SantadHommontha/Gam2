using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
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
public class MyPlayerDataInfo : PlayerData
{
    public MyPlayerDataInfo()
    {

    }
    public MyPlayerDataInfo(PlayerData _playerdata)
    {
        playerID = _playerdata.playerID;
        teamName = _playerdata.teamName;
        playerIndex = _playerdata.playerIndex;
        playerName = _playerdata.playerName;
    }
    public bool status;
    public string massage;
    public int currentPlayer;

}
[System.Serializable]
public class TeamStatus
{
    public int playerCount;
}
public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;
    public Action<MyPlayerDataInfo> ac_sendBackJoinTeam;
    private PhotonView photonView;
    public Team team = new Team();

    public int playerCount;



    [Header("Value")]
    [Space]
    [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;
    [SerializeField] private List<PlayerDisplayerValue> playerDisplayerValues;


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        photonView = GetComponent<PhotonView>();
    }
    public int PlayerCount()
    {
        RequiteTeamStatus();
        return playerCount;
    }
    void Start()
    {
        //  DontDestroyOnLoad(this.gameObject);
        team.OnPlayerTeamChange += UpdatePlayerDisplay;
    }

    public void JoinTeam(PlayerData _playerData)
    {

        photonView.RPC("RPC_JoinTeam", RpcTarget.MasterClient, JsonUtility.ToJson(_playerData));

    }

    [PunRPC]
    private void RPC_JoinTeam(string _playerDataJson, PhotonMessageInfo _info)
    {
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(_playerDataJson);
        playerData.info = _info;
        MyPlayerDataInfo sendBackJoinTeam = new MyPlayerDataInfo();

        if (team.CanAddPlayere(playerData))
        {
            var ap = team.GetAllPlayer();


            int _index = ap.Count + 1;
            playerData.playerIndex = _index;

            sendBackJoinTeam.status = true;
            sendBackJoinTeam.massage = "Join Team";
            sendBackJoinTeam.playerIndex = _index;
            sendBackJoinTeam.playerID = playerData.playerID;
            sendBackJoinTeam.playerName = playerData.playerName;
            team.AddPlayer(playerData);
            var pc = team.GetAllPlayer().Count;
            sendBackJoinTeam.currentPlayer = team.GetAllPlayer().Count;
            //  playerCount = team.GetAllPlayer().Count;
            //   Debug.Log("Ap: " + ap.Count);

        }
        else
        {
            sendBackJoinTeam.status = false;
            sendBackJoinTeam.massage = "Can't Join Team";
            sendBackJoinTeam.playerIndex = -1;
        }

        photonView.RPC("RPC_ReciveJoinTeamStatus", _info.Sender, JsonUtility.ToJson(sendBackJoinTeam));

        var allP = team.GetAllPlayer();
        Debug.Log($"EIEI {allP.Count}");
        SetLevel[] setLevels = new SetLevel[allP.Count];

        for (int i = 0; i < allP.Count; i++)
        {
            Debug.Log("----------------");
            setLevels[i] = new SetLevel();
            if (allP.Count == 1)
            {

                setLevels[i].targetIndex = i + 1;
                setLevels[i].level = 0;
                setLevels[i].subLevel = 0;
            }
            else
            {

                setLevels[i].targetIndex = i + 1;
                setLevels[i].level = i == allP.Count - 1 ? 3 : i +1;
                setLevels[i].subLevel = 0;
            }

        }

        SetLevelWarpper setLevelWarpper = new SetLevelWarpper();
        setLevelWarpper.setLevels = setLevels;
        string jsonData = JsonUtility.ToJson(setLevelWarpper);
        Debug.Log($"Send JsonData: {jsonData}");
        photonView.RPC("RPC_PlayerChange", RpcTarget.All, jsonData);
    }
    [PunRPC]
    private void RPC_PlayerChange(string _jsonData)
    {
        Debug.Log($"Recive JsonData: {_jsonData}");
        SetLevelWarpper setLevelWarpper = JsonUtility.FromJson<SetLevelWarpper>(_jsonData);
        foreach (var T in setLevelWarpper.setLevels)
        {
            if (T.targetIndex == myPlayerDataInfo.Value.playerIndex)
            {
                Debug.Log($"SetLevel: {T.level} {T.subLevel}");
                GameManager.Instance.SetMyLevel(T.level, T.subLevel);
            }
        }
        // GameManager.Instance.StartState(GameState.Wait);
    }
    [PunRPC]
    private void RPC_ReciveJoinTeamStatus(string _statusJson)
    {
        MyPlayerDataInfo sendBackJoinTeam = JsonUtility.FromJson<MyPlayerDataInfo>(_statusJson);
        //  Debug.Log("ApF: " + sendBackJoinTeam.playerIndex);
        //  IReciveJoinTeams[0].ReciveJoinTeamStatus(sendBackJoinTeam);


        myPlayerDataInfo.Value = sendBackJoinTeam;
        playerCount = sendBackJoinTeam.currentPlayer;
        // if (sendBackJoinTeam.status)
        //     {
        //         GameManager.Instance.playerIndex = sendBackJoinTeam.playerIndex;

        //     }
    }

    public void LeaveRoom(string _playerID)
    {
        photonView.RPC("RPC_LEaveRoom", RpcTarget.MasterClient, _playerID);
    }
    [PunRPC]
    private void RPC_LEaveRoom(string _playerID)
    {
        KickPlayer(_playerID);
    }
    #region  Kick

    public void KickPlayer(string _playerID)
    {

        var player = team.GetPlayerByID(_playerID);

        photonView.RPC("RPC_KickPlayer", player.info.Sender);

        team.RemovePlayer(_playerID);

        Debug.Log("Kick");




    }

    [PunRPC]
    private void RPC_KickPlayer()
    {
        RoomManager.Instance.LeaveRoom();
    }

    #endregion

    public void OnNewRoom()
    {
        var ap = team.GetAllPlayer();
        PlayerData[] allPlayer = new PlayerData[ap.Count];
        ap.CopyTo(allPlayer);

        foreach (var T in allPlayer)
        {
            KickPlayer(T.playerID);
        }
    }

    private void UpdatePlayerDisplay()
    {
        int num = 1;
        foreach (var p in playerDisplayerValues)
        {
            PlayerData playerData = new PlayerData();
            playerData.playerName = $"Player {num++}";
            p.Value = playerData;
        }

        var allPLayer = team.GetAllPlayer();
        for (int i = 0; i < allPLayer.Count; i++)
        {
            var playerDB = new PlayerData();
            playerDB = allPLayer[i];
            playerDisplayerValues[i].Value = playerDB;
        }

    }


    public void ChangePlayerIndex(string _playerID, int _newIndex)
    {
        var player = team.GetPlayerByID(_playerID);

        player.playerIndex = _newIndex;

        MyPlayerDataInfo myPlayerDataInfo = new MyPlayerDataInfo(player);
        myPlayerDataInfo.playerIndex = player.playerIndex;
        myPlayerDataInfo.status = true;

        photonView.RPC("RPC_ReciveJoinTeamStatus", player.info.Sender, JsonUtility.ToJson(myPlayerDataInfo));

    }
    [PunRPC]
    public void RPC_ChangePlayerIndex(string _jsonData)
    {
        MyPlayerDataInfo myPlayerDataInfo = JsonUtility.FromJson<MyPlayerDataInfo>(_jsonData);


    }

    public void RequiteTeamStatus()
    {
        photonView.RPC("RPC_TeamStatus", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_TeamStatus(PhotonMessageInfo _info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            TeamStatus teamStatus = new TeamStatus();
            teamStatus.playerCount = team.DataCount;

            photonView.RPC("RPC_ReiveTeamStatus", _info.Sender, JsonUtility.ToJson(teamStatus));
        }

    }
    [PunRPC]
    private void RPC_ReiveTeamStatus(string _jsonData)
    {
        TeamStatus teamStatus = JsonUtility.FromJson<TeamStatus>(_jsonData);

        playerCount = teamStatus.playerCount;
    }

}
