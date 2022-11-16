using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("GrabItem"))
        {
            if (other.gameObject.GetComponent<Item>() != null)
            {
                ItemManager.Instance.Inventory.AcquireItem(other.gameObject.GetComponent<Item>(), 50);
                if (other.gameObject.CompareTag("FourLeafClover"))
                {
                    CloverSetActiveFalse(other.gameObject);
                }
                else
                {
                    Destroy(other.gameObject);
                }
            }
            else
            {

                if (other.gameObject.transform.parent.gameObject.GetComponent<Item>() != null)
                {
                    Debug.Log("ºÎ¸ð°´Ã¼¿¡ µé¾î¿È");
                    ItemManager.Instance.Inventory.AcquireItem(other.gameObject.transform.parent.gameObject.GetComponent<Item>(), 30);
                    if (other.gameObject.CompareTag("FourLeafClover"))
                    {
                        CloverSetActiveFalse(other.gameObject.transform.parent.gameObject);
                    }
                    else
                    {
                        Destroy(other.gameObject.transform.parent.gameObject);
                    }
                }

            }

        }
    }

    private void CloverSetActiveFalse(GameObject _clover)
    {
        _clover.SetActive(false);
        CloverSpawnManager.Instance.CheckFourLeafCloverActiveSelf();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("GrabItem"))
    //    {
    //        ItemManager.Instance.Inventory.AcquireItem(collision.gameObject.GetComponent<Item>(), 50);
    //        Destroy(collision.gameObject);
    //    }
    //}
}
