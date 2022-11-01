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

    PeekabooData peekabooData;
    public PeekabooData PeekabooData { get { return peekabooData; } set { peekabooData = value; } }

    FeeFawFumData feeFawFumData;
    public FeeFawFumData FeeFawFumData { get { return feeFawFumData; } set { feeFawFumData = value; } }

    PaperSwanData paperSwanData;
    public PaperSwanData PaperSwanData { get { return paperSwanData; } set { paperSwanData = value; } }

    CloverColonyData cloverColonyData;
    public CloverColonyData CloverColonyData { get { return cloverColonyData; } set { cloverColonyData = value; } }
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