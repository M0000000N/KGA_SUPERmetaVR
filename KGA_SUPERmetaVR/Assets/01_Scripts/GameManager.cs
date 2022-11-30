using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    PlayerData playerData;
    public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }

    private GameObject player;
    public GameObject Player { get { return player; } set { player = value; } }

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        playerData = new PlayerData();
        PlayerData.PeekabooData = new PeekabooData();
        PlayerData.PeekabooData.CharacterList = new PlayerCharacter();
        playerData.FeeFawFumData = new FeeFawFumData();
        playerData.PaperSwanData = new PaperSwanData();
        playerData.CloverColonyData = new CloverColonyData();

        PlayerData.ItemSlotData = new ItemSlotData();
        playerData.ItemSlotData.ItemData = new ItemData[32];
        for (int i = 0; i < 32; i++)
        {
            playerData.ItemSlotData.ItemData[i] = new ItemData();
            PlayerData.ItemSlotData.ItemData[i].ID = 0;
            PlayerData.ItemSlotData.ItemData[i].Count = 0;
            PlayerData.ItemSlotData.ItemData[i].Equip = 0;
        }

        playerData.Friends = new FriendData();
        playerData.Friends.Friend = new List<int>();

        // [피카부 캐릭터 테스트 코드]
        PlayerData.PeekabooData.CharacterList.Character = new int[5];
        PlayerData.PeekabooData.CharacterList.Character[0] = 1;
        PlayerData.PeekabooData.CharacterList.Character[1] = 1;
        PlayerData.PeekabooData.CharacterList.Character[2] = 1;
        PlayerData.PeekabooData.CharacterList.Character[3] = 1;
        PlayerData.PeekabooData.CharacterList.Character[4] = 1;
        
        // -------------------------------------------------------------------------------- 테스트 코드

        // [아이템 테스트 코드]
        //for (int i = 0; i < 6; i++)
        //{
        //    int randomKey = Random.Range(1, 6);
        //    int randomValue = Random.Range(1, 99);

        //    playerData.ItemSlotData.ItemData[i].ID = StaticData.GetItemSheet(60000 + randomKey).ID;
        //    playerData.ItemSlotData.ItemData[i].Count = randomValue;
        //}

        // [친구]
        //playerData.Friends.Friend.Add(55);
        //playerData.Friends.Friend.Add(56);

        // -------------------------------------------------------------------------------- 테스트 코드
    }
}
