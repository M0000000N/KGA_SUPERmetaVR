using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    [SerializeField]
    private Inventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            playerInventory.AcquireItem(other.GetComponent<Item>() , 50);
            Destroy(other.gameObject);
        }
    }
}
