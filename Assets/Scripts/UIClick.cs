using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class UIClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UnityEvent unityEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        unityEvent.Invoke();
    }
}
