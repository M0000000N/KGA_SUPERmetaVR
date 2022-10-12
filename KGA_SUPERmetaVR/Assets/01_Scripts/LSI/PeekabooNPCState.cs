
using System.Collections;
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

public static class UIMoveHelper
{
    public static IEnumerator DelayTransform(Transform trans, float delay)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= delay)
            {
                yield break;
            }
            yield return null;
        }
    }
}