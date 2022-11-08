using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XRManager : OnlyOneSceneSingleton<XRManager>
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
    private Button itemInfoCloseButton;
    

    private void Start()
    {
        inventoryButton.onClick.AddListener(() => { OpenInvetoryUI(); });
        returnMenuButton.onClick.AddListener(() => { CloseInventoryUI(); });
        itemInfoCloseButton.onClick.AddListener(() => { CloseItemInfoUI(); });
        inventoryUI.SetActive(false);
        itemInfoUI.SetActive(false);
        
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

    public void OpenItemInfo()
    {
        //XRManager.Instance.ItemInfoUI.transform.SetAsLastSibling();
        itemInfoUI.SetActive(true);
    }

    private void CloseItemInfoUI()
    {
        itemInfoUI.SetActive(false);
    }


}
