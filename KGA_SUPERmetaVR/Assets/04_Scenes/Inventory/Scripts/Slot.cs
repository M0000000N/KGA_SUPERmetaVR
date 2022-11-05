using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }

    [SerializeField]
    private TextMeshProUGUI countText;

    [SerializeField]
    private GameObject countImage;

    [SerializeField]
    private Button infoButton;
    [SerializeField]
    private GameObject itemInfoUI;


    private void Start()
    {
        Initialize();
        infoButton.onClick.AddListener(() => { OpenInfo(); } );
        itemInfoUI.SetActive(false);
    }


    public void Initialize()
    {
        for (int childCount = 3; childCount < transform.childCount; childCount++)
        {
            Destroy(transform.GetChild(childCount).gameObject);
        }

        itemPrefab = null;
        countText.text = "";
        countImage.SetActive(false);
    }

    public void SetItemCount(string _itemType, int _count)
    {
        if(_itemType != "EQUIPMENT")
        {
            countText.text = _count.ToString();
            countImage.SetActive(true);
        }
        else
        {
            countImage.SetActive(false);
        }
    }

    public void AddItem(string _itemType, int _count)
    {
        
        if (_itemType != "EQUIPMENT")
        {
            countImage.SetActive(true);
            countText.text = _count.ToString();
        }
        else
        {
            countText.text = _count.ToString();
            countImage.SetActive(false);
        }
        
    }

    public void SetSlotCount(int _count)
    {
        countText.text = _count.ToString();

        if (_count <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        itemPrefab = null;
        countText.text = "0";
        countImage.SetActive(false);
    }
    private void OpenInfo()
    {
        itemInfoUI.transform.SetAsLastSibling();
        itemInfoUI.SetActive(true);
    }
}
