using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOONPCSTATE
{
    IDLE,
    LOOKINGLEFTSIDE,
    LOOKINGRIGHTSIDE,
    TAKEDAMAGE,
}

public class PeekabooNPCFSM : MonoBehaviour
{
    public PeekabooNPCData getMyData { get { return myData; } }
    public PeekabooNPCState nowState { get; private set; }
    public Animator MyAnimator { get; private set; }
    public GameObject counterAttackTarget { get; private set; }

    [SerializeField]
    private PeekabooNPCData myData;

    private PeekabooNPC myNPC;
    private Dictionary<PEEKABOONPCSTATE, PeekabooNPCState> myStates;
    private PeekabooNPCIdleState idleState;
    private PeekabooNPCLookingLeftSideState lookingLeftSideState;
    private PeekabooNPCLookingRightSideState lookingRightSideState;
    private PeekabooNPCTakeDamageState takeDamageState;

    private void Awake()
    {
        myNPC = GetComponent<PeekabooNPC>();
        myStates = new Dictionary<PEEKABOONPCSTATE, PeekabooNPCState>();
        idleState = GetComponent<PeekabooNPCIdleState>();
        lookingLeftSideState = GetComponent<PeekabooNPCLookingLeftSideState>();
        lookingRightSideState = GetComponent<PeekabooNPCLookingRightSideState>();
        takeDamageState = GetComponent<PeekabooNPCTakeDamageState>();
        MyAnimator = GetComponent<Animator>();

        AddState(PEEKABOONPCSTATE.IDLE, idleState);
        AddState(PEEKABOONPCSTATE.LOOKINGLEFTSIDE, lookingLeftSideState);
        AddState(PEEKABOONPCSTATE.LOOKINGRIGHTSIDE, lookingRightSideState);
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