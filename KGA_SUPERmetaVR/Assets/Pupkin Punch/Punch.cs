using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    [SerializeField]
    private Rigidbody pumpkin;
    [SerializeField]
    private float attackPower;

    private void Update()
    {
        Vector3 newVector3;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            newVector3 = new Vector3(0f, 0f, 1f) * attackPower;
            pumpkin.AddForce(newVector3);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            newVector3 = new Vector3(0f, 0f, -1f) * attackPower;
            pumpkin.AddForce(newVector3);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            newVector3 = new Vector3(1f, 0f, 0f) * attackPower;
            pumpkin.AddForce(newVector3);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            newVector3 = new Vector3(-1f, 0f, 0f) * attackPower;
            pumpkin.AddForce(newVector3);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            newVector3 = new Vector3(0f, 0f, 0f);
            pumpkin.velocity = newVector3;
        }
    }
}