using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooEnemyObjectPool : MonoBehaviour
{
    private static PeekabooEnemyObjectPool instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;
    [SerializeField]
    private PeekabooSpawner spwner;

    private Queue<DummyEnemy> poolingObjectQueue = new Queue<DummyEnemy>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //(spwner.NPCCount, gameObject.transform);
    }
    private DummyEnemy CreateNewObject(Transform _transform)
    {
        
        var newobj = PhotonNetwork.Instantiate(poolingObjectPrefab.name, _transform.position,Quaternion.identity).GetComponent<DummyEnemy>();
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
        if (instance.poolingObjectQueue.Count > 0)
        {
            var obj = instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newobj = instance.CreateNewObject(_transform);
            newobj.transform.SetParent(null);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }

    public static void ReturnObject(DummyEnemy _enemy)
    {
        _enemy.gameObject.SetActive(false);
        _enemy.transform.SetParent(instance.transform);
        instance.poolingObjectQueue.Enqueue(_enemy);
    }
}
