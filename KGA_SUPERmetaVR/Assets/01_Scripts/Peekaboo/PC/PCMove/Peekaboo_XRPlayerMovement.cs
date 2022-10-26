using Photon.Pun; 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.AI;

public class Peekaboo_XRPlayerMovement : MonoBehaviourPun
{
    [Header("PC Speed")]

    [SerializeField]
    private float walkSpeed = 1f;

    [SerializeField]
    private float runSpeed = 2f;

    [SerializeField]
    private Stamina stamina;

    [Header("XR")]

    [SerializeField]
    private XRNode controllerNode = XRNode.LeftHand;

    private float applySpeed;
    private bool isRun = false;
    private bool isPressedButton = false; 
    private NavMeshAgent navMeshAgent;
    private InputDevice controller;
 
    private List<InputDevice> devices = new List<InputDevice>();

    Vector3 setPos;
    Quaternion setRot;

    void Start()
    {
        if (photonView == false) return;

        GetDevice();
        applySpeed = walkSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
        controller = devices.FirstOrDefault();
    }

    void Update()
    {
        if (photonView == false) return;

        if (controller == null)
        {
            GetDevice();
        }

        Move();
    }

    private void Move()
    {

        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

        if (controller.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero)
        {
            Debug.Log("primary2DAxisClick is pressed " + primary2dValue);

            var xAxis = primary2dValue.x * applySpeed * Time.deltaTime;
            var zAxis = primary2dValue.y * applySpeed * Time.deltaTime;

            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            transform.position -= right * xAxis;
            transform.position -= forward * zAxis;
            navMeshAgent.SetDestination(transform.position);
        }
    }

    public void TryRun()
    {
        bool pressbutton = false;
        InputFeatureUsage<bool> secondaryBbutoon = CommonUsages.secondaryButton;

        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

            if (controller.TryGetFeatureValue(secondaryBbutoon, out pressbutton) == true && controller.TryGetFeatureValue(primary2DVector, out primary2dValue))
            {
                Running();
                Debug.Log(pressbutton);
                pressbutton = false;
            }
       
        else 
        {
            if(controller.TryGetFeatureValue(secondaryBbutoon, out pressbutton) == false)
            {
                RunningCancle();
                pressbutton = true;
            }
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



