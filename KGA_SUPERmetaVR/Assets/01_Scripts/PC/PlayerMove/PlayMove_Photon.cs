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

    //�÷��̾� �̵�
    private float dirX = 0;
    private float dirZ = 0;

    // �������� ���� �����͸� ������ ���� 
    Vector3 setPos;
    Quaternion setRot;

    public int score;

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
            dirX = 0; // �¿�
            dirZ = 0; // ����

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

                // �̵����� ���� �� �̵�
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

    // ������ ����ȭ�� ���� ������ ���� �� ���� ��� 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ������ ���� ��Ȳ
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(myCharacter.rotation);
        }
        // �����͸� �����ϴ� ��Ȳ
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





