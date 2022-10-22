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

    [SerializeField]
    private Camera myCamera;
   

    private Vector3 moveDir; 
    private bool isRun = false;
    private bool isMove = false; 

    private float dirX = 0;
    private float dirZ = 0;

    Vector3 setPos;
    Quaternion setRot;
    Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
    }

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
        dirX = 0; 
        dirZ = 0; 

        if (photonView.IsMine)
        {
            Vector2 StickPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            Vector3 direction = new Vector3(StickPosition.x, 0, StickPosition.y).normalized;
            direction = myCamera.transform.TransformDirection(direction);
            direction.y = 0f;
      
            transform.position += direction * applySpeed * Time.deltaTime;

        }
    }

    public void CameraRotation()
    {
        if (photonView.IsMine)
        {
            float rotationCamera = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
            cameraRig.transform.eulerAngles += new Vector3(0, rotationCamera, 0) * applySpeed * Time.deltaTime;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(myCharacter.rotation);
        }
        else if (stream.IsReading)
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
        }
    }
}