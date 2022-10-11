using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    private int mapSize;
    [SerializeField]
    private GameObject mapPrefab;
    [SerializeField]
    private Spwner spwner;

    //private int[] map;
    //public int[] Map { get { return Map; } }



    private void Awake()
    {
        createMap(mapSize);
    }

    private void createMap(int _mapsize)
    {
        for (int x = 0; x < _mapsize; x++)
        {
            for (int y = 0; y < _mapsize; y++)
            {
                Vector3 _mapPosition = new Vector3(x * 50, 0, y * 50);
                Instantiate(mapPrefab,_mapPosition,Quaternion.identity);
                spwner.firstSpawn(_mapPosition);
            }
        }
    }
}
