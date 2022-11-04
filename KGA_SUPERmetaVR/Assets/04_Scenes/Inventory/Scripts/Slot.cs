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

    //public void AddItem(Item _item, int _count)
    //{
    //    if (StaticData.GetItemSheet(_item.ItemID).Type != "EQUIPMENT")
    //    {
    //        countImage.SetActive(true);
    //        countText.text = itemCount.ToString();
    //    }
    //    else
    //    {
    //        countText.text = "0";
    //        countImage.SetActive(false);
    //    }
    //    SetColor(1);
    //}

    //public void SetSlotCount(int _count)
    //{
    //    itemCount += _count;
    //    countText.text = itemCount.ToString();

    //    if (itemCount <= 0)
    //    {
    //        ClearSlot();
    //    }
    //}

    //private void ClearSlot()
    //{
    //    item = null;
    //    itemCount = 0;
    //    itemImage.sprite = null;
    //    SetColor(0);

    //    countText.text = "0";
    //    countImage.SetActive(false);
    //}
}
