using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbySpawner : MonoBehaviourPun
{
    [SerializeField] private GameObject LobbyCamera;
    #region 나중에 수정 할 부분
    [SerializeField] private string myCharacterName; // 플레이어 외형 아이템 관련 연동 시 수정해야 할 부분
    [SerializeField] private string mySpawnEffect; // 스폰 파티클 관련 데이터 연동 시 수정해야 할 부분
    [SerializeField] private string playerCase; // 플레이어 스폰 위치 관련 값, 나중에 플레이어 데이터와 연동시켜야 함 (0 : 튜토리얼, 1 : 광장, 2 : 피카부)
    [SerializeField] private string pumpkin;
    [SerializeField] private string clover;
    [SerializeField] private string flyingStar;
    #endregion

    #region 스폰 포인트
    [Header("튜토리얼 구역 스폰 포인트")]
    [SerializeField] private Transform tutorialBottomLeft;
    [SerializeField] private Transform tutorialTopRight;

    [Header("광장 스폰 포인트")]
    [SerializeField] private Transform plazaBottomLeft;
    [SerializeField] private Transform plazaTopRight;

    [Header("피카부 구역 스폰 포인트")]
    [SerializeField] private Transform peekabooBottomLeft;
    [SerializeField] private Transform peekabooTopRight;
    #endregion

    private void Awake()
    {
        Transform lobbyCameraPosition = SpawnCamera(playerCase);
        SpawnCharacter(lobbyCameraPosition, myCharacterName, mySpawnEffect);

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPumpkin();
            SpawnClover();
            SpawnStar();
        }
    }
    
    public Transform SpawnCamera(string _case)
    {
        Vector3 bottomLeft = transform.position;
        Vector3 topRight = transform.position;

        switch (_case)
        {
            case "0":
                bottomLeft = tutorialBottomLeft.position;
                topRight = tutorialTopRight.position;
                break;

            case "1":
                bottomLeft = plazaBottomLeft.position;
                topRight = plazaTopRight.position;
                break;

            case "2":
                bottomLeft = peekabooBottomLeft.position;
                topRight = peekabooTopRight.position;
                break;
        }

        float positionX = Random.Range(bottomLeft.x, topRight.x);
        float positionY = Random.Range(bottomLeft.y, topRight.y);
        float positionZ = Random.Range(bottomLeft.z, topRight.z);

        Vector3 position = new Vector3(positionX, positionY, positionZ);
        LobbyCamera.transform.position = position;
        return LobbyCamera.transform;
    }

    private void SpawnCharacter(Transform spawnPosition, string _prefabName, string _effectName)
    {
        // 확장성을 위해 프리팹에 스폰 파티클을 넣지 않고, 따로 소환하기로 함
        GameManager.Instance.Player = PhotonNetwork.Instantiate(_prefabName, spawnPosition.position, Quaternion.identity);
        PhotonNetwork.Instantiate(_effectName, spawnPosition.position, Quaternion.identity);
    }

    private void SpawnPumpkin()
    {
        Vector3 position = new Vector3(0f, 0f, 0f);
        PhotonNetwork.Instantiate(pumpkin, position, Quaternion.identity);
    }

    private void SpawnClover()
    {
        Vector3 position = new Vector3(0f, 0f, 0f);
        PhotonNetwork.Instantiate(clover, position, Quaternion.identity);
    }

    private void SpawnStar()
    {
        Vector3 position = new Vector3(0f, 0f, 0f);
        PhotonNetwork.Instantiate(flyingStar, position, Quaternion.identity);
    }
}