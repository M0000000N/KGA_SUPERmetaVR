using UnityEngine;
using Photon.Pun;

public abstract class PeekabooCharacterState : MonoBehaviourPun
{
    // �ʿ� �� ������ FSM.ChangeState() ȣ���� ���� ����
    protected PeekabooCharacterFSM myFSM;

    // FSM.AddState() ���� �� ������ �ʱ�ȭ �Լ�
    public void Initialize(PeekabooCharacterFSM _FSM)
    {
        myFSM = _FSM;
        Initialize();
    }

    // �� ���º� �ٸ��� ������ �ʱ�ȭ �Լ�
    protected abstract void Initialize();

    // �ش� ���� ���� �� ������ �Լ�
    public abstract void OnEnter();
    // ���º� �� ������ ������ �Լ�
    public abstract void OnUpdate();
    // �ش� ���¿��� ��� �� ������ �Լ�
    public abstract void OnExit();
}