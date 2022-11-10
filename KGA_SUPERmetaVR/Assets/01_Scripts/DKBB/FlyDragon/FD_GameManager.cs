using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FD_GameManager : OnlyOneSceneSingleton<FD_GameManager>
{
    [SerializeField] private Transform[] area;
    [SerializeField] private FD_Dragon[] dragon;

    void Awake()
    {
        Initialize();
        StartCoroutine(RespawnCoroutine());
    }

    public void Initialize()
    {
        for (int i = 0; i < dragon.Length; i++)
        {
            SpawnObject(dragon[i].transform);
        }
    }

    public void SpawnObject(Transform _target)
    {
        FD_Area spawnArea;
        Vector3 spawnPosition;

        do
        {
            int randomArea = Random.Range(0, area.Length);
            spawnArea = area[randomArea].GetComponent<FD_Area>();

            float randomX = Random.Range(-1, 2);
            float randomY = Random.Range(-1, 2);

            spawnPosition = new Vector3(randomX, 0, randomY);
        }
        while (CheckSpawnArea(spawnArea, spawnPosition) == false);

        spawnArea.SpawnPosition.Add(spawnPosition);

        int randomRotation = Random.Range(0, 360);


        _target.position = spawnArea.transform.position - spawnPosition;
        _target.rotation = Quaternion.Euler(0, randomRotation, 0);

        _target.gameObject.SetActive(true);
    }

    public bool CheckSpawnArea(FD_Area _spawnArea, Vector3 _position)
    {
        for (int i = 0; i < _spawnArea.SpawnPosition.Count; i++)
        {
            if (_spawnArea.SpawnPosition[i] == _position)
            {
                return false;
            }
        }
        return true;
    }


    public void DestroyObject(GameObject _target, float _time)
    {
        if (_target.CompareTag("PaperSwan"))
        {
            _target.SetActive(false);
            // 쿨타임으로 2번 이상 진행이 불가능하지만 Stop을 추가하여 잘 못  
            StopCoroutine(ResultMessageCoroutine());
            StartCoroutine(ResultMessageCoroutine());

            FlyDragonDataBase.Instance.UpdatePlayData();
        }
        // isCoroutine = false;
    }

    IEnumerator RespawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(5f);//10800);
            Initialize();
        }
    }

    IEnumerator ResultMessageCoroutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(600f);
            if (FlyDragonDataBase.Instance.CheckCooltime(2))
            {
                UnityEngine.Debug.Log("메시지 출력");
                break;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PaperSwan"))
        {
            other.gameObject.SetActive(false);
        }
    }

}
