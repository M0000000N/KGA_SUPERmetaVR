using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    private MeshCollider meshCollider;

    [SerializeField]
    private Inventory playerInventory;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GrabItem")
        {
            playerInventory.AcquireItem(other.GetComponent<Item>() , 50);
            Destroy(other.gameObject);
        }
    }
}
