using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooEnemyObjectPool : MonoBehaviour
{
    private static PeekabooEnemyObjectPool instance;

    // LSI - NPC 동기화 테스트용
    #region
    [SerializeField]
    private InteractTester tester;
    #endregion
    // -----------------------------------

    [SerializeField]
    private GameObject poolingObjectPrefab;

    private Queue<PeekabooNPC> poolingObjectQueue = new Queue<PeekabooNPC>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }
    private PeekabooNPC CreateNewObject(Transform _transform)
    {
        PeekabooNPC newObject = PhotonNetwork.Instantiate(poolingObjectPrefab.name, _transform.position, Quaternion.identity).GetComponent<PeekabooNPC>();
        tester.NPCs.Add(newObject.gameObject);
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    private void Initalize(int _count, Transform _transform)
    {
        for (int i = 0; i < _count; ++i)
        {
            poolingObjectQueue.Enqueue(CreateNewObject(_transform));
        }
    }

    public static PeekabooNPC GetObject(Transform _transform)
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

    public static void ReturnObject(PeekabooNPC _enemy)
    {
        _enemy.gameObject.SetActive(false);
        _enemy.transform.SetParent(instance.transform);
        instance.poolingObjectQueue.Enqueue(_enemy);
    }
}
