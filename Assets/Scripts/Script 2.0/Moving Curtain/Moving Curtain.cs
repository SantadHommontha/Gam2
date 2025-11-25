using UnityEngine;

public class MovingCurtain : MonoBehaviour
{
    public float speed = -300f;
    public float stopPoint = -1200f;
    void Update()
    {
        if (transform.position.x > stopPoint)
        {
            MoveLeft();
        }
    }
    void MoveLeft()
    {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
