using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FD_Area : MonoBehaviour
{
    private List<Vector3> spawnPosition = new List<Vector3>();
    public List<Vector3> SpawnPosition { get { return spawnPosition; } set { spawnPosition = value; } }
}
