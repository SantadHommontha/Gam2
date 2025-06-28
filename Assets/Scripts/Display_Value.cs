using System;
using TMPro;
using UnityEngine;


public class Display_Value : MonoBehaviour
{
    public enum GameDataValueShow
    {
        gamescore,
        gametimer,
        gamestart,
        iamAdmin,
        roomCode
    }
    [SerializeField] private TMP_Text text_ui;
    [SerializeField] private string start_text;
    [SerializeField] private string end_text;
    [SerializeField] private string resetText;
    [Space]
    [SerializeField] private bool updateTextWhentValueChange = true;

    [Space]
    [SerializeField][Range(0, 3)] private int floatForMat = 0;
    [Header("Value")]
    [SerializeField] private IntValue intValue;
    [SerializeField] private FloatValue floatValue;
    [SerializeField] private StringValue stringValue;
    [Space]
    [SerializeField] private GameDataValueShow gameDataValueShow = GameDataValueShow.gamescore;
    [SerializeField] private GameDataValue gameDataValue;

    [Space]
    [SerializeField] private bool useUpdateFunction  =true;

    void Start()
    {
        SetOnValueChange();
    }

    public void SetOnValueChange()
    {
        if (updateTextWhentValueChange)
        {
            if (stringValue) stringValue.OnValueChange += Show_UI;
            if (intValue) intValue.OnValueChange += IntValue;
            if (floatValue) floatValue.OnValueChange += FloatValue;
            if (gameDataValue) gameDataValue.OnValueChange += GameDataValue;

        }
    }
    public void Reset()
    {
        SetOnValueChange();
        text_ui.text = resetText;
    }
    void Update()
    {
        if (useUpdateFunction)
            UpdateUI();

    }
    public void UpdateUI()
    {
        if (stringValue) Show_UI(stringValue.Value);
        if (intValue) IntValue(intValue.Value);
        if (floatValue) FloatValue(floatValue.Value);
        if (gameDataValue) GameDataValue(gameDataValue.Value);
    }
    private void Show_UI(string _text) => text_ui.text = $"{start_text}{_text}{end_text}";

    private void IntValue(int _int) => Show_UI(_int.ToString());
    private void FloatValue(float _float) => Show_UI(_float.ToString($"F{floatForMat}"));


    private void GameDataValue(GameData _gameData)
    {
        switch (gameDataValueShow)
        {
            case GameDataValueShow.gamescore:
                IntValue(gameDataValue.Value.gamescore);
                break;
            case GameDataValueShow.gametimer:
                FloatValue(gameDataValue.Value.gametimer);
                break;
            case GameDataValueShow.gamestart:
                Show_UI(gameDataValue.Value.gamestart.ToString());
                break;
            case GameDataValueShow.iamAdmin:
                Show_UI(gameDataValue.Value.iamAdmin.ToString());
                break;
            case GameDataValueShow.roomCode:
                Show_UI(gameDataValue.Value.roomCode);
                break;

        }
    }


}
