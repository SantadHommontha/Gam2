using UnityEngine;

[CreateAssetMenu(menuName ="Values/StringValue")]
public class StringValue : ScriptableValue<string>
{
    [SerializeField] private string initialValue;

    public override void ResetValue()
    {
       SetValue(initialValue);
    }
}
