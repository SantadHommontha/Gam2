using UnityEngine;


public class ShowUi : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private bool showIfPlayer;
    [SerializeField] private bool showIfAdmin;
    [Header("Value")]
    [SerializeField] private GameDataValue gameData;


    void Awake()
    {
        if (!gameObj) gameObj = this.gameObject;
    }

    public void Set()
    {
        gameObj.SetActive(false);
        if (showIfAdmin && gameData.Value.isAdmin)
        {
            gameObj.SetActive(true);

        }
        if (showIfPlayer && gameData.Value.isPlayer)
        {
            gameObj.SetActive(true);

        }

        if (!showIfAdmin && !showIfPlayer)
            gameObj.SetActive(false);

    }

}
