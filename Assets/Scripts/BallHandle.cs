using UnityEngine;
using Photon.Pun;
using System.Collections;
public class BallHandle : MonoBehaviour, IPunInstantiateMagicCallback, IPunObservable
{
    [SerializeField] private Ball ball;
    public PhotonView photonView;
    public string ballID;
    private BallDataWapper ballDataWapper = new BallDataWapper();
    [SerializeField] private MyPlayerDataInfoValue myPlayerDataInfo;
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
        Debug.Log("Hide Ball");
    }
    private void ShowBall()
    {
        ball.gameObject.SetActive(true);
        Debug.Log("Show Ball");
        //  ball.TARGET.gameObject.SetActive(true);
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
                //  StartCoroutine(Cooldown());
                isSet = true;
            }
            SendPosition(ball.gameObject.transform.localPosition);
        }
        else
        {

            HideBall();
        }
    }

    private bool lerpPo;
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!ball.gameObject.activeSelf)
                ShowBall();
            // if (ball.enabled)
            //     ball.enabled = false;

            Vector3 currectVelocity = Vector3.zero;
            // ball.gameObject.transform.localPosition = Vector3.MoveTowards(ball.gameObject.transform.localPosition, latestPosition, 5f * Time.deltaTime);
            // if (!lerpPo)
            // {
            //     lerpPo = true;
            //     StartCoroutine(LerpPosition(ball.gameObject.transform, ball.gameObject.transform.localPosition, latestPosition));
            // }
            ball.gameObject.transform.localPosition = latestPosition;
            //  ball.gameObject.transform.localRotation = Quaternion.s
            // ball.gameObject.transform.position = Vector3.Lerp(ball.gameObject.transform.position, latestPosition, 0.4f);
            //   ball.gameObject.transform.rotation = Quaternion.Lerp(ball.gameObject.transform.rotation, latestRotation, Time.deltaTime * 5f);

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

        }
        else
        {
            ball.Ball_FixedUpdate();
            ball.BallAnimation();
        }
    }

    private IEnumerator LerpPosition(Transform obj, Vector3 start, Vector3 end, float duration = 0.1f)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;   
            obj.localPosition = Vector3.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

      
        obj.localPosition = end;
        lerpPo = false;
    }

    public void AddForce(Vector2 _direction, float _force)
    {
        ball.AddForce(_direction, _force);
    }
    // public void AF(Vector2 _direction, float _force)
    // {
    //     float[] f = { _direction.x, _direction.y, _force };
    //     photonView.RPC("RPC_AF", RpcTarget.MasterClient, f);
    // }
    // [PunRPC]
    // private void RPC_AF(float[] _f)
    // {
       
    //     Vector2 direction = new Vector2(_f[0], _f[1]);
    //     float force = _f[2];
    //     AddForce(direction, force);

    // }
    public void GotShoot()
    {
        // gotShoot = true;
        GameManager.Instance.SetCurrentBallIndex(myPlayerDataInfo.Value.playerIndex);
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
        ballDataWapper.playerSendIndex = myPlayerDataInfo.Value.playerIndex;
        ballDataWapper.nextPLayerIndex = _up ? myPlayerDataInfo.Value.playerIndex + 1 : myPlayerDataInfo.Value.playerIndex - 1;
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
        Debug.Log("Recive Ball From " + ballDataWapper.playerSendIndex);

        if (ballDataWapper.nextPLayerIndex == myPlayerDataInfo.Value.playerIndex)

        {
            //    Debug.Log("I Am");
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            isSet = false;
            GameManager.Instance.SetCurrentBallIndex(myPlayerDataInfo.Value.playerIndex);
            photonView.RPC("RPC_ReciveTakeBall", _info.Sender);
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

    private void SendPosition(Vector3 _localPosition)
    {
        float[] position = { _localPosition.x, _localPosition.y, _localPosition.z };
        photonView.RPC("RPC_SendPosition", RpcTarget.MasterClient, position);
    }
    [PunRPC]
    private void RPC_SendPosition(float[] _position)
    {
        latestPosition = new Vector3(_position[0], _position[1], _position[2]);
    }
}
