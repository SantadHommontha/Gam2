using UnityEngine.UI;
using UnityEngine;

public class LoadingSceneUiHandel : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject childayCanvas;


    [SerializeField] private JoinLobby joinLobby;
    [SerializeField] private MovingCurtain movingCurtain;
    [SerializeField] private Image loadBar;
    [SerializeField] private TapScreen tapScreenOnLoaddingScene;
    void Start()
    {
        loadingCanvas.SetActive(true);
        startCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
        childayCanvas.SetActive(false);

tapScreenOnLoaddingScene.enabled = false;

    }

    void Update()
    {
        if (loadBar)
        {
            loadBar.fillAmount = joinLobby.loadPercen;

            if (loadBar.fillAmount >= 1)
            {
                movingCurtain.canmove = true;
                tapScreenOnLoaddingScene.enabled = true;
            }

            if (movingCurtain.finish)
            {
                loadingCanvas.SetActive(false);
            }
        }
    }
}
