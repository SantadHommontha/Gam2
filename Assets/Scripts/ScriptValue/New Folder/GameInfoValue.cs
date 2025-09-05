using UnityEngine;
[CreateAssetMenu(menuName = "Values/GameInfoValue")]
public class GameInfoValue : ScriptableValue<GameInfo>
{
    [SerializeField] private GameInfo initialValue;

    public override void ResetValue()
    {
        GameInfo init = new GameInfo(initialValue);
        SetValue(init);
    }
}
