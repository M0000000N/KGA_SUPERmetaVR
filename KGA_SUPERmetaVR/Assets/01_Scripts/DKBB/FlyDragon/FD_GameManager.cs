using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FD_GameManager : OnlyOneSceneSingleton<FD_GameManager>
{
    public bool IsCoolTime;

    [SerializeField] private float SpawnMinValue;
    [SerializeField] private float SpawnMaxValue;

    private int spawnCount;

    // private List<>

    void Start()
    {
        IsCoolTime = false;
        spawnCount = 10;
    }

    public void Initialize()
    {

    }

    public void SpawnObject(Transform _target)
    {
        float randomX = Random.Range(SpawnMinValue, SpawnMaxValue);
        float randomY = Random.Range(SpawnMinValue, SpawnMaxValue);

        _target.position = -new Vector3(randomX, 0, randomY);
        _target.rotation = Quaternion.identity;

        _target.gameObject.SetActive(true);
    }

    public void CheckDestroyObject()
    {

    }

    IEnumerator DestroyObject(GameObject _target, float _time)
    {
        yield return new WaitForSeconds(_time);
        if (_target.CompareTag("PaperSwan"))
        {
            _target.SetActive(false);
            StopCoroutine("ResultMessage");
            StartCoroutine("ResultMessage");
        }
        // isCoroutine = false;
    }

    IEnumerator ResultMessage()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(600f);
            if (PaperSwanDataBase.Instance.CheckCooltime(2))
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
