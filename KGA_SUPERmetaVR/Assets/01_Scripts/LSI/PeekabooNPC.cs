using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPC : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float lookingAroundCycle;
    [SerializeField]
    private float turnLeftSideTime;
    [SerializeField]
    private float leftSideWaitTime;
    [SerializeField]
    private float turnRightSideTime;
    [SerializeField]
    private float rightSideWaitTime;

    private PeekabooNPCFSM myFSM;
    private PeekabooNPCMove npcMove;
    private float elapsedTime;

    public GameObject lookingTarget;
    public bool isLooking;
    public bool isLookingAround;
    public Quaternion firstRotation;
    public Quaternion leftSideAngle;
    public Quaternion rightSideAngle;

    void Awake()
    {
        myFSM = GetComponent<PeekabooNPCFSM>();
        npcMove = GetComponent<PeekabooNPCMove>();
        elapsedTime = 0f;
        lookingTarget = null;
        isLooking = false;
    }

    void Update()
    {
        if (isLooking)
        {
            Vector3 rotateDirection = lookingTarget.transform.position - gameObject.transform.position;
            transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(rotateDirection), rotateSpeed * Time.deltaTime);
            return;
        }

        if (isLookingAround == false)
        {
            elapsedTime += Time.deltaTime;
            if (lookingAroundCycle <= elapsedTime)
            {
                isLookingAround = true;
                elapsedTime = 0f;
                // StartCoroutine(LookingAroundCoroutine());
            }
        }

        myFSM.UpdateFSM();
    }

    void OnTriggerStay(Collider _other)
    {
        if (isLooking)
        {
            return;
        }

        lookingTarget = _other.gameObject;
        isLooking = true;
        // StopAllCoroutines();
    }

    void OnTriggerExit(Collider _other)
    {
        if (_other == lookingTarget)
        {
            lookingTarget = null;
            isLooking = false;
        }
    }

    IEnumerator LookingAroundCoroutine()
    {
        firstRotation = transform.rotation;
        Vector3 rotateLeftSide = new Vector3(0f, -90f, 0f);
        leftSideAngle = transform.rotation * Quaternion.Euler(rotateLeftSide);
        Vector3 rotateRightSide = new Vector3(0f, 90f, 0f);
        rightSideAngle = transform.rotation * Quaternion.Euler(rotateRightSide);
        float elapsedTimeInCoroutine = 0f;

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(firstRotation, leftSideAngle, elapsedTimeInCoroutine / turnLeftSideTime);

            if (turnLeftSideTime <= elapsedTimeInCoroutine)
            {
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        yield return new WaitForSeconds(leftSideWaitTime);

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(leftSideAngle, firstRotation, elapsedTimeInCoroutine / turnLeftSideTime);

            if (turnLeftSideTime <= elapsedTimeInCoroutine)
            {
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(firstRotation, rightSideAngle, elapsedTimeInCoroutine / turnRightSideTime);

            if (turnRightSideTime <= elapsedTimeInCoroutine)
            {
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        yield return new WaitForSeconds(rightSideWaitTime);

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(rightSideAngle, firstRotation, elapsedTimeInCoroutine / turnRightSideTime);

            if (turnRightSideTime <= elapsedTimeInCoroutine)
            {
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        isLookingAround = false;
    }
}