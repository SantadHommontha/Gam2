using UnityEngine;


[CreateAssetMenu(menuName = "Values/IntValue")]
public class IntValue : ScriptableValue<int>
{
    [SerializeField] int initialValue;

    public override void ResetValue()
    {
        SetValue(initialValue);
    }
}
