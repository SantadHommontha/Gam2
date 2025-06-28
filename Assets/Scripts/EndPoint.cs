
using UnityEngine;

public class EndPoint : MonoBehaviour
{
     [Header("Value")]
     [SerializeField] private GameDataValue gameData;




    void OnTriggerEnter2D(Collider2D collision)
    {
        //   Debug.Log("EndPoint Touching Out");
        if (!gameData.Value.gamestart) return;
        if (collision.TryGetComponent<Ball>(out var ball))
        {
            //    Debug.Log("EndPoint Touching In");
            GameManager.Instance.AddScore();
            ball.ballHandle.OnTouchEndPoint();
        }
    }





}
