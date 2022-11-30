using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSetter : MonoBehaviour
{
    private CharacterController myController;
    private Vector3 savePosition;

    private void Awake()
    {
        myController = GetComponent<CharacterController>();
        savePosition = new Vector3(0f, 0f, 0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            myController.enabled = !myController.enabled;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            savePosition = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = savePosition;
        }
    }
}