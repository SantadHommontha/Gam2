using System;
using TMPro;
using UnityEngine;

public class Display_Value : MonoBehaviour
{

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
        }
    }
    public void Reset()
    {
        SetOnValueChange();
        text_ui.text = resetText;
    }

    public void UpdateUI()
    {
        if (stringValue) Show_UI(stringValue.Value);
        if (intValue) IntValue(intValue.Value);
        if (floatValue) FloatValue(floatValue.Value);
    }
    private void Show_UI(string _text) => text_ui.text = $"{start_text}{_text}{end_text}";

    private void IntValue(int _int) => Show_UI(_int.ToString());
    private void FloatValue(float _float) => Show_UI(_float.ToString($"F{floatForMat}"));




}
