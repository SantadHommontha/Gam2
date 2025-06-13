using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects;
    public void HideAllPLatform()
    {
        foreach (var gb in gameObjects)
        {
            gb.SetActive(false);
        }
   }
}
