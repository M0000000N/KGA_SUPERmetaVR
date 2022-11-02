using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum ITEMTYPE
    {
        EQUIPMENT,
        USED,
        INGREDIENT,
    }
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    private int itemID;
    public int ItemID { get { return itemID; } set { itemID = value; } }

        private string itemName;
    public string ItemName { get { return itemName; } set { itemName = value; } }

    private ITEMTYPE itemType;
    public ITEMTYPE ItemType { get { return itemType; } set { itemType = value; } }

    private Sprite itemImage;
    public Sprite ItemImage { get { return itemImage; } set { itemImage = value; } }

    private GameObject itemPrefab;
    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }

}
