using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerSpawningState : LobbyPlayerState
{
    [SerializeField]
    private float fadeInTime;

    private Color myColor;
    private float increaseValue;

    public override void OnEnter()
    {
        myColor = myFSM.myRenderer.material.color;
        // Debug.Log(myColor);
        increaseValue = 1 / fadeInTime;
    }

    public override void OnUpdate()
    {
        myColor.a += increaseValue * Time.deltaTime;
        myFSM.myRenderer.material.color = myColor;
        // Debug.Log(myFSM.myRenderer.material.color.a);

        if (1f <= myFSM.myRenderer.material.color.a)
        {
            myFSM.ChangeState(LOBBYPLAYERSTATE.IDLE);
            // Debug.Log("페이드 인 완료!");
        }
    }

    public override void OnExit()
    {
        myColor.a = 1f;
        myFSM.myRenderer.material.color = myColor;
        myFSM.myRenderer.material = myFSM.MyOpaqueMaterial;
    }
}