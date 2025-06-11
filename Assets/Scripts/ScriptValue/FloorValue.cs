using UnityEngine;

[CreateAssetMenu(menuName ="Values/FloatValue")]
public class FloatValue : ScriptableValue<float>
{
    [SerializeField] private float initialValue;
    
    public override void ResetValue()
    {
        SetValue(initialValue);
    }
}
