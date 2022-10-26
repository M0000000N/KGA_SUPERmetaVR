using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PeekabooNPCDieState : PeekabooCharacterState
{
    [SerializeField]
    private Renderer myRenderer;
    [SerializeField]
    private Material fadeMaterial;
    [SerializeField]
    private NavMeshAgent playerNavMeshAgent;

    private Material myMaterial;

    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        photonView.RPC("StartDie", RpcTarget.All);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }

    [PunRPC]
    private void StartDie()
    {
        StartCoroutine(DieCoroutine(2f));
    }

    private IEnumerator DieCoroutine(float _time)
    {
        myMaterial = myRenderer.material;
        myRenderer.material = fadeMaterial;
        Color myColor = myRenderer.material.color;
        float decreaseValue = 1 / _time;
        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= decreaseValue * Time.deltaTime;
            myRenderer.material.color = myColor;

            yield return null;
        }

        // photonView.RPC("StartRespawn", RpcTarget.All);
        StartCoroutine(RespawnCoroutine(3f));
    }

    [PunRPC]
    private void StartRespawn()
    {
        StartCoroutine(RespawnCoroutine(3f));
    }

    private IEnumerator RespawnCoroutine(float _time)
    {
        yield return new WaitForSeconds(_time);

        playerNavMeshAgent.enabled = false;
        transform.position = PeekabooGameManager.Instance.PeekabooSpawner.RespawnNPC(transform.position);  
        transform.position += Vector3.up * 15f;
        myRenderer.material = myMaterial;
        Color myColor = myRenderer.material.color;
        myColor.a = 255f;
        myRenderer.material.color = myColor;
        while (transform.position.y > 1.2f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 1, transform.position.z), 0.01f);
            yield return new WaitForSeconds(0.01f);
        }

        playerNavMeshAgent.enabled = true;
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
    }
}