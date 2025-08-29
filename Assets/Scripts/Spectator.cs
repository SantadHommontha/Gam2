using UnityEngine;
using UnityEngine.UI;

public class Spectator : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    [SerializeField] private GameDataValue gameDataValue;

    public void Setup()
    {
        toggle.isOn = true;
        OnToggleChange(toggle.isOn);
    }

    public void OnToggleChange(bool _value)
    {
        gameDataValue.Value.spacetator = _value;
    }


}
