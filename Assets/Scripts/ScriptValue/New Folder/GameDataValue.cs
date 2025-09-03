using UnityEngine;


[CreateAssetMenu(menuName = "Values/GameDataValue")]
public class GameDataValue : ScriptableValue<GameData>
{
    [SerializeField] private GameData initialValue;

    public override void ResetValue()
    {
        GameData init = new GameData(initialValue);
        SetValue(init);
    }
}
