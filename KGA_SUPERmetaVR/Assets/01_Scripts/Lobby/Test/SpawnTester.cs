using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTester : MonoBehaviour
{
    [SerializeField]
    private LobbySpawner mySpawner;
    [SerializeField]
    private string testCase;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            mySpawner.SpawnCamera(testCase);
        }
    }
}