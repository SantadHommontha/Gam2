using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text gametime;
    [SerializeField] private string timeTextStart;
    [SerializeField] private string timeTextEnd;
    [Space]
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private string scoreTextStart;
    [SerializeField] private string scoreTextEnd;
    [Space]
    [SerializeField] private bool updateOnEnable = false;
    [SerializeField] private bool useUpdateFunction = true;
    [Header("Value")]
    [SerializeField] private GameDataValue gamedata;

    void OnEnable()
    {
        if (updateOnEnable)
            UpdateText();
    }

    void Update()
    {
        if (useUpdateFunction)
            UpdateText();
    }
    public void UpdateText() => UpdateText(gamedata.Value);


    public void UpdateText(GameData _gameData)
    {
      
        gametime.text = $"{timeTextStart}{_gameData.usetime}{timeTextEnd}";
        gameScore.text = $"{scoreTextStart}{_gameData.gamescore}{scoreTextEnd}";
    }
}
