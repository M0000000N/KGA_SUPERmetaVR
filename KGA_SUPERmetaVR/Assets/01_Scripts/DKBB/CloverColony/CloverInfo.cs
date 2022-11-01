using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverInfo : MonoBehaviour
{
    [SerializeField] private bool isFourLeaf;
    public bool IsFourLeaf { get { return isFourLeaf; } set { isFourLeaf = value; } }

    [SerializeField] private int area;
    public int Area { get { return area; } set { area = value; } }
}
