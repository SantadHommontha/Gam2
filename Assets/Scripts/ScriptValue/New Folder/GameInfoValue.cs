using UnityEngine;
[CreateAssetMenu(menuName = "Values/GameDataValue")]
public class GameInfoValue : ScriptableValue<GameInfo>
{
    [SerializeField] private GameInfo initialValue;

    public override void ResetValue()
    {
        GameInfo init = new GameInfo(initialValue);
        SetValue(init);
    }
}
