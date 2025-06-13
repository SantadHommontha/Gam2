using UnityEngine;
using Photon.Pun;
using System.Collections;
public class BallHandle : MonoBehaviour
{
    [SerializeField] private Ball ball;
    public PhotonView photonView;


    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }



    private void CheckCurrentBallIndex()
    {
        if (photonView.IsMine)
        {
            ball.gameObject.SetActive(true);
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
        BallDataWapper ballDataWapper = new BallDataWapper();
        ballDataWapper.playerSendIndex = GameManager.Instance.playerIndex;
        ballDataWapper.nextPLayerIndex = _up ? GameManager.Instance.playerIndex + 1 : GameManager.Instance.playerIndex - 1;
        ballDataWapper.xPosition = transform.position.x;
        ballDataWapper.yPosition = transform.position.y;
        ballDataWapper.xVelocity = ball.rb.linearVelocityX;
        ballDataWapper.yVelocity = ball.rb.linearVelocityY;

        string ballDataJson = JsonUtility.ToJson(ballDataWapper);
        Debug.Log("Send Ball To " + ballDataWapper.nextPLayerIndex);
        gameObject.SetActive(false);

        photonView.RPC("RPC_TakeBall", RpcTarget.Others, ballDataJson);
    }


    [PunRPC]
    private void RPC_TakeBall(string _BallDataJson)
    {

        BallDataWapper ballDataWapper = JsonUtility.FromJson<BallDataWapper>(_BallDataJson);
        Debug.Log("Recive Ball From " + ballDataWapper.playerSendIndex);
        if (ballDataWapper.nextPLayerIndex == GameManager.Instance.playerIndex)
        {
            Debug.Log("Is Mine");
            ball.canTrigger = false;
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            ball.transform.position = new Vector3(ballDataWapper.xPosition, -4.3f, 0);
            ball.rb.linearVelocity = new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity);
            StartCoroutine(Cooldown());
        }

    }

    IEnumerator Cooldown(float _time = 1)
    {
        yield return new WaitForSeconds(_time);
        ball.canTrigger = true;
    }
}
