using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TapScreen : MonoBehaviour,IPointerDownHandler
{
    public UnityEvent unityEvent;
  //  public JoinLobby joinLobby;
    public void OnPointerDown(PointerEventData eventData)
    {
      
      unityEvent?.Invoke();
      // joinLobby.TapScreen();
    }

    
}
