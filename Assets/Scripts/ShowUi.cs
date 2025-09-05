using UnityEngine;


public class ShowUi : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private bool showIfPlayer;
    [SerializeField] private bool showIfAdmin;
    [Header("Value")]
    [SerializeField] private GameInfoValue gameInfo;


    void Awake()
    {
        if (!gameObj) gameObj = this.gameObject;
    }

    public void Set()
    {
        gameObj.SetActive(false);
        if (showIfAdmin && gameInfo.Value.isAdmin)
        {
            gameObj.SetActive(true);

        }
        if (showIfPlayer && gameInfo.Value.isPlayer)
        {
            gameObj.SetActive(true);

        }

        if (!showIfAdmin && !showIfPlayer)
            gameObj.SetActive(false);

    }

}
