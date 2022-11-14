using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOONPCANIMATIONSTATE
{
    IDLE,
    LAUGHT,
    PEEKABOO,
}

public class PeekabooNPCFSM : PeekabooCharacterFSM
{
    private PeekabooNPCLaughtState NPCLaughtState;
    private PeekabooNPCRotateToAttackerState NPCRotateToAttackerState;
    private PeekabooNPCPeekabooState NPCPeekabooState;
    private PeekabooNPCDieState NPCDieState;

    private void Awake()
    {
        BaseInitialize();
    }

    protected override void Initialize()
    {
        NPCLaughtState = GetComponent<PeekabooNPCLaughtState>();
        NPCRotateToAttackerState = GetComponent<PeekabooNPCRotateToAttackerState>();
        NPCPeekabooState = GetComponent<PeekabooNPCPeekabooState>();
        NPCDieState = GetComponent<PeekabooNPCDieState>();

        AddState(PEEKABOOCHARACTERSTATE.NPCLAUGHT, NPCLaughtState);
        AddState(PEEKABOOCHARACTERSTATE.NPCROTATETOATTACKER, NPCRotateToAttackerState);
        AddState(PEEKABOOCHARACTERSTATE.NPCPEEKABOO, NPCPeekabooState);
        AddState(PEEKABOOCHARACTERSTATE.NPCDIE, NPCDieState);
    }

    private void Update()
    {
        nowState.OnUpdate();
    }
}