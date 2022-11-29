using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] GameObject InteractionTalking;

    private void Update()
    {
        InteractionTalking.transform.forward = Camera.main.transform.forward;
    }
}
