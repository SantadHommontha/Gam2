using TMPro;
using UnityEngine;

public class RoomCodeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameInfoValue gameInfo;

    [SerializeField] private bool useUpdateFunction;
    void Start()
    {
        gameInfo.OnValueChange += UpdateText;
    }
    public void UpdateText()
    {
        text.text = gameInfo.Value.roomCode;
    }

    public void UpdateText(GameInfo _gameInfo)
    {
        UpdateText();
    }

    void Update()
    {
        if (useUpdateFunction)
            UpdateText();
    }
}
