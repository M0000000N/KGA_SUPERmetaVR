using UnityEngine;
using Photon.Pun;

public abstract class PeekabooCharacterState : MonoBehaviourPun
{
    // 필요 시 스스로 FSM.ChangeState() 호출을 위해 참조
    protected PeekabooCharacterFSM myFSM;

    // FSM.AddState() 실행 시 실행할 초기화 함수
    public void Initialize(PeekabooCharacterFSM _FSM)
    {
        myFSM = _FSM;
        Initialize();
    }

    // 각 상태별 다르게 동작할 초기화 함수
    protected abstract void Initialize();

    // 해당 상태 진입 시 실행할 함수
    public abstract void OnEnter();
    // 상태별 매 프레임 실행할 함수
    public abstract void OnUpdate();
    // 해당 상태에서 벗어날 시 실행할 함수
    public abstract void OnExit();
}