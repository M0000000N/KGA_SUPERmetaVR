using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOONPCSTATE
{
    IDLE,
    LOOKINGLEFT,
    LOOKINGRIGHT,
    TAKEDAMAGE,
}

public class PeekabooNPCFSM : MonoBehaviour
{
    public PeekabooNPCState nowState { get; private set; }
    public PEEKABOONPCSTATE nowStateKey { get; private set; }
    public Animator MyAnimator { get; private set; }
    public GameObject counterAttackTarget { get; private set; }
    public PeekabooNPC myNPC { get; private set; }

    private Dictionary<PEEKABOONPCSTATE, PeekabooNPCState> myStates;
    private PeekabooNPCIdleState idleState;
    private PeekabooNPCLookingLeftState lookingLeftSideState;
    private PeekabooNPCLookingRightState lookingRightSideState;
    private PeekabooNPCTakeDamageState takeDamageState;

    private void Awake()
    {
        myNPC = GetComponent<PeekabooNPC>();
        myStates = new Dictionary<PEEKABOONPCSTATE, PeekabooNPCState>();
        idleState = GetComponent<PeekabooNPCIdleState>();
        lookingLeftSideState = GetComponent<PeekabooNPCLookingLeftState>();
        lookingRightSideState = GetComponent<PeekabooNPCLookingRightState>();
        takeDamageState = GetComponent<PeekabooNPCTakeDamageState>();
        MyAnimator = GetComponent<Animator>();

        AddState(PEEKABOONPCSTATE.IDLE, idleState);
        AddState(PEEKABOONPCSTATE.LOOKINGLEFT, lookingLeftSideState);
        AddState(PEEKABOONPCSTATE.LOOKINGRIGHT, lookingRightSideState);
        AddState(PEEKABOONPCSTATE.TAKEDAMAGE, takeDamageState);

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
        nowStateKey = _stateKey;
        nowState.OnEnter();
    }

    public void UpdateFSM()
    {
        nowState.OnUpdate();
    }

    public void SetCounterAttackTarget(GameObject _attaker)
    {
        counterAttackTarget = _attaker;
    }
}