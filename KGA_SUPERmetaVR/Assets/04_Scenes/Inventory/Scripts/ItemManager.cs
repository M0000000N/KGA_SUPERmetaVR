using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemManager : OnlyOneSceneSingleton<ItemManager>
{
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject inventoryUI;
    [SerializeField]
    private Button inventoryButton;
    [SerializeField]
    private Button returnMenuButton;
    [SerializeField]
    private GameObject itemInfoUI;
    [SerializeField]
    private GameObject itemInfoCloseOutUI;
    [SerializeField]
    private ItemInfo itemInfo;
    [SerializeField]
    private Inventory inventory;
    public Inventory Inventory { get { return inventory; } }
    

    private void Start()
    {
        inventoryButton.onClick.AddListener(() => { OpenInvetoryUI(); });
        returnMenuButton.onClick.AddListener(() => { CloseInventoryUI(); });
        
        Button itemInfoCloseOutButton = itemInfoCloseOutUI.GetComponentInChildren<Button>();
        itemInfoCloseOutButton.onClick.AddListener(() => { CloseItemInfoUI(); });
        inventoryUI.SetActive(false);
        itemInfoUI.SetActive(false);
        itemInfoCloseOutUI.SetActive(false);
    }

    private void OpenInvetoryUI()
    {
        menuUI.SetActive(false);
        inventoryUI.SetActive(true);
    }

    private void CloseInventoryUI()
    {
        UserDataBase.Instance.SaveItemData();
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
        itemInfoCloseOutUI.SetActive(true);
        itemInfoUI.SetActive(true);
    }

    public void CloseItemInfoUI()
    {
        itemInfo.DestroyButton.onClick.RemoveAllListeners();
        itemInfo.UseButton.onClick.RemoveAllListeners();
        itemInfo.ClearItemInfo();
        itemInfoCloseOutUI.SetActive(false);
        itemInfoUI.SetActive(false);
    }


}
