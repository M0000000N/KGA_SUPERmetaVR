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

    public void Initialize()
    {
        playerData = new PlayerData();
        PlayerData.PeekabooData = new PeekabooData();
        PlayerData.PeekabooData.CharacterList = new PlayerCharacter();
        playerData.FeeFawFumData = new FeeFawFumData();

        // 테스트 코드
        GameManager.Instance.PlayerData.PeekabooData.CharacterList.Character = new int[5];
        GameManager.Instance.PlayerData.PeekabooData.CharacterList.Character[0] = 1;
        GameManager.Instance.PlayerData.PeekabooData.CharacterList.Character[1] = 1;
        GameManager.Instance.PlayerData.PeekabooData.CharacterList.Character[2] = 1;
        GameManager.Instance.PlayerData.PeekabooData.CharacterList.Character[3] = 1;
        GameManager.Instance.PlayerData.PeekabooData.CharacterList.Character[4] = 1;

        // TODO : 아이템 임시 코드 넣어주기
        // 테스트 코드
    }
}
