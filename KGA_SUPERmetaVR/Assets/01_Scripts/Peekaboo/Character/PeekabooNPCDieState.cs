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
    [SerializeField]
    private Transform NPCbody;

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
        Color myColor = myRenderer.material.color;
        float decreaseValue = 1 / _time;
        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= decreaseValue * Time.deltaTime;
            myRenderer.material.color = myColor;

            yield return null;
        }
        Debug.Log("NPC 사망");
        // NPC 리스폰
        StartCoroutine(RespawnCoroutine(3f));

    }

    private IEnumerator RespawnCoroutine(float _time)
    {
        yield return new WaitForSeconds(_time);

        // 지금 있는 구역이 아닌 다른 랜덤한 구역으로 이동
        NPCbody.position = PeekabooGameManager.Instance.PeekabooSpawner.RespawnNPC(transform.position);  
        playerNavMeshAgent.enabled = false;
        NPCbody.position += Vector3.up * 15f;
        Color myColor = myRenderer.material.color;
        myColor.a = 255f;
        myRenderer.material.color = myColor;
        // 위에서 아래로 천천히 떨어짐
        while (NPCbody.position.y > 1.2f)
        {
            NPCbody.position = Vector3.Lerp(NPCbody.position, new Vector3(NPCbody.position.x, 1, NPCbody.position.z), 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
       
        playerNavMeshAgent.enabled = true;

        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
    }
}