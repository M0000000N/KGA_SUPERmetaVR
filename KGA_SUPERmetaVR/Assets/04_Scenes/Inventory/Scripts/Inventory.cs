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
            slots[i].Initialize();
        }

        for (int i = 0; i < GameManager.Instance.PlayerData.ItemSlotData.ItemData.Length; i++)
        {
            int pageSlotNumber = nowPage * numberOfSlots;
            if (pageSlotNumber <= i && i < (numberOfSlots + pageSlotNumber))
            {
                GameObject prefab = Resources.Load<GameObject>("Item/" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].ID).Prefabname);
                if (prefab == null) continue;
                slots[i - pageSlotNumber].ItemPrefab = Instantiate(prefab, slots[i - pageSlotNumber].transform);
                slots[i - pageSlotNumber].ItemPrefab.transform.localPosition = Vector3.zero;
                slots[i - pageSlotNumber].SetItemCount(GameManager.Instance.PlayerData.ItemSlotData.ItemData[i].Count);
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
        for (int i = 0; i < playerData.ItemSlotData.ItemData.Length; i++)
        {
            if (playerData.ItemSlotData.ItemData[i].ID <= 0)
            {
                playerData.ItemSlotData.ItemData[i].ID = _item.ItemID;
                playerData.ItemSlotData.ItemData[i].Count = _count;
                GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(_item.ItemID).Prefabname);
                slots[i - nowPage * numberOfSlots].ItemPrefab = Instantiate(prefab, slots[i - nowPage * numberOfSlots].transform);
                slots[i - nowPage * numberOfSlots].ItemPrefab.transform.localPosition = Vector3.zero;
                slots[i].AddItem(_item, _count);

                //UserDataBase.Instance.SaveItemData();
                Debug.Log("���������� ����");
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
                            Debug.Log("99�� �ȳ��� : �������� ����");
                            return;
                        }
                        else
                        {
                            int remainNumber = playerData.ItemSlotData.ItemData[i].Count + _count - 99;
                            playerData.ItemSlotData.ItemData[i].Count = 99;
                            slots[i].SetSlotCount(playerData.ItemSlotData.ItemData[i].Count);
                             for (int j = i + 1; j < playerData.ItemSlotData.ItemData.Length; ++j)
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
                                    Debug.Log("99�� ���� : �������� ����");
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
