using UnityEngine;
using Photon.Pun;

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
    private PhotonView photonView;
    private Team team = new Team();

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
        DontDestroyOnLoad(this.gameObject);
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

            sendBackJoinTeam.status = false;
            sendBackJoinTeam.massage = "Can't Join Team";
            sendBackJoinTeam.playerIndex = ap.Count + 1;

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

        GameManager.Instance.playerIndex = sendBackJoinTeam.playerIndex;
    }

}
