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
        PlayerData.PlayerPeekabooData = new PlayerPeekabooData();
        PlayerData.PlayerPeekabooData.CharacterList = new PlayerCharacter();
        playerData.PlayerFeefawfumData = new PlayerFeefawfumData();

        // 테스트 코드
        GameManager.Instance.PlayerData.PlayerPeekabooData.CharacterList.Character = new int[5];
        GameManager.Instance.PlayerData.PlayerPeekabooData.CharacterList.Character[0] = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.CharacterList.Character[1] = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.CharacterList.Character[2] = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.CharacterList.Character[3] = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.CharacterList.Character[4] = 1;
        // 테스트 코드
    }
}
