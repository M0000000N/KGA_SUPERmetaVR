using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Item"))
    //    {
    //        ItemManager.Instance.Inventory.AcquireItem(other.GetComponent<Item>(), 50);
    //        Destroy(other.gameObject);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            ItemManager.Instance.Inventory.AcquireItem(collision.gameObject.GetComponent<Item>(), 50);
            Destroy(collision.gameObject);
        }
    }

    
}
