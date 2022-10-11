using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOONPCSTATE
{
    IDLE,
    LOOKINGLEFTSIDE,
    LOOKINGRIGHTSIDE,
}

public class PeekabooNPCFSM : MonoBehaviour
{
    public PeekabooNPCData getMyData { get { return myData; } }
    public PeekabooNPCState nowState { get; private set; }

    [SerializeField]
    private PeekabooNPCData myData;

    private Dictionary<PEEKABOONPCSTATE, PeekabooNPCState> myStates;
    private PeekabooNPCIdleState idleState;
    private PeekabooNPCLookingLeftSideState lookingLeftSideState;
    private PeekabooNPCLookingRightSideState lookingRightSideState;

    private void Awake()
    {
        myStates = new Dictionary<PEEKABOONPCSTATE, PeekabooNPCState>();
        idleState = GetComponent<PeekabooNPCIdleState>();
        lookingLeftSideState = GetComponent<PeekabooNPCLookingLeftSideState>();
        lookingRightSideState = GetComponent<PeekabooNPCLookingRightSideState>();

        AddState(PEEKABOONPCSTATE.IDLE, idleState);
        AddState(PEEKABOONPCSTATE.LOOKINGLEFTSIDE, lookingLeftSideState);
        AddState(PEEKABOONPCSTATE.LOOKINGRIGHTSIDE, lookingRightSideState);

        ChangeState(PEEKABOONPCSTATE.IDLE);
    }

    public void AddState(PEEKABOONPCSTATE _stateKey, PeekabooNPCState _state)
    {
        _state.Initialize(this);
        myStates[_stateKey] = _state;
    }

    public void ChangeState(PEEKABOONPCSTATE _stateKey)
    {
        if (nowState != null)
        {
            nowState.OnExit();
        }
        nowState = myStates[_stateKey];
        nowState.OnEnter();
    }

    public void UpdateFSM()
    {
        nowState.OnUpdate();
    }
}