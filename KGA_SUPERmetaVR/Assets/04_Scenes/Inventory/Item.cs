using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ITEMTYPE
    {
        EQUIPMENT,
        USED,
        INGREDIENT,
    }
    public string ItemName;
    public ITEMTYPE ItemType;
    public Sprite ItemImage;
    public GameObject ItemPrefab;
    
}
