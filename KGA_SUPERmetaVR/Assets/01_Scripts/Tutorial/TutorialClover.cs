using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialClover : MonoBehaviour
{
    [SerializeField] private Transform area;

    private int fourLeafCloverSpawnCount;

    [SerializeField] private List<GameObject> fourLeafCloverList = new List<GameObject>();

    private float randomMinValue = -1f;
    private float randomMaxValue = 1f;

    bool isFourLeafCloverRespawn;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        fourLeafCloverSpawnCount = Random.Range(1, fourLeafCloverList.Count + 1);

        for (int i = 0; i < fourLeafCloverSpawnCount; i++)
        {
            float randomX = Random.Range(randomMinValue, randomMaxValue);
            float randomY = Random.Range(randomMinValue, randomMaxValue);

            SpawnClovers(fourLeafCloverList[i].transform, area);
        }
    }

    public void SpawnClovers(Transform _clover, Transform _areaRoom)
    {
        float randomX = Random.Range(randomMinValue, randomMaxValue);
        float randomY = Random.Range(randomMinValue, randomMaxValue);

        int randomSize = Random.Range(0, 3);
        switch (randomSize)
        {
            case 0:
                _clover.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                _clover.transform.position = _areaRoom.position - new Vector3(randomX, -0.75f, randomY);
                break;
            case 1:
                _clover.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                _clover.transform.position = _areaRoom.position - new Vector3(randomX, -0.6f, randomY);
                break;
            default:
                _clover.localScale = new Vector3(1, 1, 1);
                _clover.transform.position = _areaRoom.position - new Vector3(randomX, -0.5f, randomY);
                break;
        }
        int randomRotation = Random.Range(0, 360);

        _clover.localRotation = Quaternion.Euler(-90, randomRotation, 0);
        _clover.gameObject.SetActive(true);
    }

    public void ReSpawnClover(Transform _clover)
    {
        // TODO : MasterClient만 실행해야함
        SpawnClovers(_clover, area);
    }

    public void ReSpawnFourLeafClover()
    {
        // TODO : MasterClient만 실행해야함
        if (isFourLeafCloverRespawn) return;

        for (int i = 0; i < fourLeafCloverList.Count; i++)
        {
            if (fourLeafCloverList[i].activeSelf)
            {
                return;
            }
        }
        isFourLeafCloverRespawn = true;
        StartCoroutine("RespawnFourLeafCloverCoroutine");
    }

    IEnumerator RespawnFourLeafCloverCoroutine()
    {
        fourLeafCloverSpawnCount = Random.Range(0, fourLeafCloverSpawnCount);

        for (int i = 0; i < fourLeafCloverSpawnCount; i++)
        {
            SpawnClovers(fourLeafCloverList[i].transform, area);
        }

        isFourLeafCloverRespawn = false;
        yield break;
    }

    // 네잎 클로버 뽑아 사라졌을 때 호출하여 확인 진행
    public void CheckFourLeafCloverActiveSelf()
    {
        for (int i = 0; i < fourLeafCloverList.Count; i++)
        {
            if (fourLeafCloverList[i].activeSelf)
            {
                return;
            }
        }
        ReSpawnFourLeafClover();
    }
}
