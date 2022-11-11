using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class VRMap
{
    public Transform VRTarget { get { return vrTarget; } }

    [SerializeField] private Transform vrTarget;
    [SerializeField] private Transform rigTarget;
    [SerializeField] private Vector3 trackingPositionOffset;
    [SerializeField] private Vector3 trackingRotationOffset;

    public void SetMyVRTarget(GameObject _gameObject)
    {
        vrTarget = _gameObject.transform;
    }

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class PlayerRigging : MonoBehaviourPun
{
    public VRMap Head { get { return head; } }

    [SerializeField] private VRMap head;
    [SerializeField] private VRMap leftHand;
    [SerializeField] private VRMap rightHand;
    [SerializeField] private Transform headConstraint;
    [SerializeField] private Vector3 headBodyOffset;

    private GameObject myXROrigin;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            myXROrigin = GameObject.Find("Lobby Camera");

            head.SetMyVRTarget(myXROrigin.transform.GetChild(0).Find("Main Camera").gameObject);
            leftHand.SetMyVRTarget(myXROrigin.transform.GetChild(0).Find("Left Ray Interactor").gameObject);
            rightHand.SetMyVRTarget(myXROrigin.transform.GetChild(0).Find("Right Ray Interactor").gameObject);
        }
    }

    private void Start()
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        headBodyOffset = transform.position - headConstraint.position;
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized;

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}