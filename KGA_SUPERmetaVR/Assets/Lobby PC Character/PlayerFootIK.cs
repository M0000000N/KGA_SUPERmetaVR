using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFootIK : MonoBehaviourPun
{
    private Animator myAnimator;
    [SerializeField] private Vector3 footOffset;
    [SerializeField] [Range(0, 1)] private float rightFootPosWeight = 1;
    [SerializeField] [Range(0, 1)] private float rightFootRotWeight = 1;
    [SerializeField] [Range(0, 1)] private float leftFootPosWeight = 1;
    [SerializeField] [Range(0, 1)] private float leftFootRotWeight = 1;

    private void Start()
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        myAnimator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        Vector3 rightFootPos = myAnimator.GetIKPosition(AvatarIKGoal.RightFoot);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(rightFootPos + Vector3.up, Vector3.down, out hit);

        if (hasHit)
        {
            myAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPosWeight);
            myAnimator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + footOffset);

            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            myAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotWeight);
            myAnimator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            myAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }

        Vector3 leftFootPos = myAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);

        hasHit = Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out hit);

        if (hasHit)
        {
            myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, rightFootPosWeight);
            myAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + footOffset);

            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotWeight);
            myAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }
}