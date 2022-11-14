using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemSlot : MonoBehaviour
{
    private int slotNumber;

    [SerializeField]
    private GameObject itemPrefab;
    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }

    [SerializeField]
    private TextMeshProUGUI countText;
    public TextMeshProUGUI CountText { get { return countText; } set { countText = value; } }

    [SerializeField]
    private GameObject countImage;

    [SerializeField]
    private Button infoButton;
    public Button InfoButton { get { return infoButton; } }
   
    public void Initialize()
    {
        for (int childCount = 2; childCount < transform.childCount; childCount++)
        {
            Destroy(transform.GetChild(childCount).gameObject);
        }
        infoButton.onClick.RemoveAllListeners();
        itemPrefab = null;
        countText.text = string.Empty;
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

    public void ClearSlot()
    {
        for (int childCount = 2; childCount < transform.childCount; childCount++)
        {
            Destroy(transform.GetChild(childCount).gameObject);
        }
        infoButton.onClick.RemoveAllListeners();
        itemPrefab = null;
        countText.text = string.Empty;
        countImage.SetActive(false);
    }
  
}
