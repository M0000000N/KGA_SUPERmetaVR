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
    // private면 Json파싱에 사용할 수 없는 이슈로 Public 선언

    public int SelectCharacter;
    //public int SelectCharacter { get { return selectCharacter; } set { selectCharacter = value; } }
    public int[] Character;
    //public int[] Character { get { return character; } set { character = value; } }
}