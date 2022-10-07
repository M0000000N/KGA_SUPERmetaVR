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
    private DummyEnemy CreateNewObject(Transform transform)
    {
        var newobj = Instantiate(poolingObjectPrefab, transform).GetComponent<DummyEnemy>();
        newobj.gameObject.SetActive(false);
        return newobj;
    }

    private void Initalize(int count, Transform transform)
    {
        for (int i = 0; i < count; ++i)
        {
            poolingObjectQueue.Enqueue(CreateNewObject(transform));
        }
    }

    public static DummyEnemy GetObject(Transform transform)
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
            var newobj = Instance.CreateNewObject(transform);
            newobj.transform.SetParent(null);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }

    public static void ReturnObject(DummyEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(enemy);
    }
}
