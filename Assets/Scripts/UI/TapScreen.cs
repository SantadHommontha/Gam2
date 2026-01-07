using UnityEngine;
using UnityEngine.EventSystems;

public class TapScreen : MonoBehaviour,IPointerDownHandler
{
    public JoinLobby joinLobby;
    public void OnPointerDown(PointerEventData eventData)
    {
       joinLobby.TapScreen();
    }

    
}
