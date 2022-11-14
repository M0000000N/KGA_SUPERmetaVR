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

    private ItemSlot[] slots;
    public ItemSlot[] Slots { get { return slots; } }

    [SerializeField]
    private int numberOfSlots;
    public int NumberOfSlots { get { return numberOfSlots; } }

    private int nowPage;
    public int NowPage { get { return nowPage; } }

    [SerializeField]
    private int maxNnumberOfItems;


    private void Start()
    {
        playerData = GameManager.Instance.PlayerData;
        Debug.Log($"인벤토리 길이 {playerData.ItemSlotData.ItemData.Length}");
        ///테스트용
        //for (int i = 0; i < 4; i++)
        //{
        //    int randomKey = UnityEngine.Random.Range(1, 5);
        //    int randomValue = UnityEngine.Random.Range(1, maxNnumberOfItems);

        //    playerData.ItemSlotData.ItemData[i].ID = StaticData.GetItemSheet(60000 + randomKey).ID;
        //    playerData.ItemSlotData.ItemData[i].Count = randomValue;
        //}
         
        ///
        
        slots = SlotGrid.GetComponentsInChildren<ItemSlot>();
        Initialize();
    }

    private void Initialize()
    {
        nowPage = 0;
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            slots[i].Initialize();
        }

        for (int i = 0; i < GameManager.Instance.PlayerData.ItemSlotData.ItemData.Length; i++)
        {
            int slotID = i;
            int pageSlotNumber = nowPage * numberOfSlots;
            if (pageSlotNumber <= i && i < (numberOfSlots + pageSlotNumber))
            {
                Debug.Log($"인벤토리 길이dsds {(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID)}");
                if (GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID > 0)
                {
                    GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID).Prefabname);
                    if (prefab == null) continue;
                    slots[i - pageSlotNumber].ItemPrefab = Instantiate(prefab, slots[i - pageSlotNumber].transform);
                    slots[i - pageSlotNumber].ItemPrefab.transform.localPosition = Vector3.zero;
                    slots[i - pageSlotNumber].SetItemCount(StaticData.GetItemSheet(playerData.ItemSlotData.ItemData[i].ID).Type, playerData.ItemSlotData.ItemData[i].Count);
                    slots[i - pageSlotNumber].InfoButton.onClick.AddListener(() => { ItemManager.Instance.OpenItemInfo(slotID); });
                }
                
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

    public void AcquireItem(Item _item, int _count)
    {
        ItemData[] itemData = playerData.ItemSlotData.ItemData;
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i].ID <= 0)
            {
                itemData[i].ID = _item.ItemID;
                itemData[i].Count = _count;
                RefreshUI();
                return;
            }
            else
            {
                if (StaticData.GetItemSheet(_item.ItemID).Type != "EQUIPMENT")
                {
                    if (itemData[i].ID == _item.ItemID)
                    {
                        if (itemData[i].Count + _count <= maxNnumberOfItems)
                        {
                            itemData[i].Count += _count;
                            slots[i - nowPage * numberOfSlots].SetSlotCount(itemData[i].Count);
                            RefreshUI();
                            return;
                        }
                        else
                        {
                            int remainNumber = itemData[i].Count + _count - maxNnumberOfItems;
                            itemData[i].Count = maxNnumberOfItems;
                             for (int j = i + 1; j < itemData.Length; ++j)
                            {
                                if (itemData[j].ID == _item.ItemID && itemData[j].Count + remainNumber > maxNnumberOfItems)
                                {
                                    remainNumber = itemData[j].Count + _count - maxNnumberOfItems;
                                    itemData[j].Count = maxNnumberOfItems;
                                }
                                else if (itemData[j].ID == _item.ItemID && itemData[j].Count + remainNumber <= maxNnumberOfItems)
                                {
                                    itemData[j].Count += remainNumber;
                                    RefreshUI();
                                    return;
                                }
                                else if (itemData[j].ID <= 0)
                                {
                                    itemData[j].ID = _item.ItemID;
                                    itemData[j].Count = remainNumber;
                                    GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(_item.ItemID).Prefabname);
                                    RefreshUI();
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
