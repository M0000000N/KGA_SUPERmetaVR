using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimationController : MonoBehaviourPun
{
    [SerializeField] private float speedTreshold = 0f;
    [SerializeField] [Range(0, 1)] private float smoothing = 1f;
    private Animator myAnimator;
    private Vector3 previousPos;
    private PlayerRigging vrRig;

    private void Start()
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        myAnimator = GetComponent<Animator>();
        vrRig = GetComponent<PlayerRigging>();
        previousPos = vrRig.Head.VRTarget.position;
    }

    private void Update()
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        Vector3 headSetSpeed = (vrRig.Head.VRTarget.position - previousPos) / Time.deltaTime;
        headSetSpeed.y = 0f;

        Vector3 headSetLocalSpeed = transform.InverseTransformDirection(headSetSpeed);
        previousPos = vrRig.Head.VRTarget.position;

        float previousDirectionX = myAnimator.GetFloat("DirectionX");
        float previousDirectionY = myAnimator.GetFloat("DirectionY");

        myAnimator.SetBool("IsMoving", headSetLocalSpeed.magnitude > speedTreshold);
        myAnimator.SetFloat("DirectionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headSetLocalSpeed.x, -1f, 1f), smoothing));
        myAnimator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headSetLocalSpeed.z, -1f, 1f), smoothing));
    }
}