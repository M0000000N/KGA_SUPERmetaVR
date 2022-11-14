using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ItemManager.Instance.Inventory.AcquireItem(other.GetComponent<Item>(), 50);
            Destroy(other.gameObject);
        }
    }
}
