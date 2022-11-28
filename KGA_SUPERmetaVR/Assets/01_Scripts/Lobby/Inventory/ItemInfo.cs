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
    private Button disconnectEquipButton;
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
        PlayerData playerData = GameManager.Instance.PlayerData;
        if (itemType.Equals("EQUIPMENT"))
        {
            useButton.interactable = false;
            if (playerData.ItemSlotData.ItemData[_slotNumber].Equip == (int)ITEMSTATE.EQUIP)
            {
                destoryButton.interactable = false;
                equipButton.gameObject.SetActive(false);
                disconnectEquipButton.gameObject.SetActive(true);
                disconnectEquipButton.onClick.AddListener(() => { UnEquipItem(_slotNumber); });
            }
            else
            {
                if (ItemManager.Instance.IsEquipItem)
                {
                    equipButton.interactable = false;
                }
                disconnectEquipButton.interactable = false;
                destoryButton.onClick.AddListener(() => { itemDelete.DeleteItem(_slotNumber); });
                equipButton.onClick.AddListener(() => { itemEquip.EquipItemButton(_slotNumber); });
            }
        }
        else if (itemType.Equals("USED"))
        {
            useButton.onClick.AddListener(() => { itemUse.UseItemButton(_slotNumber); });
            destoryButton.onClick.AddListener(() => { itemDelete.DeleteItem(_slotNumber); });
            equipButton.interactable = false;
        }

      
       
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
        equipButton.gameObject.SetActive(true);
        disconnectEquipButton.gameObject.SetActive(false);
        for (int childCount = 0; childCount < itemPrefabImage.transform.childCount; childCount++)
        {
            Destroy(itemPrefabImage.transform.GetChild(childCount).gameObject);
        }
        itemName.text = string.Empty;
        itemCount.text = string.Empty;
        itemDiscription.text = string.Empty;
    }

    public void ClearItemInfoButton()
    {
        equipButton.onClick.RemoveAllListeners();
        useButton.onClick.RemoveAllListeners();
        destoryButton.onClick.RemoveAllListeners();
    }

    public void UnEquipItem(int _slotNumber)
    {
        PlayerCustum playerCustum = GameManager.Instance.Player.GetComponentInChildren<PlayerCustum>();
        GameManager.Instance.PlayerData.Customize = GameManager.Instance.PlayerData.DefaultCustomize;
        // 바뀐 커스터마이징 서버에 올리는 코드
        ItemManager.Instance.IsEquipItem = false;
        GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].Equip = (int)ITEMSTATE.UNEQUIP;
        playerCustum.ChangeCustum(GameManager.Instance.PlayerData.DefaultCustomize);
        disconnectEquipButton.onClick.RemoveAllListeners();
        ItemManager.Instance.CloseItemInfoUI();
    }
}
