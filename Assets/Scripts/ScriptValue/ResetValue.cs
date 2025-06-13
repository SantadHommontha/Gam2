using System.Collections.Generic;
using UnityEngine;

public class ResetValue : MonoBehaviour
{
    [SerializeField] private List<ScriptableValueBase> scriptableObject;
    //[SerializeField] private Reset_Value_Default defualt_value;

    void Awake()
    {
        // foreach(var T in defualt_value.scriptableValues)
        // {
        //      T.ResetValue();
        // }
        if (scriptableObject.Count > 0)
        {
            foreach (var T in scriptableObject)
            {
                T.ResetValue();
            }
        }

    }

}
