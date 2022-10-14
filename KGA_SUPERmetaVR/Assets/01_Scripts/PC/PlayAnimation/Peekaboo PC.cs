
using UnityEngine;

public abstract class PeekabooPC : MonoBehaviour
{
    protected PeekabooPCFSM pcFSM;

    public void Initialize(PeekabooPCFSM _pcfsm)
    {
        pcFSM = _pcfsm;
    }
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit(); 

}
