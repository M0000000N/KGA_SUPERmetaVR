using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    private static EnemyObjectPool Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;
    [SerializeField]
    private Spwner spwner;

    private Queue<DummyEnemy> poolingObjectQueue = new Queue<DummyEnemy>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initalize(spwner.NPCCount, gameObject.transform);
    }
    private DummyEnemy CreateNewObject(Transform _transform)
    {
        var newobj = Instantiate(poolingObjectPrefab, _transform).GetComponent<DummyEnemy>();
        newobj.gameObject.SetActive(false);
        return newobj;
    }

    private void Initalize(int _count, Transform _transform)
    {
        for (int i = 0; i < _count; ++i)
        {
            poolingObjectQueue.Enqueue(CreateNewObject(_transform));
        }
    }

    public static DummyEnemy GetObject(Transform _transform)
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newobj = Instance.CreateNewObject(_transform);
            newobj.transform.SetParent(null);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }

    public static void ReturnObject(DummyEnemy _enemy)
    {
        _enemy.gameObject.SetActive(false);
        _enemy.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(_enemy);
    }
}
