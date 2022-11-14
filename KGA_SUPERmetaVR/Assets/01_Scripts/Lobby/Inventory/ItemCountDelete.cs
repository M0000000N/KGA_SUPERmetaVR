using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCountDelete : MonoBehaviour
{
    [SerializeField]
    private Button plusOneButton;
    [SerializeField]
    private Button plusTenButton;
    [SerializeField]
    private Button maxButton;
    [SerializeField]
    private Button clearButton;
    [SerializeField]
    private Button okButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private TMP_InputField destroyItemCountText;
    [SerializeField]
    private GameObject ItemDeleteUI;
    private ItemDelete itemDelete;
    private int itemDeleteCount;

    private void Update()
    {
        if(itemDeleteCount > 0)
        {
            destroyItemCountText.text = itemDeleteCount.ToString();
        }
        else
        {
            destroyItemCountText.text = null;
        }
    }

    public void CountDestroyItem(int _slotNumber)
    {
        plusOneButton.onClick.AddListener(() => { PlusCount(1, _slotNumber); });
        plusTenButton.onClick.AddListener(() => { PlusCount(10, _slotNumber); });
        clearButton.onClick.AddListener(() => { ClearCount(); });
        okButton.onClick.AddListener(() => { DeleteCountItem(_slotNumber); });
        maxButton.onClick.AddListener(() => { PlusCount(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count, _slotNumber); });
        cancelButton.onClick.AddListener(() => { CloseItemCountDeleteUI(); });
    }
    private void PlusCount(int _count, int _slotNumber)
    {
        Debug.Log($"¹öÆ°µé¾î¿È{itemDeleteCount}");
        if (itemDeleteCount + _count < GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count)
        {
            itemDeleteCount += _count;
        }
        else
        {
            itemDeleteCount = GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count;
        }
    }

    private void ClearCount()
    {
        destroyItemCountText.text = null;
        itemDeleteCount = 0;
    }

    private void DeleteCountItem(int _slotNumber)
    {
        GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count -= itemDeleteCount;
        if (GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count == 0)
        {
            GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID = 0;
            GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count = 0;
            ItemManager.Instance.Inventory.Slots[_slotNumber].ClearSlot();
            ItemManager.Instance.CloseItemInfoUI();
        }
        ItemManager.Instance.Inventory.Slots[_slotNumber].CountText.text = GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count.ToString();
        CloseItemCountDeleteUI();
        gameObject.SetActive(false);
    }
    private void CloseItemCountDeleteUI()
    {
        ClearButton();
        ClearCount();
        itemDelete = ItemDeleteUI.GetComponent<ItemDelete>();
        itemDelete.CloseItemDeleteUI();
        gameObject.SetActive(false);
        ItemDeleteUI.SetActive(false);
        ItemManager.Instance.CloseItemInfoUI();
    }

    private void ClearButton()
    {
        plusOneButton.onClick.RemoveAllListeners();
        plusTenButton.onClick.RemoveAllListeners();
        clearButton.onClick.RemoveAllListeners();
        okButton.onClick.RemoveAllListeners();
        maxButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }
}
