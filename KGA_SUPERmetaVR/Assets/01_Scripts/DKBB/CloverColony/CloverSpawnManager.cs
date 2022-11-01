using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform area1;
    private List<Transform> area1List = new List<Transform>();

    [SerializeField] private Transform area2;
    private List<Transform> area2List = new List<Transform>();

    [SerializeField] private Transform CloverParent;
    [SerializeField] private GameObject fourLeafCloverPrefab;
    [SerializeField] private GameObject threeLeafCloverPrefab;

    [SerializeField] private int fourLeafCloverMaxCount = 4;
    [SerializeField] private int threeLeafCloverMaxCount = 80;

    private int fourLeafCloverSpawnCount;

    private List<GameObject> fourLeafCloverList = new List<GameObject>();
    private List<GameObject> threeLeafCloverList = new List<GameObject>();

    private float randomMinValue = -2f;
    private float randomMaxValue = 2f;

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

        for (int i = 0; i < fourLeafCloverMaxCount; i++)
        {
            GameObject clover = Instantiate(fourLeafCloverPrefab, CloverParent);
            clover.GetComponent<CloverInfo>().IsFourLeaf = true;
            fourLeafCloverList.Add(clover);
        }

        for (int i = 0; i < threeLeafCloverMaxCount; i++)
        {
            GameObject clover = Instantiate(threeLeafCloverPrefab, CloverParent);
            clover.GetComponent<CloverInfo>().IsFourLeaf = false;
            threeLeafCloverList.Add(clover);
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        fourLeafCloverSpawnCount = Random.Range(1, fourLeafCloverMaxCount + 1);

        for (int i = 0; i < fourLeafCloverList.Count; i++)
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

        _clover.transform.position = _areaRoom.position - new Vector3(randomX, -0.5f, randomY);
        _clover.gameObject.SetActive(true);
    }

    public void ReSpawnClover(Transform _clover, int _area)
    {
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

    public void PickUpClover(GameObject _clover)
    {
        if(_clover.GetComponent<CloverInfo>().IsFourLeaf)
        {

        }
        else
        {
            StartCoroutine("PickUpThreeLeafClover", _clover);
        }
    }

    IEnumerator PickUpThreeLeafClover(GameObject _clover)
    {
        yield return new WaitForSeconds(2);
        // TODO : 사라지는 연출이 있다면 추가 필요
        // TODO : 잡은 판정이 있다면 취소가 필요
        ReSpawnClover(_clover.transform, _clover.GetComponent<CloverInfo>().Area);
    }

    void Update()
    {
        
    }
}
