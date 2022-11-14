using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUse : MonoBehaviour
{
    [SerializeField]
    private Button itemUseButton;
    [SerializeField]
    private Button itemUseCancelButton;
    [SerializeField]
    private TextMeshProUGUI UseDiscriptionText;

    public void UseItemButton(int _slotNumber)
    {
        gameObject.SetActive(true);
        string itemName = StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Name;
        UseDiscriptionText.text = "'" + itemName + "'을(를) 사용 하시겠습니까?\n\n 한 번 사용된 " + itemName + "은(는) 삭제됩니다.\n 한 번 사용한" + itemName + "은(는) 철회가 불가능합니다.";
        itemUseButton.onClick.AddListener(() => { UseItem(_slotNumber); });
        itemUseCancelButton.onClick.AddListener(() => { CloseItemUseUI(); });
    }

    private void UseItem(int _slotNumber)
    {
        itemUseButton.onClick.RemoveAllListeners();
        GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count -= 1;
        if (GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count == 0)
        {
            GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID = 0;
            GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count = 0;
            ItemManager.Instance.Inventory.Slots[_slotNumber - (ItemManager.Instance.Inventory.NowPage * ItemManager.Instance.Inventory.NumberOfSlots)].ClearSlot();
        }

        ItemManager.Instance.Inventory.Slots[_slotNumber - (ItemManager.Instance.Inventory.NowPage * ItemManager.Instance.Inventory.NumberOfSlots)].CountText.text = GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count.ToString();

        gameObject.SetActive(false);
        ItemManager.Instance.CloseItemInfoUI();
    }

    public void CloseItemUseUI()
    {
        itemUseButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
