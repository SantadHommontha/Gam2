
using TMPro;
using UnityEngine;

public class Scene_Game_All_UI : MonoBehaviour
{

    public static Scene_Game_All_UI Instance;
    [Header("Panel")]
    public GameObject Panel_enterName;
    public GameObject Panel_gameOver;
    public GameObject Panel_gamcontrol;


    [Header("Text")]
    public GameObject timeAndSocreGroup;
    public GameObject playerIndex;

    [Header("Button")]
    public GameObject openMenuBTN;
    public GameObject openControlBTN;




    [Header("GameOver_SubUI")]
    public GameObject backbtn;
    public TMP_Text backBtn_text;

    void Awake()
    {
        Instance = this;
    }

    public void HideAll()
    {
        //Panel
        Panel_enterName.SetActive(false);
        Panel_gameOver.SetActive(false);
        Panel_gamcontrol.SetActive(false);
        //Text
        timeAndSocreGroup.SetActive(false);
        playerIndex.SetActive(false);

        //Button
        timeAndSocreGroup.SetActive(false);
        playerIndex.SetActive(false);


        //GameOver_SubUI
        // backbtn.SetActive(false);


    }

}
