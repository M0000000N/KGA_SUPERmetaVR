using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbySpawner : MonoBehaviourPun
{
    [SerializeField] private GameObject LobbyCamera;
    #region ���߿� ���� �� �κ�
    [SerializeField] private string myCharacterName; // �÷��̾� ���� ������ ���� ���� �� �����ؾ� �� �κ�
    [SerializeField] private string mySpawnEffect; // ���� ��ƼŬ ���� ������ ���� �� �����ؾ� �� �κ�
    [SerializeField] private string playerCase; // �÷��̾� ���� ��ġ ���� ��, ���߿� �÷��̾� �����Ϳ� �������Ѿ� �� (0 : Ʃ�丮��, 1 : ����, 2 : ��ī��)
    [SerializeField] private string pumpkin;
    [SerializeField] private string clover;
    [SerializeField] private string flyingStar;
    #endregion

    #region ���� ����Ʈ
    [Header("Ʃ�丮�� ���� ���� ����Ʈ")]
    [SerializeField] private Transform tutorialBottomLeft;
    [SerializeField] private Transform tutorialTopRight;

    [Header("���� ���� ����Ʈ")]
    [SerializeField] private Transform plazaBottomLeft;
    [SerializeField] private Transform plazaTopRight;

    [Header("��ī�� ���� ���� ����Ʈ")]
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
        // Ȯ�强�� ���� �����տ� ���� ��ƼŬ�� ���� �ʰ�, ���� ��ȯ�ϱ�� ��
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