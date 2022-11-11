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

    [SerializeField]
    private int maxNnumberOfItems;

    private void Start()
    {
        playerData = GameManager.Instance.PlayerData;
        ///테스트용
        for (int i = 0; i < 3; i++)
        {
            int randomKey = UnityEngine.Random.Range(1, 3);
            int randomValue = UnityEngine.Random.Range(1, maxNnumberOfItems);

            playerData.ItemSlotData.ItemData[i].ID = StaticData.GetItemSheet(60000 + randomKey).ID;
            playerData.ItemSlotData.ItemData[i].Count = randomValue;
        }
        UserDataBase.Instance.SaveItemData();
        ///
        // UserDataBase.Instance.LoadItemData();
        slots = SlotGrid.GetComponentsInChildren<Slot>();
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        nowPage = 0;
        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < NumberOfSlots; i++)
        {
           // slots[i].Initialize();
        }

        for (int i = 0; i < GameManager.Instance.PlayerData.ItemSlotData.ItemData.Length; i++)
        {
            int pageSlotNumber = nowPage * numberOfSlots;
            if (pageSlotNumber <= i && i < (numberOfSlots + pageSlotNumber))
            {
                GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID).Prefabname);
                if (prefab == null) continue;
                slots[i - pageSlotNumber].ItemPrefab = Instantiate(prefab, slots[i - pageSlotNumber].transform);
                slots[i - pageSlotNumber].ItemPrefab.transform.localPosition = Vector3.zero;
                slots[i - pageSlotNumber].SetItemCount(StaticData.GetItemSheet(playerData.ItemSlotData.ItemData[i].ID).Type, playerData.ItemSlotData.ItemData[i].Count);
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

    private void GetPrefab(Item _item, int _itemID)
    {
        GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(_item.ItemID).Prefabname);
        if (prefab == null) return;
        slots[_itemID].ItemPrefab = Instantiate(prefab, slots[_itemID].transform);
        slots[_itemID].ItemPrefab.transform.localPosition = Vector3.zero;
    }

    public void AcquireItem(Item _item, int _count)
    {
        for (int i = 0; i < playerData.ItemSlotData.ItemData.Length; i++)
        {
            if (playerData.ItemSlotData.ItemData[i].ID <= 0)
            {
                playerData.ItemSlotData.ItemData[i].ID = _item.ItemID;
                playerData.ItemSlotData.ItemData[i].Count = _count;
                RefreshUI();
                //slots[i].AddItem(_item, _count);

                //UserDataBase.Instance.SaveItemData();
                Debug.Log("장착아이템들어옴");
                return;
            }
            else
            {
                if (StaticData.GetItemSheet(_item.ItemID).Type != "EQUIPMENT")
                {
                    if (playerData.ItemSlotData.ItemData[i].ID == _item.ItemID)
                    {
                        if (playerData.ItemSlotData.ItemData[i].Count + _count <= maxNnumberOfItems)
                        {
                            playerData.ItemSlotData.ItemData[i].Count += _count;
                            slots[i - nowPage * numberOfSlots].SetSlotCount(playerData.ItemSlotData.ItemData[i].Count);
                            // UserDataBase.Instance.SaveItemData();
                            RefreshUI();
                            Debug.Log("99개 안넘음");
                            return;
                        }
                        else
                        {
                            int remainNumber = playerData.ItemSlotData.ItemData[i].Count + _count - maxNnumberOfItems;
                            playerData.ItemSlotData.ItemData[i].Count = maxNnumberOfItems;
                            //slots[i].SetSlotCount(playerData.ItemSlotData.ItemData[i].Count);
                             for (int j = i + 1; j < playerData.ItemSlotData.ItemData.Length; ++j)
                            {
                                if (playerData.ItemSlotData.ItemData[j].ID == _item.ItemID && playerData.ItemSlotData.ItemData[j].Count + remainNumber > maxNnumberOfItems)
                                {
                                    remainNumber = playerData.ItemSlotData.ItemData[j].Count + _count - maxNnumberOfItems;
                                    playerData.ItemSlotData.ItemData[j].Count = maxNnumberOfItems;
                                    //slots[j].SetSlotCount(playerData.ItemSlotData.ItemData[j].Count);
                                }
                                else if (playerData.ItemSlotData.ItemData[j].ID == _item.ItemID && playerData.ItemSlotData.ItemData[j].Count + remainNumber <= maxNnumberOfItems)
                                {
                                    playerData.ItemSlotData.ItemData[j].Count += remainNumber;
                                    //UserDataBase.Instance.SaveItemData();
                                    //slots[j].SetSlotCount(playerData.ItemSlotData.ItemData[j].Count);
                                    Debug.Log("99개넘음");
                                    RefreshUI();
                                    return;
                                }
                                else if (playerData.ItemSlotData.ItemData[j].ID <= 0)
                                {
                                    playerData.ItemSlotData.ItemData[j].ID = _item.ItemID;
                                    playerData.ItemSlotData.ItemData[j].Count = remainNumber;
                                    GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(_item.ItemID).Prefabname);
                                    RefreshUI();
                                    //slots[j].ItemPrefab = Instantiate(prefab, slots[j].transform);
                                    //slots[j].ItemPrefab.transform.localPosition = Vector3.zero;
                                    //slots[j].AddItem(_item, remainNumber);
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
