using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int itemID;
    public int ItemID { get { return itemID; } set { itemID = value; } }
}
