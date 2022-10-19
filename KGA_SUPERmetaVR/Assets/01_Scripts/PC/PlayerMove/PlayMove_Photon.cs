using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI; 

public class PlayMove_Photon : MonoBehaviourPun, IPunObservable
{

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    [SerializeField]
    private GameObject peekaboo;

    [SerializeField]
    private GameObject cameraRig;

    [SerializeField]
    private Stamina stamina;

    [SerializeField]
    private PointerEvents pointerEvents; 

    [SerializeField]
    private Transform myCharacter;

    [SerializeField]
    private LayserPointer layser;

    [SerializeField]
    private GameObject rayPointer; 

    private bool isRun = false;

    //플레이어 이동
    private float dirX = 0;
    private float dirZ = 0;

    // 서버에서 받은 데이터를 저장할 변수 
    Vector3 setPos;
    Quaternion setRot;

    private void Start()
    {
        cameraRig.SetActive(photonView.IsMine);
        applySpeed = walkSpeed;
    }

    private void Update()
    {
        TryRun();
        Move();
    }
    public void Move()
    {
        if (!photonView.IsMine)
            return;

        if (photonView.IsMine)
        {
            dirX = 0; // 좌우
            dirZ = 0; // 상하

            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
            {
                Vector2 pos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

                var absX = Mathf.Abs(pos.x);
                var absY = Mathf.Abs(pos.y);

                if (absX > absY)
                {
                    //right
                    if (pos.x > 0)
                        dirX = +1;
                    //left
                    else
                        dirX = -1;
                }
                else
                {
                    //up
                    if (pos.y > 0)
                        dirZ = +1;
                    //down
                    else
                        dirZ = -1;
                }

                // 이동방향 설정 후 이동
                Vector3 moveDir = new Vector3(dirX * applySpeed, 0, dirZ * applySpeed);
                transform.Translate(moveDir * Time.deltaTime);
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) && (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)))
                {
                    Attack();
                }
            }
        }         
    }

    public void TryRun()
    {
        if (OVRInput.Get(OVRInput.RawButton.B) && stamina.GetProgress() > 0) 
        Running();
      
        if(!OVRInput.Get(OVRInput.RawButton.B) && stamina.GetProgress() >= 0)
        RunningCancle();
    }
    public void Running()
    {
        isRun = true;
        stamina.DecreaseProgress();
        applySpeed = runSpeed;
    }

    public void RunningCancle()
    {
        isRun = false;
        stamina.IncreaseProgress();
        applySpeed = walkSpeed;
    }

    public void Attack()
    {
        if (layser.CalculatedEnd() != null)
        {
            pointerEvents.CallPeekaboo();
        }

    }

    // 데이터 동기화를 위한 데이터 전송 및 수신 기능 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 전송 상황
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(myCharacter.rotation);
        }
        // 데이터를 수신하는 상황
        else if (stream.IsReading)
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public void CullingRay()
    {
        if(!photonView.IsMine)
        {
            rayPointer.SetActive(false);    
        }
    }



}





