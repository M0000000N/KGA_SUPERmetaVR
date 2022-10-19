using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PEEKABOOPCSTATE
{
    IDLE,
    LOOKINGLEFT,
    LOOKINGRIGHT,
    ATTACK, // ���� ���� �� ��ī�� ī��Ʈ +1
    DAMAGE,    // ���� ���� �� �Ҹ� - ���� �˾�â���� ����(���� ����) 
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
