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
                Destroy(other.gameObject);
            }
            else
            {
                //Debug.Log("else ����");
                //foreach (Transform parent in other.gameObject.transform.parent)
                //{
                //    Debug.Log("�θ� ����");
                //    Debug.Log($"�θ��̸�{parent.gameObject.name}");
                    if (other.gameObject.transform.parent.gameObject.GetComponent<Item>() != null)
                    {
                        Debug.Log("�θ�ü�� ����");
                        ItemManager.Instance.Inventory.AcquireItem(other.gameObject.transform.parent.gameObject.GetComponent<Item>(), 30);
                        Destroy(other.gameObject.transform.parent.gameObject);
                        //break;
                    }
                //}
            }
            
        }
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
