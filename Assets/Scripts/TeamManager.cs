using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System;

[System.Serializable]
public class SendBackJoinTeam
{
    public bool status;
    public string massage;
    public int playerIndex;
}
public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;
    public Action<SendBackJoinTeam> ac_sendBackJoinTeam;
    private PhotonView photonView;
    public Team team = new Team();





    [Header("Value")]
    [Space]
    [SerializeField] private SendBackJoinTeamValue sendBackJoinTeamValue;
    [SerializeField] private List<PlayerDisplayerValue> playerDisplayerValues;
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
        SendBackJoinTeam sendBackJoinTeam = new SendBackJoinTeam();

        if (team.TryToAddPlayer(playerData))
        {
            var ap = team.GetAllPlayer();

            sendBackJoinTeam.status = true;
            sendBackJoinTeam.massage = "Join Team";
            sendBackJoinTeam.playerIndex = ap.Count;
            //   Debug.Log("Ap: " + ap.Count);

        }
        else
        {
            sendBackJoinTeam.status = false;
            sendBackJoinTeam.massage = "Can't Join Team";
            sendBackJoinTeam.playerIndex = -1;
        }

        photonView.RPC("RPC_ReciveJoinTeamStatus", _info.Sender, JsonUtility.ToJson(sendBackJoinTeam));

    }

    [PunRPC]
    private void RPC_ReciveJoinTeamStatus(string _statusJson)
    {
        SendBackJoinTeam sendBackJoinTeam = JsonUtility.FromJson<SendBackJoinTeam>(_statusJson);
        //  Debug.Log("ApF: " + sendBackJoinTeam.playerIndex);
        //  IReciveJoinTeams[0].ReciveJoinTeamStatus(sendBackJoinTeam);


        sendBackJoinTeamValue.Value = sendBackJoinTeam;
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
}
