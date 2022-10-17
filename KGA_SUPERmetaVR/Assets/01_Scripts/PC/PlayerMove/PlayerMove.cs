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
    private float applySpeed; 

    public bool isRun = false;

    [SerializeField]
    private GameObject peekaboo;

    [SerializeField]
    private Stamina stamina;

    [SerializeField]
    private LayserPointer layser;

    [SerializeField]
    private PointerEvents pointerEvents;

    [SerializeField]
    private Transform myCharacter;

    [SerializeField]
    private GameObject NPC;

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
        peekaboo.SetActive(false);
       
    }

    private void Update()
    {
        TryRun();
        Move();
        Attack();
       
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
                curDir.x = +1;
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

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Attack();
            }
        }
    }

    public void TryRun()
    {     
            if (Input.GetKey(KeyCode.Space) && stamina.GetProgress() > 0)
            {
                Running();
            }
            if(!Input.GetKey(KeyCode.Space) && stamina.GetProgress() >=0)
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
       if(layser.CalculatedEnd() != null)
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

}
