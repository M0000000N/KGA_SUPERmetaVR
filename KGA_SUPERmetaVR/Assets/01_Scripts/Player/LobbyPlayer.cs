using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    private LobbyPlayerFSM myFSM;

    private void Awake()
    {
        myFSM = GetComponent<LobbyPlayerFSM>();
    }

    private void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x , gameObject.transform.position.y -1.2f, gameObject.transform.position.z);
    }

    private void Update()
    {
        myFSM.UpdateFSM();
    }
}