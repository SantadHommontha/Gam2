using UnityEngine;
using UnityEngine.UI;

public class Spectator : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    [SerializeField] private GameDataValue gameDataValue;
    [SerializeField] private GameObject controlUI;
    [SerializeField] private GameDataValue gameData;

    public void Setup()
    {
        toggle.isOn = true;
        OnToggleChange(toggle.isOn);
    }

    public void OnToggleChange(bool _value)
    {
        gameDataValue.Value.spacetator = _value;
    }

    public void EnterSpactator()
    {
        if (!gameData.Value.gamestart) return;
        Setup();
        controlUI.SetActive(false);

    }

}
