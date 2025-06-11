using UnityEngine;
[CreateAssetMenu(menuName = "Values/Vector3Value")]
public class Vector3Value : ScriptableValue<Vector3>
{
    [SerializeField] private Vector3 initialValue;

    public override void ResetValue()
    {
        SetValue(initialValue);
    }
}
