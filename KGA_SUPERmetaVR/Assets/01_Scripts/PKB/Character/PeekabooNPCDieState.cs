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
        myBody.transform.position += Vector3.up * 15f;
        //myRenderer.material = opaqueMaterial;
        Color myColor = myRenderer.material.color;
        myColor.a = 1f;
        myRenderer.material.color = myColor;
        while (myBody.transform.position.y > 1.2f)
        {
            myBody.transform.position = Vector3.Lerp(myBody.transform.position, new Vector3(myBody.transform.position.x, 1, myBody.transform.position.z), 0.01f);
            yield return new WaitForSeconds(0.01f);
        }

        playerNavMeshAgent.enabled = true;
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
    }
}