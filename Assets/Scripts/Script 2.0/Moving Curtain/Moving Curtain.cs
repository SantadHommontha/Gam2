using UnityEngine;

public class MovingCurtain : MonoBehaviour
{
    public float speed = -300f;
    public float stopPoint = -1200f;
    public bool finish = false;
    public bool canmove = false;
    void Start()
    {
        
        finish = false;
    }
    void Update()
    {
        if(!canmove) return;
        if (transform.position.x > stopPoint)
        {
            MoveLeft();
        }
        else
        {
            finish = true;
        }
    }
    void MoveLeft()
    {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
    }


}
