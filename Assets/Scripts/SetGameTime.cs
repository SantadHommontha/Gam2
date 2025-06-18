using UnityEngine;
using System.Linq;
using TMPro;
public class SetGameTime : MonoBehaviour
{
    [SerializeField] private GameSetting gameSetting;


    [SerializeField] private TMP_InputField tMP_InputField;


    void Start()
    {
        SetINput(gameSetting.gameTime.ToString());
    }

    public void SetINput(string _text)
    {
        tMP_InputField.text = _text;
        TextUpdate(_text);
    }

    public void TextUpdate(string _text)
    {
        if (_text.All(char.IsDigit))
        {
            gameSetting.gameTime = int.Parse(_text);
        }
    }
}
