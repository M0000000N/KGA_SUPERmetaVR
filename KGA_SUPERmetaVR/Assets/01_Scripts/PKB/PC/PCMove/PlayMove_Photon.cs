 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.AI; 

public class PlayMove_Photon : MonoBehaviourPun, IPunObservable
{

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private GameObject cameraRig;

    [SerializeField]
    private Stamina stamina;

    [SerializeField]
    private Camera myCamera;

    private NavMeshAgent navMeshAgent; 
    private float applySpeed;
    private bool isRun = false;

    Vector3 setPos;
    Quaternion setRot;
  
    private void Start()
    {
        if (photonView.IsMine == false) return;

        GameObject camera = PeekabooGameManager.Instance.OVRCamera;

        cameraRig = camera.transform.GetChild(0).gameObject;
       // stamina = camera.GetComponentInChildren<Stamina>();

        applySpeed = walkSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>(); 
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;

        if (PeekabooGameManager.Instance.IsGameOver == false)
        {
            TryRun();
            Move();
        }
    }

    public void Move()
    {
            Vector2 StickPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            Vector3 direction = new Vector3(StickPosition.x, 0, StickPosition.y).normalized;
            direction = cameraRig.transform.TransformDirection(direction);
            direction.y = 0f;
      
            transform.position += direction * applySpeed * Time.deltaTime;
            navMeshAgent.SetDestination(transform.position); 
    }

    public void CameraRotation()
    {
            float rotationCamera = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
            cameraRig.transform.eulerAngles += new Vector3(0, rotationCamera, 0) * applySpeed * Time.deltaTime;
    }

    public void TryRun()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.Get(OVRInput.RawButton.B) && stamina.GetProgress() > 0 && OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
                Running();

            if (!OVRInput.Get(OVRInput.RawButton.B) && stamina.GetProgress() > 0)
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
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
        }
    }
}