using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Inventory : MonoBehaviour
{
    private PlayerData playerData;

    [SerializeField]
    private GameObject SlotGrid;

    private Slot[] slots;

    [SerializeField]
    private int numberOfSlots;
    public int NumberOfSlots { get { return numberOfSlots; } set { numberOfSlots = value; } }

    private int nowPage;

    private void Start()
    {
        playerData = GameManager.Instance.PlayerData;
        slots = SlotGrid.GetComponentsInChildren<Slot>();
        Initialize();
    }

    private void Initialize()
    {
        nowPage = 0;
        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < GameManager.Instance.PlayerData.ItemSlotData.ItemData.Length; i++)
        {
            if (nowPage * numberOfSlots <= i && i < (numberOfSlots + numberOfSlots * nowPage))
            {
                GameObject prefab = Resources.Load<GameObject>("Item/" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID).Prefabname);
                slots[i - nowPage * numberOfSlots].ItemPrefab = Instantiate(prefab, slots[i - nowPage * numberOfSlots].transform);
                slots[i - nowPage * numberOfSlots].ItemPrefab.transform.localPosition = Vector3.zero;
                slots[i - nowPage * numberOfSlots].SetItemCount(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].Count);
            }
        }
    }

    public void PressNextButton()
    {
        if (nowPage < 3)
        {
            nowPage++;
        }
        RefreshUI();
    }

    public void PressPreviousButton()
    {
        if (nowPage > 0)
        {
            nowPage--;
        }
        RefreshUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Item testitem = new Item();
            testitem.ItemID = 60001;

            AcquireItem(testitem, 50);
            RefreshUI();
        }
    }

    public void AcquireItem(Item _item, int _count)
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            if (playerData.ItemSlotData.ItemData[i] == null)
            {
                playerData.ItemSlotData.ItemData[i].ID = _item.ItemID;
                playerData.ItemSlotData.ItemData[i].Count = _count;
                // UserDataBase.Instance.SaveItemData();
                return;
            }
            else
            {
                if (StaticData.GetItemSheet(_item.ItemID).Type != "EQUIPMENT")
                {
                    if (playerData.ItemSlotData.ItemData[i].ID == _item.ItemID)
                    {
                        if (playerData.ItemSlotData.ItemData[i].Count + _count <= 99)
                        {
                            playerData.ItemSlotData.ItemData[i].Count += _count;
                            // UserDataBase.Instance.SaveItemData();
                            return;
                        }
                        else
                        {
                            int remainNumber = playerData.ItemSlotData.ItemData[i].Count + _count - 99;
                            playerData.ItemSlotData.ItemData[i].Count = 99;
                            for (int j = i + 1; j < numberOfSlots; ++j)
                            {
                                if (playerData.ItemSlotData.ItemData[i] == null)
                                {
                                    playerData.ItemSlotData.ItemData[j].ID = _item.ItemID;
                                    playerData.ItemSlotData.ItemData[j].Count = remainNumber;
                                    // UserDataBase.Instance.SaveItemData();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
