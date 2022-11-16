using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverSpawnManager : SingletonBehaviour<CloverSpawnManager>
{
    [SerializeField] private Transform area1;
    private List<Transform> area1List = new List<Transform>();

    [SerializeField] private Transform area2;
    private List<Transform> area2List = new List<Transform>();

    [SerializeField] private Transform CloverParent;
    [SerializeField] private GameObject fourLeafCloverPrefab;
    [SerializeField] private GameObject threeLeafCloverPrefab;

    private int fourLeafCloverSpawnCount;

    [SerializeField] private List<GameObject> fourLeafCloverList = new List<GameObject>();
    [SerializeField] private List<GameObject> threeLeafCloverList = new List<GameObject>();

    private float randomMinValue = -2f;
    private float randomMaxValue = 2f;

    bool isFourLeafCloverRespawn;

    void Awake()
    {
        for (int i = 0; i < area1.childCount; i++)
        {
            area1List.Add(area1.GetChild(i));
        }

        for (int i = 0; i < area2.childCount; i++)
        {
            area2List.Add(area2.GetChild(i));
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        fourLeafCloverSpawnCount = Random.Range(1, fourLeafCloverList.Count +1);

        for (int i = 0; i < fourLeafCloverSpawnCount; i++)
        {
            int area = Random.Range(0, 2);

            float randomX = Random.Range(randomMinValue, randomMaxValue);
            float randomY = Random.Range(randomMinValue, randomMaxValue);

            switch (area)
            {
                case 0:
                    {
                        int areaRoomNumber = Random.Range(0, area1List.Count);
                        fourLeafCloverList[i].GetComponent<CloverInfo>().Area = area;
                        SpawnClovers(fourLeafCloverList[i].transform, area1List[areaRoomNumber]);
                    }
                    break;

                default:
                    {
                        int areaRoomNumber = Random.Range(0, area2List.Count);
                        fourLeafCloverList[i].GetComponent<CloverInfo>().Area = area;
                        SpawnClovers(fourLeafCloverList[i].transform, area2List[areaRoomNumber]);
                    }
                    break;
            }
        }

        for (int i = 0; i < threeLeafCloverList.Count; i++)
        {
            if(i < threeLeafCloverList.Count/2)
            {
                //area1
                int areaRoomNumber = Random.Range(0, area1List.Count);
                threeLeafCloverList[i].GetComponent<CloverInfo>().Area = 0;
                SpawnClovers(threeLeafCloverList[i].transform, area1List[areaRoomNumber]);
            }
            else
            {
                //area2
                int areaRoomNumber = Random.Range(0, area2List.Count);
                threeLeafCloverList[i].GetComponent<CloverInfo>().Area = 1;
                SpawnClovers(threeLeafCloverList[i].transform, area2List[areaRoomNumber]);
            }
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

    public void ReSpawnClover(Transform _clover, int _area)
    {
        // TODO : MasterClient만 실행해야함

        switch (_area)
        {
            case 0:
                {
                    int areaRoomNumber = Random.Range(0, area1List.Count);
                    SpawnClovers(_clover, area1List[areaRoomNumber]);
                }
                break;

            default:
                {
                    int areaRoomNumber = Random.Range(0, area2List.Count);
                    SpawnClovers(_clover, area2List[areaRoomNumber]);
                }
                break;
        }
    }

    public void ReSpawnFourLeafClover()
    {
        // TODO : MasterClient만 실행해야함
        if (isFourLeafCloverRespawn) return;

        for (int i = 0; i < fourLeafCloverList.Count; i++)
        {
            if(fourLeafCloverList[i].activeSelf)
            {
                return;
            }
        }
        isFourLeafCloverRespawn = true;
        StartCoroutine("RespawnFourLeafCloverCoroutine");
    }

    IEnumerator RespawnFourLeafCloverCoroutine()
    {
        yield return new WaitForSecondsRealtime(2400);
        fourLeafCloverSpawnCount = Random.Range(0, fourLeafCloverSpawnCount);

        for (int i = 0; i < fourLeafCloverSpawnCount; i++)
        {
            int area = Random.Range(0, 2);

            switch (area)
            {
                case 0:
                    {
                        int areaRoomNumber = Random.Range(0, area1List.Count);
                        fourLeafCloverList[i].GetComponent<CloverInfo>().Area = area;
                        SpawnClovers(fourLeafCloverList[i].transform, area1List[areaRoomNumber]);
                    }
                    break;

                default:
                    {
                        int areaRoomNumber = Random.Range(0, area2List.Count);
                        fourLeafCloverList[i].GetComponent<CloverInfo>().Area = area;
                        SpawnClovers(fourLeafCloverList[i].transform, area2List[areaRoomNumber]);
                    }
                    break;
            }
        }
        isFourLeafCloverRespawn = false;
        StopCoroutine("RelocationFourLeafCloverTransformCoroutine");
        StartCoroutine("RelocationFourLeafCloverTransformCoroutine");
    }

    IEnumerator RelocationFourLeafCloverTransformCoroutine()
    {
        yield return new WaitForSecondsRealtime(1200);
        for (int i = 0; i < fourLeafCloverList.Count; i++)
        {
            if (fourLeafCloverList[i].activeSelf)
            {
                int area = Random.Range(0, 2);

                switch (area)
                {
                    case 0:
                        {
                            int areaRoomNumber = Random.Range(0, area1List.Count);
                            SpawnClovers(fourLeafCloverList[i].transform, area1List[areaRoomNumber]);
                        }
                        break;

                    default:
                        {
                            int areaRoomNumber = Random.Range(0, area2List.Count);
                            SpawnClovers(fourLeafCloverList[i].transform, area2List[areaRoomNumber]);
                        }
                        break;
                }
            }
        }
    }

    // 네잎 클로버 뽑아 사라졌을 때 호출하여 확인 진행
    public void CheckFourLeafCloverActiveSelf()
    {
        for (int i = 0; i < fourLeafCloverList.Count; i++)
        {
            if(fourLeafCloverList[i].activeSelf)
            {
                return;
            }
        }
        ReSpawnFourLeafClover();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // ToDo : 연산 엄청 먹겠다
        {
            for (int i = 0; i < fourLeafCloverList.Count; i++)
            {
                fourLeafCloverList[i].GetComponent<Collider>().isTrigger = true;
            }
            for (int i = 0; i < threeLeafCloverList.Count; i++)
            {
                threeLeafCloverList[i].GetComponent<Collider>().isTrigger = true;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // ToDo : 연산 엄청 먹겠다
        {
            for (int i = 0; i < fourLeafCloverList.Count; i++)
            {
                fourLeafCloverList[i].GetComponent<Collider>().isTrigger = false;
            }
            for (int i = 0; i < threeLeafCloverList.Count; i++)
            {
                threeLeafCloverList[i].GetComponent<Collider>().isTrigger = false;
            }
        }
        else if (other.gameObject.CompareTag("ThreeLeafClover") || other.gameObject.CompareTag("FourLeafClover"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
