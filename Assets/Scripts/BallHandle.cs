using UnityEngine;
using Photon.Pun;
using System.Collections;
public class BallHandle : MonoBehaviour, IPunInstantiateMagicCallback, IPunObservable
{
    [SerializeField] private Ball ball;
    public PhotonView photonView;
    public string ballID;
    private BallDataWapper ballDataWapper = new BallDataWapper();
    //  [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;
    [SerializeField] private GameInfoValue gameInfo;
    private Vector3 latestPosition;
    private Quaternion latestRotation;
    private bool gotShoot = true;
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        latestPosition = ball.gameObject.transform.position;
        latestRotation = ball.gameObject.transform.rotation;


        if (PhotonNetwork.IsMasterClient)
        {
            ball.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            ball.rb.simulated = false;
        }
    }
    private void HideBall()
    {
        ball.gameObject.SetActive(false);
        ball.TARGET.gameObject.SetActive(false);
    }
    private void ShowBall()
    {
        ball.gameObject.SetActive(true);

    }

    public bool isSet = true;

    private void CheckCurrentBallIndex()
    {
        //  if (!gotShoot) return;
        if (photonView.IsMine)
        {
            ShowBall();
            if (!isSet)
            {
                ball.canTrigger = true;
                float yPos = ballDataWapper.up ? -4.3f : 6.7f;
                ball.transform.position = new Vector3(ballDataWapper.xPosition, yPos, 0);
                ball.rb.linearVelocity = new Vector2(ballDataWapper.xVelocity, ballDataWapper.yVelocity);

                isSet = true;
            }
            SendPositionToMaster(ball.gameObject.transform.localPosition);
        }
        else
        {

            HideBall();
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!ball.gameObject.activeSelf)
                ShowBall();
            Vector3 currectVelocity = Vector3.zero;
            ball.gameObject.transform.localPosition = latestPosition;
        }
        else
        {

            CheckCurrentBallIndex();
            ball.Ball_Update();
            ball.BallAnimation();
        }
    }
    void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ball.BallAnimation();
        }
        else
        {
            ball.Ball_FixedUpdate();
            ball.BallAnimation();
        }
    }

    public void AddForce(Vector2 _direction, float _force) => ball.AddForce(_direction, _force);


    public void GotShoot()
    {
      
        GameManager.Instance.SetCurrentBallIndex(gameInfo.Value.myPlayerIndex);
        ShowBall();

        StartCoroutine(GotshootCooldown());
    }

    private IEnumerator GotshootCooldown()
    {
        yield return new WaitForSeconds(1);
        gotShoot = false;
    }

    public void TakeBvall(bool _up)
    {
        ball.canTrigger = false;
        BallDataWapper ballDataWapper = new BallDataWapper();
        ballDataWapper.playerSendIndex = gameInfo.Value.myPlayerIndex;
        ballDataWapper.nextPLayerIndex = _up ? gameInfo.Value.myPlayerIndex + 1 : gameInfo.Value.myPlayerIndex - 1;
        ballDataWapper.up = _up;
        ballDataWapper.xPosition = ball.transform.position.x;
        ballDataWapper.yPosition = ball.transform.position.y;
        ballDataWapper.xVelocity = ball.rb.linearVelocityX;
        ballDataWapper.yVelocity = ball.rb.linearVelocityY;

        string ballDataJson = JsonUtility.ToJson(ballDataWapper);

        Debug.Log($"send ball to: {ballDataWapper.nextPLayerIndex}");
        photonView.RPC("RPC_TakeBall", RpcTarget.Others, ballDataJson);

    }

    public void TranferOwner(Photon.Realtime.Player _player)
    {
        photonView.TransferOwnership(_player);
    }
    public void TranferOwner()
    {
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
    }
    [PunRPC]
    private void RPC_TakeBall(string _BallDataJson, PhotonMessageInfo _info)
    {

        ballDataWapper = JsonUtility.FromJson<BallDataWapper>(_BallDataJson);
        // Debug.Log("Recive Ball From " + ballDataWapper.playerSendIndex);

        if (ballDataWapper.nextPLayerIndex == gameInfo.Value.myPlayerIndex)

        {
        
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            isSet = false;
            GameManager.Instance.SetCurrentBallIndex(gameInfo.Value.myPlayerIndex);
            photonView.RPC("RPC_ReciveTakeBall", _info.Sender);
        }

    }
    [PunRPC]
    private void RPC_ReciveTakeBall()
    {
        HideBall();
    }
    IEnumerator Cooldown(float _time = 1)
    {
        yield return new WaitForSeconds(_time);
        ball.canTrigger = true;
    }

    public void OnTouchEndPoint()
    {
        HideBall();
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //    stream.SendNext(ball.gameObject.transform.localPosition);
            //    stream.SendNext(ball.gameObject.transform.localRotation);
        }
        else
        {
            //     latestPosition = (Vector3)stream.ReceiveNext();
            //     latestRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SendPositionToMaster(Vector3 _localPosition)
    {
        float[] position = { _localPosition.x, _localPosition.y, _localPosition.z };
        photonView.RPC("RPC_ReceivePositionFormOther", RpcTarget.MasterClient, position);
    }
    [PunRPC]
    private void RPC_ReceivePositionFormOther(float[] _position)
    {
        latestPosition = new Vector3(_position[0], _position[1], _position[2]);
    }
}
