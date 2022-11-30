using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDelete : MonoBehaviour
{
    [SerializeField]
    private Button itemDeleteButton;
    [SerializeField]
    private Button itemDeleteCancelButton;
    [SerializeField]
    private GameObject itemCountDeleteUI;
    private ItemCountDelete itemCountDelete;
    [SerializeField]
    private TextMeshProUGUI destroyDiscriptionText;

    private void OnEnable()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
    }


    public void DeleteItem(int _slotNumber)
    {
        gameObject.SetActive(true);
        destroyDiscriptionText.text = "'" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Name + "'을(를) 파괴 하시겠습니까?\n 한 번 파괴된 아이템은 복구 할 수 없습니다.";
        string itemType = StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Type;

        if (itemType.Equals("EQUIPMENT"))
        {
            itemDeleteButton.onClick.AddListener(() => { DeleteEquipmentItme(_slotNumber); } );
        }
        else if (itemType.Equals("USED"))
        {
            itemDeleteButton.onClick.AddListener(() => { DeleteUseItem(_slotNumber); });
        }

        itemDeleteCancelButton.onClick.AddListener(() => { CloseItemDeleteUI(); });
    }

    private void DeleteUseItem(int _slotNumber)
    {
        itemCountDelete = itemCountDeleteUI.GetComponent<ItemCountDelete>();
        itemCountDelete.CountDestroyItem(_slotNumber);
        itemDeleteButton.onClick.RemoveAllListeners();
        itemCountDeleteUI.SetActive(true);
    }

    private void DeleteEquipmentItme(int _slotNumber)
    {
        itemDeleteButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
        GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID = 0;
        GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count = 0;
        ItemManager.Instance.Inventory.Slots[_slotNumber - (ItemManager.Instance.Inventory.NowPage * ItemManager.Instance.Inventory.NumberOfSlots)].ClearSlot();
        ItemManager.Instance.CloseItemInfoUI();
    }

    public void CloseItemDeleteUI()
    {
        itemDeleteButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
