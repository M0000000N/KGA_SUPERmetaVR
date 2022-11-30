using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSetter : MonoBehaviour
{
    private CapsuleCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            myCollider.isTrigger = !myCollider.isTrigger;
        }
    }
}