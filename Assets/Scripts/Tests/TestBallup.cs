using JetBrains.Annotations;
using UnityEngine;

public class TestBallup : MonoBehaviour
{
    public BoolValue boolValue;
    public void SetTrue() => boolValue.Value = true;
    public void SetFalse() => boolValue.Value = false;


}
