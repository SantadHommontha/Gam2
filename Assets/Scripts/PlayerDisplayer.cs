using TMPro;
using UnityEngine;


// [System.Serializable]
// public class PlayerDisplayData
// {

// }
public class PlayerDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private PlayerDisplayerValue playerValue;


    void Start()
    {
        playerValue.OnValueChange += UpdateName;
    }

    void OnEnable()
    {
        UpdateName();
    }
    public void UpdateName() => UpdateName(playerValue.Value);

    private void UpdateName(PlayerData _playerData)
    {
        playerName.text = $"{GameManager.Instance.playerIndex}: {_playerData.playerName}";
    }


    public void KickBtn()
    {
        Debug.Log($"BTN {playerValue.Value.playerName} : {playerValue.Value.playerID}");
        TeamManager.Instance.KickPlayer(playerValue.Value.playerID);
    }
}
