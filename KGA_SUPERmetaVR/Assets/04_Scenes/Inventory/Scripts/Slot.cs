using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }

    [SerializeField]
    private TextMeshProUGUI countText;

    [SerializeField]
    private GameObject countImage;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for (int childCount = 1; childCount < transform.childCount; childCount++)
        {
            Destroy(transform.GetChild(childCount).gameObject);
        }

        itemPrefab = null;
        countText.text = "";
        countImage.SetActive(false);
    }

    public void SetItemCount(int _count)
    {
        if(_count > 1)
        {
            countText.text = _count.ToString();
            countImage.SetActive(true);
        }
        else
        {
            countImage.SetActive(false);
        }
    }

    public void AddItem(Item _item, int _count)
    {
        
        if (StaticData.GetItemSheet(_item.ItemID).Type != "EQUIPMENT")
        {
            countImage.SetActive(true);
            countText.text = _count.ToString();
        }
        else
        {
            countText.text = _count.ToString();
            countImage.SetActive(false);
        }
        
    }

    public void SetSlotCount(int _count)
    {
        countText.text = _count.ToString();

        if (_count <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        itemPrefab = null;
        countText.text = "0";
        countImage.SetActive(false);
    }
}
