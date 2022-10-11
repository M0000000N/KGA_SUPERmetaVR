using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOONPCSTATE
{
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

    private void Awake()
    {

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
}