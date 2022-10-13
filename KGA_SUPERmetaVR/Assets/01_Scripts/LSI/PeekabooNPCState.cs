using UnityEngine;

public abstract class PeekabooNPCState : MonoBehaviour
{
    protected PeekabooNPCFSM myFSM;

    public void Initialize(PeekabooNPCFSM _fsm)
    {
        myFSM = _fsm;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}