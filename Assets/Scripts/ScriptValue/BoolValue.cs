using UnityEngine;


[CreateAssetMenu(menuName ="Values/BoolValue")]
public class BoolValue : ScriptableValue<bool>
{
   [SerializeField] private bool initialValue;

    public override void ResetValue()
    {
        SetValue(initialValue);
    }
}
