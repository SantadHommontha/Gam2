using UnityEngine;
using Photon.Pun;
using System.Collections;
public class BallHandle : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField] private Ball ball;
    public PhotonView photonView;
    public string ballID;
    private BallDataWapper ballDataWapper = new BallDataWapper();
    [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void HideBall()
    {
        ball.gameObject.SetActive(false);
        ball.TARGET.gameObject.SetActive(false);
    }
    private void ShowBall()
    {
        ball.gameObject.SetActive(true);
        ball.TARGET.gameObject.SetActive(true);
    }

    public bool isSet = true;

    private void CheckCurrentBallIndex()
    {
        if (photonView.IsMine)
        {

            ShowBall();
            if (!isSet)
            {


                ball.canTrigger = true;
                float yPos = ballDataWapper.up ? -4.3f : 6.7f;
                ball.transform.position = new Vector3(ballDataWapper.xPosition, yPos, 0);
                ball.rb.linearVelocity = new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity);
                //  StartCoroutine(Cooldown());
                isSet = true;
            }
        }
        else
        {

            HideBall();
        }
    }


    void Update()
    {
        CheckCurrentBallIndex();
    }


    public void AddForce(Vector2 _direction, float _force)
    {
        ball.AddForce(_direction, _force);
    }



    public void TakeBvall(bool _up)
    {
        ball.canTrigger = false;
        BallDataWapper ballDataWapper = new BallDataWapper();
        ballDataWapper.playerSendIndex = myPlayerDataInfo.Value.playerIndex;
        ballDataWapper.nextPLayerIndex = _up ? myPlayerDataInfo.Value.playerIndex + 1 : myPlayerDataInfo.Value.playerIndex - 1;
        ballDataWapper.up = _up;
        ballDataWapper.xPosition = ball.transform.position.x;
        ballDataWapper.yPosition = ball.transform.position.y;
        ballDataWapper.xVelocity = ball.rb.linearVelocityX;
        ballDataWapper.yVelocity = ball.rb.linearVelocityY;
        //  Debug.Log($"Velocity Send :{new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity)}");
        string ballDataJson = JsonUtility.ToJson(ballDataWapper);
        //  Debug.Log("Send Ball To " + ballDataWapper.nextPLayerIndex);

        HideBall();
        photonView.RPC("RPC_TakeBall", RpcTarget.Others, ballDataJson);
    }


    [PunRPC]
    private void RPC_TakeBall(string _BallDataJson)
    {

        ballDataWapper = JsonUtility.FromJson<BallDataWapper>(_BallDataJson);
        Debug.Log("Recive Ball From " + ballDataWapper.playerSendIndex);

        if (ballDataWapper.nextPLayerIndex == myPlayerDataInfo.Value.playerIndex)

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

    public void OnTouchEndPoint()
    {
        //  HideBall();
        PhotonNetwork.Destroy(this.gameObject);
    }

     public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.InstantiationData != null && info.photonView.InstantiationData.Length > 0)
        {
            string id = (string)info.photonView.InstantiationData[0];
            ballID = id;
        }
    }
}
