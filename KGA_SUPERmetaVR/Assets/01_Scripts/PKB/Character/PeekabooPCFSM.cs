using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOOPCANIMATIONSTATE
{
    IDLE,
    SUPRISED,
    PEEKABOO,
    FLOP,
    LOSINGSOUL,
}

public class PeekabooPCFSM : PeekabooCharacterFSM
{
    private PeekabooPCRotateToAttackerState PCRotateToAttackerState;
    private PeekabooPCSuprisedState PCSuprisedState;
    private PeekabooPCRotateToTargetState PCRotateToTargetState;
    private PeekabooPCPeekabooState PCPeekabooState;
    private PeekabooPCFlopState PCFlopState;
    private PeekabooPCLosingSoulState PCLosingSoulState;
    private PeekabooPCDieState PCDieState;

    private void Awake()
    {
        BaseInitialize();
    }

    protected override void Initialize()
    {
        PCRotateToAttackerState = GetComponent<PeekabooPCRotateToAttackerState>();
        PCSuprisedState = GetComponent<PeekabooPCSuprisedState>();
        PCRotateToTargetState = GetComponent<PeekabooPCRotateToTargetState>();
        PCPeekabooState = GetComponent<PeekabooPCPeekabooState>();
        PCFlopState = GetComponent<PeekabooPCFlopState>();
        PCLosingSoulState = GetComponent<PeekabooPCLosingSoulState>();
        PCDieState = GetComponent<PeekabooPCDieState>();

        AddState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER, PCRotateToAttackerState);
        AddState(PEEKABOOCHARACTERSTATE.PCSUPRISED, PCSuprisedState);
        AddState(PEEKABOOCHARACTERSTATE.PCROTATETOTARGET, PCRotateToTargetState);
        AddState(PEEKABOOCHARACTERSTATE.PCPEEKABOO, PCPeekabooState);
        AddState(PEEKABOOCHARACTERSTATE.PCFLOP, PCFlopState);
        AddState(PEEKABOOCHARACTERSTATE.PCLOSINGSOUL, PCLosingSoulState);
        AddState(PEEKABOOCHARACTERSTATE.PCDIE, PCDieState);
    }
}