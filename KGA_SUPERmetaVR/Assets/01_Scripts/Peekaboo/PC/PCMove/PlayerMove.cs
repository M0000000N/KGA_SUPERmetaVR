using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using OVR;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPun, IPunObservable
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
    private GameObject cameraRig;

    [SerializeField]
    private Stamina stamina; 

    //플레이어 이동
    private float dirX = 0;
    private float dirZ = 0;

    private Vector3 curDir;

    private NavMeshAgent playerAgent;
    // 서버에서 받은 데이터를 저장할 변수 
    Vector3 setPos;
    Quaternion setRot;

    private void Start()
    {
        cameraRig.SetActive(photonView.IsMine);
        curDir = Vector3.zero;
        applySpeed = walkSpeed;
        playerAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //if (PeekabooGameManager.Instance.IsGameOver == false)
        //{
            Move();
            TryRun();
//        }
    }

    // 플레이어 이동

    private void Move()
    {
        if (!photonView.IsMine)
            return;

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
            playerAgent.SetDestination(transform.position);
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

}
