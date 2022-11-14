using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject itemPrefabImage;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemCount;
    [SerializeField]
    private TextMeshProUGUI itemDiscription;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private Button useButton;
    public Button UseButton { get { return useButton; } }
    [SerializeField]
    private Button destoryButton;
    public Button DestroyButton { get { return destoryButton; } }
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private ItemDelete itemDelete;
    [SerializeField]
    private ItemUse itemUse;
    [SerializeField]
    private ItemEquip itemEquip;
 

    private void Start()
    {
        closeButton.onClick.AddListener(() => { ItemManager.Instance.CloseItemInfoUI(); });
    }

    public void ActiveButton(int _slotNumber)
    {
        string itemType = StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Type;
        
        if (itemType.Equals("EQUIPMENT"))
        {
            useButton.interactable = false;
        }
        else if (itemType.Equals("USED"))
        {
            equipButton.interactable = false;
        }
        destoryButton.onClick.AddListener(() => { itemDelete.DeleteItem(_slotNumber); });
        useButton.onClick.AddListener(() => { itemUse.UseItemButton(_slotNumber); });
        equipButton.onClick.AddListener(() => { itemEquip.EquipItemButton(_slotNumber); });
    }

    public void ItemPrefab(int _slotNumber)
    {
        GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Prefabname);
        itemPrefab = Instantiate(prefab, itemPrefabImage.transform);
        itemPrefab.transform.localPosition = Vector3.zero;
    }

    public void ItemName(int _slotNumber)
    {
        itemName.text = StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Name;
    }

    public void ItemCount(int _slotNumber)
    {
        itemCount.text = GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Count.ToString();
    }

    public void ItemDiscription(int _slotNumber)
    {
        itemDiscription.text = StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Discription;
    }

    public void ClearItemInfo()
    {
        useButton.interactable = true;
        equipButton.interactable = true;
        for (int childCount = 0; childCount < itemPrefabImage.transform.childCount; childCount++)
        {
            Destroy(itemPrefabImage.transform.GetChild(childCount).gameObject);
        }
        itemName.text = string.Empty;
        itemCount.text = string.Empty;
        itemDiscription.text = string.Empty;
    }
}
