
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    // [Header("Value")]
    // [SerializeField] private IntValue score;




    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Ball>(out var ball))
        {

            GameManager.Instance.AddScore();
            ball.ballHandle.OnTouchEndPoint();
        }
    }





}
