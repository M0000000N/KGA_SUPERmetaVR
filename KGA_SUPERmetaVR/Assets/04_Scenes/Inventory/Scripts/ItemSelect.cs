using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSelect : MonoBehaviour
{
    public GameObject[] model;
    private int _index;
    private Vector3 ScreenCenter;
    void Update()
    {

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            RaycastGun();
        }
    }
    private void RaycastGun()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))

        {
            if (hit.collider.gameObject.CompareTag("Item"))
            {
                Debug.Log("ÆÄ±«ÇÕ´Ï´Ù");
                Destroy(hit.collider.gameObject);
            }
        }
    }
}