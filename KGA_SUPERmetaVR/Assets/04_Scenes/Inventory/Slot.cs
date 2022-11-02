using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SlotData
{
    public int ItemID;

    public int ItemCount;
}

public class Slot : MonoBehaviour
{
  
    [SerializeField]
    private Image itemImage;
    public Image ItemImage {get { return itemImage; } set { itemImage = value; } }

    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private GameObject countImage;

    
    // 슬롯에 아이템의 유무에 따라 투명도 조절
    private void SetColor (float _alpha)
    {
        Color itemcolor = itemImage.color;
        itemcolor.a = _alpha;
        itemImage.color = itemcolor;
    }

    //public void AddItem(Item _item, int _count = 1)
    //{
    //    item = _item;
    //    itemCount = _count;
    //    itemImage.sprite = item.ItemImage;

    //    if (item.ItemType != Item.ITEMTYPE.EQUIPMENT)
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
