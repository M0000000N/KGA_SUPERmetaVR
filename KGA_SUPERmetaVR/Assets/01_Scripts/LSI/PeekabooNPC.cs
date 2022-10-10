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

    private PeekabooNPCMove npcMove;
    private float elapsedTime;

    public GameObject lookingTarget;
    public bool isLooking;
    public bool isLookingAround;
    public Quaternion firstRotation;
    public Quaternion leftSideRotation;
    public Quaternion rightSideRotation;

    void Awake()
    {
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
                StartCoroutine(LookingAroundCoroutine());
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (isLooking)
        {
            return;
        }

        lookingTarget = other.gameObject;
        isLooking = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other == lookingTarget)
        {
            lookingTarget = null;
            isLooking = false;
        }
    }

    IEnumerator LookingAroundCoroutine()
    {
        firstRotation = transform.rotation;
        leftSideRotation = firstRotation;
        leftSideRotation.y -= 40;
        rightSideRotation = firstRotation;
        rightSideRotation.y += 40;
        float elapsedTimeInCoroutine = 0f;

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(firstRotation, leftSideRotation, elapsedTime / turnLeftSideTime);

            if (turnLeftSideTime <= elapsedTimeInCoroutine)
            {
                transform.rotation = leftSideRotation;
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        yield return new WaitForSeconds(leftSideWaitTime);

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(leftSideRotation, firstRotation, elapsedTime / turnLeftSideTime);

            if (turnLeftSideTime <= elapsedTimeInCoroutine)
            {
                transform.rotation = firstRotation;
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(firstRotation, rightSideRotation, elapsedTime / turnRightSideTime);

            if (turnRightSideTime <= elapsedTimeInCoroutine)
            {
                transform.rotation = rightSideRotation;
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        yield return new WaitForSeconds(rightSideWaitTime);

        while (true)
        {
            yield return null;
            elapsedTimeInCoroutine += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(rightSideRotation, firstRotation, elapsedTime / turnRightSideTime);

            if (turnRightSideTime <= elapsedTimeInCoroutine)
            {
                transform.rotation = firstRotation;
                elapsedTimeInCoroutine = 0f;
                break;
            }
        }

        isLookingAround = false;
    }
}