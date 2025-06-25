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

    [SerializeField] private TMP_InputField tMP_InputField;


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
        playerName.text = $"{_playerData.playerIndex}: {_playerData.playerName}";
        tMP_InputField.text = playerValue.Value.playerIndex.ToString();
    }


    public void KickBtn()
    {
        Debug.Log($"BTN {playerValue.Value.playerName} : {playerValue.Value.playerID}");
        TeamManager.Instance.KickPlayer(playerValue.Value.playerID);
    }

    public void SetBTN()
    {
        TeamManager.Instance.ChangePlayerIndex(playerValue.Value.playerID, int.Parse(tMP_InputField.text));

    }

}
