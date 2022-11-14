using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    PlayerData playerData;
    public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }

    void Awake()
    {
        Initialize();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            UnityEngine.Debug.Log(GameManager.Instance.playerData.ItemSlotData);
        }
    }

    public void Initialize()
    {
        playerData = new PlayerData();
        PlayerData.PeekabooData = new PeekabooData();
        PlayerData.PeekabooData.CharacterList = new PlayerCharacter();
        playerData.FeeFawFumData = new FeeFawFumData();

        PlayerData.ItemSlotData = new ItemSlotData();
        playerData.ItemSlotData.ItemData = new ItemData[32];
        Debug.Log($"아이템슬롯 길이{playerData.ItemSlotData.ItemData.Length}");
        for (int i = 0; i < 32; i++)
        {
            playerData.ItemSlotData.ItemData[i] = new ItemData();
            PlayerData.ItemSlotData.ItemData[i].ID = 0;
            PlayerData.ItemSlotData.ItemData[i].Count = 0;
            Debug.Log($"아이템슬롯 길이{playerData.ItemSlotData.ItemData[i].ID}");
        }

        // 테스트 코드
        PlayerData.PeekabooData.CharacterList.Character = new int[5];
        PlayerData.PeekabooData.CharacterList.Character[0] = 1;
        PlayerData.PeekabooData.CharacterList.Character[1] = 1;
        PlayerData.PeekabooData.CharacterList.Character[2] = 1;
        PlayerData.PeekabooData.CharacterList.Character[3] = 1;
        PlayerData.PeekabooData.CharacterList.Character[4] = 1;

        //for (int i = 0; i < 6; i++)
        //{
        //    int randomKey = Random.Range(1, 6);
        //    int randomValue = Random.Range(1, 99);

        //    playerData.ItemSlotData.ItemData[i].ID = StaticData.GetItemSheet(60000 + randomKey).ID;
        //    playerData.ItemSlotData.ItemData[i].Count = randomValue;
        //}
        
        // TODO : 아이템 임시 코드 넣어주기
        // 테스트 코드
    }
}
