using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject SlotGrid;

    private Slot[] slots;

    private void Start()
    {
        slots = SlotGrid.GetComponentsInChildren<Slot>();     
    }

    private void AcquireItem(Item _item, int _count = 1)
    {
        if (Item.ITEMTYPE.EQUIPMENT != _item.ItemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Item != null)
                {
                    if (slots[i].Item.ItemName == _item.ItemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }                                                                                                
            }
        }

        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i].Item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
