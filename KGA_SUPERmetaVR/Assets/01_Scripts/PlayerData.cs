using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    string id;
    public string ID { get { return id; } set { id = value; } }

    string nickName;
    public string Nickname { get { return nickName;} set { nickName = value; } }

    int coin;
    public int Coin { get { return coin; } set { coin = value; } }
}
