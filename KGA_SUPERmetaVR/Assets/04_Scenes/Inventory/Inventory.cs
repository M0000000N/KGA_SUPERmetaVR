using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotData111
{
    private int itemID;
    public int ItemID { get { return itemID; } set { itemID = value; } }

    private int itemCount;
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
}

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject SlotGrid;

    private Slot[] slots;

    private Dictionary<int,SlotData> slotDB;

    private void Start()
    {
        slots = SlotGrid.GetComponentsInChildren<Slot>();
        slotDB = new Dictionary<int, SlotData>();
        
    }

    //private void SlotDB()
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        SlotData slotData = new SlotData();
    //        slotData.ItemID = slots[i].Item.ItemID;
    //        slotData.ItemCount = slots[i].ItemCount;
    //        slotDB.Add(i, slotData);
    //    }
    //}

    //public void AcquireItem(Item _item, int _count = 1)
    //{
    //    if (Item.ITEMTYPE.EQUIPMENT != _item.ItemType)
    //    {
    //        for (int i = 0; i < slots.Length; i++)
    //        {
    //            if (slots[i].Item != null)
    //            {
    //                if (slots[i].Item.ItemName == _item.ItemName)
    //                {
    //                    if (slots[i].ItemCount < 99)
    //                    {
    //                        slots[i].SetSlotCount(_count);
    //                        return;
    //                    }
    //                    else
    //                    {

    //                    }
                        
    //                }
    //            }                                                                                                
    //        }
    //    }

    //    for (int i = 0; i < slots.Length; ++i)
    //    {
    //        if (slots[i].Item == null)
    //        {
    //            slots[i].AddItem(_item, _count);
    //            return;
    //        }
    //    }
    //}
}
