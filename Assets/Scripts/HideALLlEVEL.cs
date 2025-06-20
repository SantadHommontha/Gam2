using System.Collections.Generic;
using UnityEngine;

public class HideALLlEVEL : MonoBehaviour
{
    [SerializeField] private List<GameObject> allLeavel;

    public void HideAll()
    {
        foreach (var Le in allLeavel)
            Le.SetActive(false);
    }

    public void ShowAll()
    {
        foreach (var Le in allLeavel)
            Le.SetActive(true);
    }

}
