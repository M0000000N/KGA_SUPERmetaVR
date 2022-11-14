using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FD_Dragon : MonoBehaviour
{
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 40)
        {
            rigidbody.velocity = Vector3.zero;
        }
    }
}
