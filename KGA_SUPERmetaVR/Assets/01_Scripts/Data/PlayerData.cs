using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private string uid;
    public string UID { get { return uid; } set { uid = value; } }

    private string id;
    public string ID { get { return id; } set { id = value; } }

    private string nickName;
    public string Nickname { get { return nickName;} set { nickName = value; } }

    private int coin;
    public int Coin { get { return coin; } set { coin = value; } }

    private int clearTutorial;
    public int ClearTutorial { get { return clearTutorial; } set { clearTutorial = value; } }

    private ItemSlotData itemSlotData;
    public ItemSlotData ItemSlotData { get { return itemSlotData; } set { itemSlotData = value; } }

    private FriendData friends;
    public FriendData Friends { get { return friends; } set { friends = value; } }

    private PeekabooData peekabooData;
    public PeekabooData PeekabooData { get { return peekabooData; } set { peekabooData = value; } }

    private FeeFawFumData feeFawFumData;
    public FeeFawFumData FeeFawFumData { get { return feeFawFumData; } set { feeFawFumData = value; } }

    private PaperSwanData paperSwanData;
    public PaperSwanData PaperSwanData { get { return paperSwanData; } set { paperSwanData = value; } }

    private CloverColonyData cloverColonyData;
    public CloverColonyData CloverColonyData { get { return cloverColonyData; } set { cloverColonyData = value; } }
}

[System.Serializable]
public class ItemSlotData
{
    public ItemData[] ItemData;
}

[System.Serializable]
public class ItemData
{
    public int ID;
    public int Count;
}

[System.Serializable]
public class FriendData
{
    public List<int> Friend;
}

[System.Serializable]
public class PeekabooData
{
    public int SelectCharacter;
    public PlayerCharacter CharacterList;

    public int PlayCount;
    public int WinCount;
    public int DieCount;
    public float SurviveTime;
    public int AttackPC;
    public int AttackNPC;
}

[System.Serializable]
public class PlayerCharacter
{
    public int[] Character;
}

[System.Serializable]
public class FeeFawFumData
{
    public string CoolTime;
    public int TodayCount;
    public int TotalCount;
}

[System.Serializable]
public class PaperSwanData
{
    public string CoolTime;
    public int TodayCount;
    public int TotalCount;
}

[System.Serializable]
public class CloverColonyData
{
    public string CoolTime;
    public int TodayCount;
    public int TotalCount;
}