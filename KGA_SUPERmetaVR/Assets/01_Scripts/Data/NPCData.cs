using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : SingletonBehaviour<NPCData>
{
    [SerializeField]
    public static NPCData[] Data { get; private set; }

    public static NPCData GetData(int _id)
    {
        for (int i = 0; i < Data.Length; i++)
        {
            if (Data[i].ID == _id)
            {
                return Data[i];
            }
        }

        return null;
    }
}