using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooNPCDieState : PeekabooCharacterState
{
    [SerializeField]
    private Renderer myRenderer;
    [SerializeField]
    private Material fadeMaterial;
    [SerializeField]
    private NavMeshAgent playerNavMeshAgent;

    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        Debug.Log("¿∏æ” ¡Í±›");
        StartCoroutine(DieCoroutine(2f));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }

    private IEnumerator DieCoroutine(float _time)
    {
        Color myColor = myRenderer.material.color;
        myRenderer.material = fadeMaterial;
        float decreaseValue = 1 / _time;
        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= decreaseValue * Time.deltaTime;
            myRenderer.material.color = myColor;

            yield return null;
        }
        Debug.Log("∏ÆΩ∫∆˘¡ﬂ");
        StartCoroutine(RespawnCoroutine(5f));
        //playerNavMeshAgent.enabled = false;
        //PeekabooGameManager.Instance.PeekabooSpawner.RespawnNPC(transform.position);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 1, transform.position.z), 5f);
        //playerNavMeshAgent.enabled = true;

    }
    
    private IEnumerator RespawnCoroutine (float _time)
    {
        playerNavMeshAgent.enabled = false;
        PeekabooGameManager.Instance.PeekabooSpawner.RespawnNPC(transform.position);
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 1, transform.position.z), _time);
        yield return new WaitForSeconds(_time);
        playerNavMeshAgent.enabled = true;
    }
}