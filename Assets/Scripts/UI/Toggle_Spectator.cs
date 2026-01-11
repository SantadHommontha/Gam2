using UnityEngine;

public class Toggle_Spectator : MonoBehaviour
{
    [SerializeField] private GameDataValue gameData;
    [SerializeField] private bool setValueOnStart;

    [SerializeField] private bool valueOnSet;

    private void Start()
    {
        if (setValueOnStart)
        {
            OnToggleChange(valueOnSet);
        }
    }
    public void OnToggleChange(bool value)
    {
        gameData.Value.spacetator = value;
    }
}
