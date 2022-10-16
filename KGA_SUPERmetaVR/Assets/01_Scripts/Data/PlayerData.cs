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

    PlayerPeekabooData playerPeekabooData;
    public PlayerPeekabooData PlayerPeekabooData { get { return playerPeekabooData; } set { playerPeekabooData = value; } }
}

[System.Serializable]
public class PlayerPeekabooData
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