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

    public float applySpeed;

    [SerializeField]
    private GameObject peekaboo;

    [SerializeField]
    private GameObject cameraRig;

    [SerializeField]
    private Transform camera; 

    [SerializeField]
    private Stamina stamina;

    [SerializeField]
    private Transform myCharacter;

    [SerializeField]
    private GameObject rayPointer;

    [SerializeField]
    private AppearPeekaboo appearPeekaboo;

    private Vector3 moveDir; 
    private bool isRun = false;
    private bool isMove = false; 

    //�÷��̾� �̵�
    private float dirX = 0;
    private float dirZ = 0;

    //�������� ���� �����͸� ������ ���� 
    Vector3 setPos;
    Quaternion setRot;

    //�и����� ���� 
    Rigidbody rigibody; 

    private void Start()
    {
        rigibody.velocity = Vector3.zero;
        cameraRig.SetActive(photonView.IsMine);
        applySpeed = walkSpeed;
    }

    private void Update()
    {
        TryRun();
        Move();
        CameraRotation();
    }

    public void CameraRotation()
    {
        if (isMove)
        {
            if (photonView.IsMine)
            {
                Vector2 pos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

                // ī�޶� ȸ�� 
                Vector3 lookDirection = pos.y * Vector3.forward + pos.x * Vector3.right;

                this.transform.rotation = Quaternion.LookRotation(lookDirection);
                this.transform.Translate(Vector3.forward * applySpeed * Time.deltaTime);
            }
        }
        return; 
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
                Vector3 moveDirection = (transform.forward * dirZ) + (transform.right * dirX);
                Vector3 moveDir = moveDirection * applySpeed;
                transform.Translate(moveDir * Time.deltaTime);
            }
        }
    }

    public void TryRun()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.Get(OVRInput.RawButton.B) && stamina.GetProgress() > 0 && OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
                Running();

            if (!OVRInput.Get(OVRInput.RawButton.B) && stamina.GetProgress() >= 0)
                RunningCancle();
        }
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
}