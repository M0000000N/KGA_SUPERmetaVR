using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PEEKABOOCHARACTERSTATE
{
    IDLE,
    ROTATETOSOMEONE,
    LOOKINGSOMEONE,
    FRONTTOLEFT,
    LOOKINGLEFT,
    FRONTTORIGHT,
    LOOKINGRIGHT,
    ROTATETOFRONT,
    NPCLAUGHT,
    NPCROTATETOATTACKER,
    NPCPEEKABOO,
    NPCDIE,
    PCSUPRISED,
    PCPEEKABOO,
    PCFLOP,
    PCLOSINGSOUL,
}

public abstract class PeekabooCharacterFSM : MonoBehaviourPun
{
    public float ViewAngleHalf { get; private set; }
    public Vector3 ForwardDirectionOfBody { get { return myBody.transform.forward; } }
    public Animator MyAnimator { get; private set; }
    public PeekabooCharacter MyCharacter { get; private set; }

    [SerializeField]
    protected GameObject myBody;

    protected Dictionary<PEEKABOOCHARACTERSTATE, PeekabooCharacterState> myStates;
    protected PeekabooCharacterState nowState;

    protected PeekabooCharacterIdleState idleState;
    protected PeekabooCharacterRotateToSomeoneState rotateToSomeoneState;
    protected PeekabooCharacterLookingSomeoneState lookingSomeoneState;
    protected PeekabooCharacterFrontToLeftState frontToLeftState;
    protected PeekabooCharacterLookingLeftState lookingLeftState;
    protected PeekabooCharacterFrontToRightState frontToRightState;
    protected PeekabooCharacterLookingRightState lookingRightState;
    protected PeekabooCharacterRotateToFrontState rotateToFrontState;

    protected Action<PEEKABOOCHARACTERSTATE> myAction;

    protected void BaseInitialize()
    {
        ViewAngleHalf = 70f;
        MyAnimator = GetComponent<Animator>();

        myStates = new Dictionary<PEEKABOOCHARACTERSTATE, PeekabooCharacterState>();
        nowState = idleState;

        idleState = new PeekabooCharacterIdleState();
        rotateToSomeoneState = new PeekabooCharacterRotateToSomeoneState();
        lookingSomeoneState = new PeekabooCharacterLookingSomeoneState();
        frontToLeftState = new PeekabooCharacterFrontToLeftState();
        lookingLeftState = new PeekabooCharacterLookingLeftState();
        frontToRightState = new PeekabooCharacterFrontToRightState();
        lookingRightState = new PeekabooCharacterLookingRightState();
        rotateToFrontState = new PeekabooCharacterRotateToFrontState();

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
            transform.rotation = Quaternion.Lerp(initialQuaternion, _targetQuaternion, Time.deltaTime * _rotateTime);

            yield return null;
        }

        transform.rotation = _targetQuaternion;

        myAction(_stateKey);
    }

    public IEnumerator WaitForNextBehaviourCoroutine(float _waitTime, PEEKABOOCHARACTERSTATE _stateKey)
    {
        yield return new WaitForSeconds(_waitTime);

        myAction(_stateKey);
    }
}