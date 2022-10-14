using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOOPCSTATE
{
    IDLE,
    LOOKINGLEFT,
    LOOKINGRIGHT,
    ATTACK, // 공격 성공 시 피카부 카운트 +1
    DAMAGE,    // 공격 실패 시 소멸 - 관전 팝업창으로 연결(추후 연결) 
}

public class PeekabooPCFSM : MonoBehaviour
{
    public PeekabooPCState nowState { get; private set; }
    public PEEKABOONPCSTATE nowStateKey { get; private set; }
    public Animator PCAnimator { get; private set; }
    public GameObject counterPCtarget { get; private set; }
    public PeekabooPC PC { get; private set; }
  
    private Dictionary<PEEKABOOPCSTATE, PeekabooPCState> pcStates;
    private PeekabooPCIdleState pcIdleState;
    private PeekabooPCLookingLeftState pcLookingLeftState;
    private PeekabooPCLookingRIghtState pcLookingRightState;
    private PeekabooPCDamageState peekabooPCDamageState;
    //private PeekabooPCAttackState peekabooPCAttackState;

    private void Awake()
    {
        PC = GetComponent<PeekabooPC>();
        pcStates = new Dictionary<PEEKABOOPCSTATE, PeekabooPCState>();
        pcIdleState = GetComponent<PeekabooPCIdleState>();
        pcLookingLeftState = GetComponent<PeekabooPCLookingLeftState>();
        pcLookingRightState = GetComponent<PeekabooPCLookingRIghtState>();
        peekabooPCDamageState = GetComponent<PeekabooPCDamageState>();
       // peekabooPCAttackState = GetComponent<PeekabooPCAttackState>();

       
        ChangeState(PEEKABOOPCSTATE.IDLE);
    }

    public void AddState(PEEKABOOPCSTATE _stateKey, PeekabooPCState _pcState)
    {
        _pcState.Initialize(this);
        pcStates[_stateKey] = _pcState;
    }

    public void ChangeState(PEEKABOOPCSTATE _stateKey)
    {
 
    }





}
