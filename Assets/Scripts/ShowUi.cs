using UnityEngine;

public class ShowUi : MonoBehaviour
{
    [SerializeField] private GameObject opneMenu;
    [SerializeField] private GameObject openControl;
    [Header("Value")]
    [SerializeField] private BoolValue isPlayer;
    [SerializeField] private BoolValue isAdmin;
    [SerializeField] private GameDataValue gameData;


    public void SetUp()
    {
      
        opneMenu.gameObject.SetActive(gameData.Value.isPlayer);
        openControl.gameObject.SetActive(gameData.Value.isAdmin);

    }
}
