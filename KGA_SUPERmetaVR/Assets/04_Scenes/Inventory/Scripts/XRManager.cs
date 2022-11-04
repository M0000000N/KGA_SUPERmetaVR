using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XRManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject inventoryUI;
    [SerializeField]
    private Button inventoryButton;
    [SerializeField]
    private Button returnMenuButton;

    private void Start()
    {
        inventoryButton.onClick.AddListener(() => { OpenInvetoryUI(); });
        returnMenuButton.onClick.AddListener(() => { CloseInventoryUI(); });
        inventoryUI.SetActive(false);
    }

    private void OpenInvetoryUI()
    {
        UserDataBase.Instance.LoadItemData();
        menuUI.SetActive(false);
        inventoryUI.SetActive(true);
    }

    private void CloseInventoryUI()
    {
        UserDataBase.Instance.SaveItemData();
        inventoryUI.SetActive(false);
        menuUI.SetActive(true);
    }
}
