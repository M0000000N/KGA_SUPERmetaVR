using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerfulNPC : MonoBehaviour
{
    public float animSpeed = 3.0f;
    public Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.speed = animSpeed;
    }
}
