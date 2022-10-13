using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCTakeDamageState : PeekabooNPCState
{
    [SerializeField]
    private Renderer TargetRenderer;
    [SerializeField]
    private Material ChangeMaterial;

    public override void OnEnter()
    {
        StartCoroutine(LaughtCoroutine(1f));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }

    private IEnumerator LaughtCoroutine(float _time)
    {
        myFSM.MyAnimator.SetBool("isLaught", true);
        yield return new WaitForSeconds(_time);
        myFSM.MyAnimator.SetBool("isLaught", false);

        StartCoroutine(RotateToTargetCoroutine(myFSM.counterAttackTarget));
    }

    private IEnumerator RotateToTargetCoroutine(GameObject _target)
    {
        float elapsedTime = 0f;
        Quaternion initialQuaternion = transform.rotation;
        Vector3 targetDirection = _target.transform.position - transform.position;
        float targetAngle = Quaternion.FromToRotation(Vector3.forward, targetDirection).eulerAngles.y;
        Quaternion rotateQuaternion = Quaternion.Euler(new Vector3(0f, targetAngle, 0f));
        Quaternion targetQuaternion = transform.rotation * rotateQuaternion;

        while (elapsedTime <= StaticData.GetNPCData(15).VALUE)
        {
            yield return null;

            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialQuaternion, targetQuaternion, elapsedTime / StaticData.GetNPCData(15).VALUE);
        }

        transform.rotation = targetQuaternion;
        myFSM.MyAnimator.SetBool("isAttack", true);

        StartCoroutine(AttackCoroutine(1f));
    }

    private IEnumerator AttackCoroutine(float _time)
    {
        myFSM.MyAnimator.SetBool("isAttack", true);
        yield return new WaitForSeconds(_time);
        myFSM.MyAnimator.SetBool("isAttack", false);

        StartCoroutine(Die(1f));
    }

    private IEnumerator Die(float _time)
    {
        TargetRenderer.material = ChangeMaterial;
        Color myColor = TargetRenderer.material.color;
        float decreaseValue = 1 / _time;
        while (0 < TargetRenderer.material.color.a)
        {
            yield return null;

            myColor.a -= decreaseValue * Time.deltaTime;
            TargetRenderer.material.color = myColor;
        }

        Destroy(gameObject);
    }
}