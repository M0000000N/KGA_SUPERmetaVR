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
    public Material opaqueMaterial;
    [SerializeField]
    private Material fadeMaterial;
    [SerializeField]
    private NavMeshAgent playerNavMeshAgent;
    [SerializeField]
    private GameObject myBody;
    [SerializeField]
    private float spawnMoveTime;
   
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
        myRenderer.material = fadeMaterial;
        Color myColor = myRenderer.material.color;
        float decreaseValue = 1 / _time;
        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= decreaseValue * Time.deltaTime;
            myRenderer.material.color = myColor;

            yield return null;
        }

        StartCoroutine(Peekaboo(3f));
        StartCoroutine(RespawnCoroutine(3f));
    }

    private IEnumerator Peekaboo(float _time)
    {
        yield return new WaitForSeconds(_time);

        myRenderer.material = opaqueMaterial;
    }

    private IEnumerator RespawnCoroutine(float _time)
    {
        yield return new WaitForSeconds(_time);

        playerNavMeshAgent.enabled = false;
        myBody.transform.position = PeekabooGameManager.Instance.PeekabooSpawner.RespawnNPC(transform.position);  
        myBody.transform.position += Vector3.up * 3f;
        //myRenderer.material = opaqueMaterial;
        Color myColor = myRenderer.material.color;
        myColor.a = 1f;
        myRenderer.material.color = myColor;
        Vector3 initPosition = myBody.transform.position;
        float elapsedTime = 0f;
        float spawnTime = 1 / spawnMoveTime;
        while (myBody.transform.position.y > -0.5f)
        {
            myBody.transform.position = Vector3.Lerp(initPosition, new Vector3(myBody.transform.position.x, -0.5f, myBody.transform.position.z), spawnTime * elapsedTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        playerNavMeshAgent.enabled = true;
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
    }
}