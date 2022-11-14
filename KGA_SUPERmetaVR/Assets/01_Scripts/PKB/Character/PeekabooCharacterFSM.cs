using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PEEKABOOCHARACTERSTATE
{
    IDLE,
    LOOKINGSOMEONE,
    FRONTTOLEFT,
    LOOKINGLEFT,
    LEFTTOFRONT,
    FRONTTORIGHT,
    LOOKINGRIGHT,
    RIGHTTOFRONT,
    NPCLAUGHT,
    NPCROTATETOATTACKER,
    NPCPEEKABOO,
    NPCDIE,
    PCROTATETOATTACKER,
    PCSUPRISED,
    PCROTATETOTARGET,
    PCPEEKABOO,
    PCFLOP,
    PCLOSINGSOUL,
    PCDIE,
}

public abstract class PeekabooCharacterFSM : MonoBehaviourPun
{
    public float ViewAngleHalf { get; private set; }
    public Quaternion MyBodyRotation { get { return myBody.transform.rotation; } }
    public Animator MyAnimator { get; private set; }
    public PeekabooCharacter MyCharacter { get; private set; }

    [SerializeField]
    protected GameObject myBody;

    protected Dictionary<PEEKABOOCHARACTERSTATE, PeekabooCharacterState> myStates;
    protected PeekabooCharacterState nowState;

    protected PeekabooCharacterIdleState idleState;
    protected PeekabooCharacterLookingSomeoneState lookingSomeoneState;
    protected PeekabooCharacterFrontToLeftState frontToLeftState;
    protected PeekabooCharacterLookingLeftState lookingLeftState;
    protected PeekabooCharacterLeftToFrontState leftToFrontState;
    protected PeekabooCharacterFrontToRightState frontToRightState;
    protected PeekabooCharacterLookingRightState lookingRightState;
    protected PeekabooCharacterRightToFrontState rightToFrontState;

    protected Action<PEEKABOOCHARACTERSTATE> myAction;

    protected void BaseInitialize()
    {
        ViewAngleHalf = 70f;
        MyAnimator = GetComponent<Animator>();
        MyCharacter = GetComponent<PeekabooCharacter>();

        myStates = new Dictionary<PEEKABOOCHARACTERSTATE, PeekabooCharacterState>();
        nowState = idleState;

        idleState = GetComponent<PeekabooCharacterIdleState>();
        lookingSomeoneState = GetComponent<PeekabooCharacterLookingSomeoneState>();
        frontToLeftState = GetComponent<PeekabooCharacterFrontToLeftState>();
        lookingLeftState = GetComponent<PeekabooCharacterLookingLeftState>();
        leftToFrontState = GetComponent<PeekabooCharacterLeftToFrontState>();
        frontToRightState = GetComponent<PeekabooCharacterFrontToRightState>();
        lookingRightState = GetComponent<PeekabooCharacterLookingRightState>();
        rightToFrontState = GetComponent<PeekabooCharacterRightToFrontState>();

        AddState(PEEKABOOCHARACTERSTATE.IDLE, idleState);
        AddState(PEEKABOOCHARACTERSTATE.LOOKINGSOMEONE, lookingSomeoneState);
        AddState(PEEKABOOCHARACTERSTATE.FRONTTOLEFT, frontToLeftState);
        AddState(PEEKABOOCHARACTERSTATE.LOOKINGLEFT, lookingLeftState);
        AddState(PEEKABOOCHARACTERSTATE.LEFTTOFRONT, leftToFrontState);
        AddState(PEEKABOOCHARACTERSTATE.FRONTTORIGHT, frontToRightState);
        AddState(PEEKABOOCHARACTERSTATE.LOOKINGRIGHT, lookingRightState);
        AddState(PEEKABOOCHARACTERSTATE.RIGHTTOFRONT, rightToFrontState);

        nowState = idleState;
        ChangeState(PEEKABOOCHARACTERSTATE.IDLE);

        myAction = ChangeState;

        Initialize();
    }

    protected abstract void Initialize();

    // Character에서 Update가 호출될 때마다 실행 할 함수
    public void UpdateFSM()
    {
        nowState.OnUpdate();
    }

    public void AddState(PEEKABOOCHARACTERSTATE _stateKey, PeekabooCharacterState _state)
    {
        _state.Initialize(this);
        myStates[_stateKey] = _state;
    }

    public void ChangeState(PEEKABOOCHARACTERSTATE _stateKey)
    {
        nowState.OnExit();
        nowState = myStates[_stateKey];
        nowState.OnEnter();
    }

    public IEnumerator RotateCoroutine(Quaternion _targetQuaternion, float _rotateTime, PEEKABOOCHARACTERSTATE _stateKey)
    {
        Quaternion initialQuaternion = transform.rotation;
        float elapsedTimeInCoroutine = 0f;

        while (elapsedTimeInCoroutine <= _rotateTime)
        {
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(initialQuaternion, _targetQuaternion, elapsedTimeInCoroutine * _rotateTime);

            yield return null;
        }

        transform.localRotation = _targetQuaternion;

        myAction(_stateKey);
    }

    public IEnumerator WaitForNextBehaviourCoroutine(float _waitTime, PEEKABOOCHARACTERSTATE _stateKey)
    {
        yield return new WaitForSeconds(_waitTime);

        myAction(_stateKey);
    }
}