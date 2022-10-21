using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using OVR; 

public class PlayerMove : MonoBehaviourPun
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private Transform myCharacter;

    private float applySpeed; 

    public bool isRun = false;

    [SerializeField]
    private GameObject peekaboo;

    [SerializeField]
    private Stamina stamina; 

    //�÷��̾� �̵�
    private float dirX = 0;
    private float dirZ = 0;

    private Vector3 curDir;

    // �������� ���� �����͸� ������ ���� 
    Vector3 setPos;
    Quaternion setRot;

    private void Start()
    {
        curDir = Vector3.zero;
        applySpeed = walkSpeed;     
    }

    private void Update()
    {
        TryRun();
        Move();
    }

    // �÷��̾� �̵�

    private void Move()
    {
        if (photonView.IsMine)
        {
            curDir = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                curDir.x = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                curDir.x = 1;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                curDir.z = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                curDir.z = -1;
            }

            curDir.Normalize();
            transform.position += curDir * (applySpeed * Time.deltaTime);
        }
    }

    public void TryRun()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Running();
            }
            RunningCancle();
        }
    }

    public void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
        stamina.DecreaseProgress(); 
    }

    public void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
        stamina.IncreaseProgress();
    }

    public void Attack()
    {
        peekaboo.SetActive(true);
    }

    // ������ ����ȭ�� ���� ������ ���� �� ���� ��� 
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // ������ ���� ��Ȳ
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(transform.position);
    //        stream.SendNext(myCharacter.rotation);
    //    }
    //    // �����͸� �����ϴ� ��Ȳ
    //    else if (stream.IsReading)
    //    {
    //        setPos = (Vector3)stream.ReceiveNext();
    //        setRot = (Quaternion)stream.ReceiveNext();
    //    }
    //}

}
