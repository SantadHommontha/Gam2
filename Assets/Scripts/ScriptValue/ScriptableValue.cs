using UnityEngine;
using System;

[System.Serializable]
public  class ScriptableValue<T> : ScriptableValueBase
{
#if UNITY_EDITOR //เอาไว้อธิบายว่าเก็บค่าอะไร 
    [SerializeField][TextArea] private string description;
#endif
    [SerializeField] protected T value;
    public Action<T> OnValueChange;

    public T Value
    {
        get { return value; }
        set
        {
            this.value = value;
            OnValueChange?.Invoke(value);
        }
    }

    public virtual void SetValue(T _value)
    {
        value = _value;
    }
    public void ClearAction()
    {
        OnValueChange = null;
    }
    

}
