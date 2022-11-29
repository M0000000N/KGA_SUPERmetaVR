using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum ITEMSTATE
{
    UNEQUIP,
    EQUIP,
}

public class ItemManager : SingletonBehaviour<ItemManager>
{
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject inventoryUI;
    [SerializeField]
    private Button openInventoryButton;
    [SerializeField]
    private Button closeInventoryButton;
    [SerializeField]
    private GameObject itemInfoUI;
    [SerializeField]
    private Button itemInfoCloseOutUI;
    [SerializeField]
    private ItemInfo itemInfo;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private GameObject exitUI;
    [SerializeField]
    private Button openExitButton;
    [SerializeField]
    private Button closeExitButton;
    [SerializeField]
    private Button quitGameButton;
    public Inventory Inventory { get { return inventory; } }

    private GameObject[] playerCustumList;
    public GameObject[] PlayerCustumList { get { return playerCustumList; } set { playerCustumList = value; } }

    private int equipItemSlotNumber;
    public int EquipItemSlotNumber { get { return equipItemSlotNumber; } set { equipItemSlotNumber = value; } }

    private bool isEquipItem;
    public bool IsEquipItem { get { return isEquipItem; } set { isEquipItem = value; } }




    private void Start()
    {
        PlayerCustum playerCustum = GameManager.Instance.Player.GetComponentInChildren<PlayerCustum>();
        playerCustum.ChangeCustum(GameManager.Instance.PlayerData.Customize);
        if (GameManager.Instance.PlayerData.Customize == GameManager.Instance.PlayerData.DefaultCustomize)
        {
            isEquipItem = false;
        }
        else
        {
            IsEquipItem = true;
        }
        //UserDataBase.Instance.LoadItemData();
        openInventoryButton.onClick.AddListener(() => { OpenInvetoryUI(); });
        closeInventoryButton.onClick.AddListener(() => { CloseInventoryUI(); });
        openExitButton.onClick.AddListener(() => { OpenExitUI(); });
        closeExitButton.onClick.AddListener(() => { CloseExitUI(); });
        quitGameButton.onClick.AddListener(() => { QuitGameButton(); });

        //Button itemInfoCloseOutButton = itemInfoCloseOutUI.GetComponentInChildren<Button>();
        itemInfoCloseOutUI.onClick.AddListener(() => { CloseItemInfoUI(); });
        inventoryUI.SetActive(false);
        itemInfoUI.SetActive(false);
        exitUI.SetActive(false);
        itemInfoCloseOutUI.gameObject.SetActive(false);
    }

    private void QuitGameButton()
    {
        UserDataBase.Instance.SaveItemData();
        Application.Quit();
    }

    private void CloseExitUI()
    {
        exitUI.SetActive(false);
    }

    private void OpenExitUI()
    {
        exitUI.SetActive(true);
    }

    private void OpenInvetoryUI()
    {
        menuUI.SetActive(false);
        inventoryUI.SetActive(true);
        inventory.RefreshUI();
    }

    private void CloseInventoryUI()
    {
        inventoryUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void OpenItemInfo(int _slotNumber)
    {
        itemInfo.ItemPrefab(_slotNumber);
        itemInfo.ItemName(_slotNumber);
        itemInfo.ItemCount(_slotNumber);
        itemInfo.ItemDiscription(_slotNumber);
        itemInfo.ActiveButton(_slotNumber);
        itemInfoCloseOutUI.gameObject.SetActive(true);
        itemInfoUI.SetActive(true);
    }

    public void CloseItemInfoUI()
    {
        itemInfo.ClearItemInfoButton();
        itemInfo.ClearItemInfo();
        itemInfoCloseOutUI.gameObject.SetActive(false);
        itemInfoUI.SetActive(false);
    }

    
}
