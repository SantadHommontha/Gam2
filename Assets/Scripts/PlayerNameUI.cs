using UnityEngine;
using Photon.Pun;
using TMPro;



public class PlayerNameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField tmp_playerName;


    [Header("Value")]
    [Space]
    [SerializeField] private StringValue value_playerName;
    [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;

    void Start()
    {
       // sendBackJoinTeamValue.OnValueChange += ReciveJoinTeamStatus;
    }
    public void EnterBTN()
    {
        PlayerData playerData = new PlayerData();

        value_playerName.Value = tmp_playerName.text;

        playerData.playerName = tmp_playerName.text;
        playerData.playerID = PhotonNetwork.LocalPlayer.UserId;

        TeamManager.Instance.JoinTeam(playerData);


    }

    // public void ReciveJoinTeamStatus(SendBackJoinTeam _sendBackJoinTeam)
    // {
    //     if (_sendBackJoinTeam.status)
    //     {
    //         gameObject.SetActive(false);
    //     }
    // }
}
