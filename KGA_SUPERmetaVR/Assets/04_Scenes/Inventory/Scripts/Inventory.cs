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
        for (int i = 0; i < 3; i++)
        {
            int randomKey = UnityEngine.Random.Range(1, 3);
            int randomValue = UnityEngine.Random.Range(1, 99);

            playerData.ItemSlotData.ItemData[i].ID = StaticData.GetItemSheet(60000 + randomKey).ID;
            playerData.ItemSlotData.ItemData[i].Count = randomValue;
        }
        UserDataBase.Instance.SaveItemData();
        // UserDataBase.Instance.LoadItemData();
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
                if (GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID > 0)
                {
                    GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID).Prefabname);
                    slots[i - nowPage * numberOfSlots].ItemPrefab = Instantiate(prefab, slots[i - nowPage * numberOfSlots].transform);
                    slots[i - nowPage * numberOfSlots].ItemPrefab.transform.localPosition = Vector3.zero;
                    slots[i - nowPage * numberOfSlots].SetItemCount(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].Count);

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
        for (int i = 0; i < numberOfSlots * (nowPage + 1); i++)
        {
            if (playerData.ItemSlotData.ItemData[i].ID == 0)
            {
                playerData.ItemSlotData.ItemData[i].ID = _item.ItemID;
                playerData.ItemSlotData.ItemData[i].Count = _count;
                GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(_item.ItemID).Prefabname);
                slots[i - nowPage * numberOfSlots].ItemPrefab = Instantiate(prefab, slots[i - nowPage * numberOfSlots].transform);
                slots[i - nowPage * numberOfSlots].ItemPrefab.transform.localPosition = Vector3.zero;
                slots[i].AddItem(_item, _count);

                //UserDataBase.Instance.SaveItemData();
                Debug.Log("장비아이템이 들어옴");
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
                            slots[i - nowPage * numberOfSlots].SetSlotCount(playerData.ItemSlotData.ItemData[i].Count);
                            // UserDataBase.Instance.SaveItemData();
                            Debug.Log("99개 안넘음 : 아이템이 들어옴");
                            return;
                        }
                        else
                        {
                            int remainNumber = playerData.ItemSlotData.ItemData[i].Count + _count - 99;
                            playerData.ItemSlotData.ItemData[i].Count = 99;
                            slots[i].SetSlotCount(playerData.ItemSlotData.ItemData[i].Count);
                            for (int j = i + 1; j < numberOfSlots * 4; ++j)
                            {
                                if (playerData.ItemSlotData.ItemData[j].ID == _item.ItemID && playerData.ItemSlotData.ItemData[j].Count + remainNumber > 99)
                                {
                                    remainNumber = playerData.ItemSlotData.ItemData[j].Count + _count - 99;
                                    playerData.ItemSlotData.ItemData[j].Count = 99;
                                    slots[j].SetSlotCount(playerData.ItemSlotData.ItemData[j].Count);
                                }
                                else if (playerData.ItemSlotData.ItemData[j].ID == _item.ItemID && playerData.ItemSlotData.ItemData[j].Count + remainNumber <= 99)
                                {
                                    playerData.ItemSlotData.ItemData[j].Count += remainNumber;
                                    //UserDataBase.Instance.SaveItemData();
                                    slots[j].SetSlotCount(playerData.ItemSlotData.ItemData[j].Count);
                                    Debug.Log("99개 넘음 : 아이템이 들어옴");
                                    return;
                                }
                                else if (playerData.ItemSlotData.ItemData[j].ID == 0)
                                {
                                    playerData.ItemSlotData.ItemData[j].ID = _item.ItemID;
                                    playerData.ItemSlotData.ItemData[j].Count = remainNumber;
                                    GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(_item.ItemID).Prefabname);
                                    slots[j].ItemPrefab = Instantiate(prefab, slots[j].transform);
                                    slots[j].ItemPrefab.transform.localPosition = Vector3.zero;
                                    slots[j].AddItem(_item, remainNumber);
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
