using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    private int mapSize;
    public int MapSize { get { return mapSize; } }
    [SerializeField]
    private GameObject mapPrefab;
    [SerializeField]
    private Spwner spwner;

    [SerializeField]
    private float mapLength;
    public float MapLength { get { return mapLength; } }

    private List<Vector3> map;
    public List<Vector3> Map { get { return map; } }



    private void Awake()
    {
        map = new List<Vector3>();
        CreateMaps(mapSize);
    }

    private void CreateMaps(int _mapsize)
    {
        for (int x = 0; x < _mapsize; x++)
        {
            for (int z = 0; z < _mapsize; z++)
            {
                Vector3 _mapPosition = new Vector3(x * 25, 0, z * 25);
                PhotonNetwork.Instantiate(mapPrefab.name,_mapPosition,Quaternion.identity);
                spwner.FirstSpawn(_mapPosition);
                map.Add(_mapPosition);
                map.Add(_mapPosition);
            }
        }
    }
}
