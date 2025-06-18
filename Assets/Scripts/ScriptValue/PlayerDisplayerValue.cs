using NUnit.Framework.Constraints;
using UnityEngine;

[CreateAssetMenu(menuName = "Values/PlayerDisplayerValue")]
public class PlayerDisplayerValue : ScriptableValue<PlayerData>
{
    [SerializeField] private string defaultName = "None";
    public override void ResetValue()
    {
        PlayerData playerData = new PlayerData();
        playerData.playerName = defaultName;
        SetValue(playerData);
    }
}
