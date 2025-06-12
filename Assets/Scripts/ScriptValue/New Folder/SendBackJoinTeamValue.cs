using UnityEngine;


[CreateAssetMenu(menuName ="Values/SendBackJoinTeamValue")]
public class SendBackJoinTeamValue : ScriptableValue<SendBackJoinTeam>
{
     [SerializeField] private SendBackJoinTeam initialValue;
    
    public override void ResetValue()
    {
        SetValue(new SendBackJoinTeam());
    }
}
