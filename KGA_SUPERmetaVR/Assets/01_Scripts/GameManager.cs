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
        for (int i = 0; i < 32; i++)
        {
            PlayerData.ItemSlotData.ItemData[i] = new ItemData();
        }

        // 테스트 코드
        PlayerData.PeekabooData.CharacterList.Character = new int[5];
        PlayerData.PeekabooData.CharacterList.Character[0] = 1;
        PlayerData.PeekabooData.CharacterList.Character[1] = 1;
        PlayerData.PeekabooData.CharacterList.Character[2] = 1;
        PlayerData.PeekabooData.CharacterList.Character[3] = 1;
        PlayerData.PeekabooData.CharacterList.Character[4] = 1;

        for (int i = 0; i < 6; i++)
        {
            int randomKey = Random.Range(1, 3);
            int randomValue = Random.Range(1, 3);

            playerData.ItemSlotData.ItemData[i].ID = StaticData.GetItemSheet(60000 + randomKey).ID;
            playerData.ItemSlotData.ItemData[i].Count = randomValue;
        }
        // TODO : 아이템 임시 코드 넣어주기
        // 테스트 코드
    }
}
