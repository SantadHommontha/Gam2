using UnityEngine;
using Photon.Pun;
using System.Collections;
public class BallHandle : MonoBehaviour
{
    [SerializeField] private Ball ball;
    public PhotonView photonView;
    private BallDataWapper ballDataWapper = new BallDataWapper();
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public bool isSet = true;

    private void CheckCurrentBallIndex()
    {
        if (photonView.IsMine)
        {
            ball.gameObject.SetActive(true);
            if (!isSet)
            {

             
                ball.canTrigger = false;
                float yPos = ballDataWapper.up ? -4.3f : 6.7f;
                ball.transform.position = new Vector3(ballDataWapper.xPosition, yPos, 0);
                ball.rb.linearVelocity = new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity);
                StartCoroutine(Cooldown());
                isSet = true;
            }
        }
        else
        {
            ball.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        CheckCurrentBallIndex();
    }






    public void TakeBvall(bool _up)
    {
        ball.canTrigger = false;
        BallDataWapper ballDataWapper = new BallDataWapper();
        ballDataWapper.playerSendIndex = GameManager.Instance.playerIndex;
        ballDataWapper.nextPLayerIndex = _up ? GameManager.Instance.playerIndex + 1 : GameManager.Instance.playerIndex - 1;
        ballDataWapper.up = _up;
        ballDataWapper.xPosition = ball.transform.position.x;
        ballDataWapper.yPosition = ball.transform.position.y;
        ballDataWapper.xVelocity = ball.rb.linearVelocityX;
        ballDataWapper.yVelocity = ball.rb.linearVelocityY;
      //  Debug.Log($"Velocity Send :{new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity)}");
        string ballDataJson = JsonUtility.ToJson(ballDataWapper);
      //  Debug.Log("Send Ball To " + ballDataWapper.nextPLayerIndex);
        ball.gameObject.SetActive(false);

        photonView.RPC("RPC_TakeBall", RpcTarget.Others, ballDataJson);
    }


    [PunRPC]
    private void RPC_TakeBall(string _BallDataJson)
    {

        ballDataWapper = JsonUtility.FromJson<BallDataWapper>(_BallDataJson);
        Debug.Log("Recive Ball From " + ballDataWapper.playerSendIndex);

        if (ballDataWapper.nextPLayerIndex == GameManager.Instance.playerIndex)

        {
            Debug.Log("I Am");
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            isSet = false;

            // if (photonView.IsMine)
            // {
            //     Debug.Log("Is Mine");

            //     ball.canTrigger = false;

            //     Debug.Log($"Ball Velocity {ballDataWapper.xVelocity} , {ballDataWapper.yVelocity}");
            //     ball.transform.position = new Vector3(ballDataWapper.xPosition, -4.3f, 0);
            //     ball.rb.linearVelocity = new Vector2(ballDataWapper.xVelocity, 10);
            //     StartCoroutine(Cooldown());
            // }

        }

    }

    IEnumerator Cooldown(float _time = 1)
    {
        yield return new WaitForSeconds(_time);
        ball.canTrigger = true;
    }
}
