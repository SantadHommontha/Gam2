
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    // [Header("Value")]
    // [SerializeField] private IntValue score;




    void OnTriggerEnter2D(Collider2D collision)
    {
         Debug.Log("EndPoint Touching Out");
        if (collision.TryGetComponent<Ball>(out var ball))
        {
            Debug.Log("EndPoint Touching In");
            GameManager.Instance.AddScore();
            ball.ballHandle.OnTouchEndPoint();
        }
    }





}
