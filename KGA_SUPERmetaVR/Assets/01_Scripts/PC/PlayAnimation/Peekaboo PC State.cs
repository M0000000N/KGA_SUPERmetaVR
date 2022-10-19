using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PeekabooPCState : MonoBehaviour
{
    protected PeekabooPCState pCState;
    public void Initialize(PeekabooPCFSM _pcfsm)
    {
       

    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit(); 




}
