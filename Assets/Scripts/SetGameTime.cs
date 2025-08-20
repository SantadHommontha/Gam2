using UnityEngine;
using System.Linq;
using TMPro;
using Photon.Pun.Demo.PunBasics;
public class SetGameTime : MonoBehaviour
{
    [SerializeField] private GameSetting gameSetting;

    [SerializeField] private GameDataValue gameData;
    [SerializeField] private TMP_InputField tMP_InputField;


    void Start()
    {
        SetINput(gameSetting.gameTime.ToString());

    }

    public void SetINput(string _text)
    {
        tMP_InputField.text = _text;
        TextUpdate(_text);
        gameData.Value.gametimer = gameSetting.gameTime;
    }

    public void TextUpdate(string _text)
    {
        if (_text.All(char.IsDigit))
        {
            gameSetting.gameTime = int.Parse(_text);
            gameData.Value.gametimer = gameSetting.gameTime;
            GameManager.Instance.ChangeGameDataSetting();
        }
    }
}
