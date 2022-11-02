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
        if (other.gameObject.tag == "Item")
        {
            //playerInventory.AcquireItem(other.GetComponent<ItemPickUp>().item);
            Destroy(other.gameObject);
        }
    }
}
