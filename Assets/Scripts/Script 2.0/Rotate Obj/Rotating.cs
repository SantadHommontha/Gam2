using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float rotateSpeed = 100f;
    private void Update()
    {
        RotateObject();
    }
    void RotateObject() //หมุน sprite 
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
