using UnityEngine;


[CreateAssetMenu(menuName ="Values/MyPlayerDataInfoValue")]
public class MyPlayerDataInfoValue : ScriptableValue<MyPlayerDataInfo>
{
     [SerializeField] private MyPlayerDataInfo initialValue;
    
    public override void ResetValue()
    {
        SetValue(new MyPlayerDataInfo());
    }
}
