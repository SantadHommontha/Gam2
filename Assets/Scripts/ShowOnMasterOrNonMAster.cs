using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShowOnMasterOrNonMAster : MonoBehaviour
{
    [SerializeField] private List<GameObject> masterShow;
    [SerializeField] private List<GameObject> nonMasterShow;
  

    public void Fetch()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var T in masterShow)
            {
                T.gameObject.SetActive(true);
            }
            foreach (var T in nonMasterShow)
            {
                T.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var T in masterShow)
            {
                T.gameObject.SetActive(false);
            }
            foreach (var T in nonMasterShow)
            {
                T.gameObject.SetActive(true);
            }
        }
    }
}
