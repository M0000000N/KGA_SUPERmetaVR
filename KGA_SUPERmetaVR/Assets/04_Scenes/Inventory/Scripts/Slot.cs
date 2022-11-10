using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Slot : MonoBehaviour
{
    private int slotNumber;
    public int SlotNumber { get { return slotNumber; } set { slotNumber = value; } }

    [SerializeField]
    private GameObject itemPrefab;
    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }

    [SerializeField]
    private TextMeshProUGUI countText;

    [SerializeField]
    private GameObject countImage;

    [SerializeField]
    private Button infoButton;

    public Button InfoButton { get { return infoButton; } }
   
    

    private void Start()
    {
        //Initialize();
       // infoButton.onClick.AddListener(() => { XRManager.Instance.OpenItemInfo(slotNumber + XRManager.Instance.Inventory.NowPage * XRManager.Instance.Inventory.NumberOfSlots); } );
    }


    public void Initialize()
    {
        //if (itemPrefab = null) return;
        for (int childCount = 2; childCount < transform.childCount; childCount++)
        {
            Destroy(transform.GetChild(childCount).gameObject);
        }
        infoButton.onClick.RemoveAllListeners();
        itemPrefab = null;
        countText.text = "";
        countImage.SetActive(false);
    }

    public void SetItemCount(string _itemType, int _count)
    {
        if(_itemType != "EQUIPMENT")
        {
            Debug.Log("���ƴ�");
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
  
}
