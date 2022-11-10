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
    [SerializeField]
    private Button destoryButton;
    [SerializeField]
    private Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() => { XRManager.Instance.CloseItemInfoUI(); });
    }

    public void ActiveButton(int _slotNumber)
    {
        string itemType = StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Type;
        
        if (itemType == "EQUIPMENT")
        {
            useButton.interactable = false;
        }
        else if (itemType == "USED")
        {
            equipButton.interactable = false;
        }
    }

    public void ItemPrefab(int _slotNumber)
    {
        GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(GameManager.Instance.PlayerData.ItemSlotData.ItemData[_slotNumber].ID).Prefabname);
        itemPrefab = Instantiate(prefab, itemPrefabImage.transform);
        itemPrefab.transform.localPosition = Vector3.zero;

        //itemPrefab.transform.position = new Vector3 (itemPrefab.transform.position.x - 0.2f, itemPrefab.transform.position.y);
        //itemPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
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
        //itemPrefab = null;
        //itemPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
        itemName.text = "";
        itemCount.text = "";
        itemDiscription.text = "";
    }
}
