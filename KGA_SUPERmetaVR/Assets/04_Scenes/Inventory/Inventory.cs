using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject SlotGrid;

    private Slot[] slots;

    private int slotCount = 8;
    private int nowSlot;

    private void Start()
    {
        slots = SlotGrid.GetComponentsInChildren<Slot>();
        Initialize();
    }

    private void Initialize()
    {
        nowSlot = 0;
        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < GameManager.Instance.PlayerData.ItemSlotData.ItemData.Length; i++)
        {
            if(nowSlot * slotCount <= i && i < (slotCount + slotCount * nowSlot))
            {
                GameObject prefab = Resources.Load<GameObject>("Item/" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID).Prefabname);
                slots[i].ItemPrefab = Instantiate(prefab, slots[i].transform);
                slots[i].ItemPrefab.transform.localPosition = Vector3.zero;
                slots[i].SetItemCount(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].Count);
            }
        }
    }

    public void PressNextButton()
    {
        if(nowSlot < 3)
        {
            nowSlot++;
        }
        RefreshUI();
    }

    public void PressPreviousButton()
    {
        if (nowSlot > 0)
        {
            nowSlot--;
        }
        RefreshUI();
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
