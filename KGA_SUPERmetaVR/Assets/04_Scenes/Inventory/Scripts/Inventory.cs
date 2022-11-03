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

    [SerializeField]
    private int numberOfSlots;
    public int NumberOfSlots { get { return numberOfSlots; } set { numberOfSlots = value; } }

    private void Start()
    {
        slots = SlotGrid.GetComponentsInChildren<Slot>();
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < GameManager.Instance.PlayerData.ItemSlotData.ItemData.Length; i++)
        {
            GameObject prefab = Resources.Load<GameObject>("Item/" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemID).Prefabname);
            slots[i].ItemPrefab = Instantiate(prefab, slots[i].transform);
            slots[i].ItemPrefab.transform.localPosition = Vector3.zero;
            slots[i].SetItemCount(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemCount);
        }
    }

    public void AcquireItem(Item _item, int _count)
    {
        if (StaticData.GetItemSheet(_item.ItemID).Type != "EQUIPMENT")
        {
            for (int i = 0; i < numberOfSlots; i++)
            {
                if (slots[i].ItemPrefab != null)
                {
                    if (slots[i].ItemPrefab.name == StaticData.GetItemSheet(_item.ItemID).Prefabname)
                    {
                        if (GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemCount + _count <= 99)
                        {
                            GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemCount += _count;
                            UserDataBase.Instance.SaveItemData();
                            return;
                        }
                        else
                        {
                            int remainNumber = GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemCount + _count - 99;
                            GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemCount = 99;
                            for (int j = i + 1; j < numberOfSlots; ++j)
                            {
                                if (slots[j].ItemPrefab == null)
                                {
                                    GameManager.Instance.PlayerData.ItemSlotData.ItemData[j].ItemID = _item.ItemID;
                                    GameManager.Instance.PlayerData.ItemSlotData.ItemData[j].ItemCount = remainNumber;
                                    UserDataBase.Instance.SaveItemData();
                                    return;
                                }
                            }
                        }

                    }
                }
            }
        }

        // 아이템 종류가 장착 아이템일 시
        for (int i = 0; i < numberOfSlots; ++i)
        {
            if (slots[i].ItemPrefab == null)
            {
                GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemID = _item.ItemID;
                GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ItemCount = _count;
                UserDataBase.Instance.SaveItemData();
                return;
            }
        }
    }
}
