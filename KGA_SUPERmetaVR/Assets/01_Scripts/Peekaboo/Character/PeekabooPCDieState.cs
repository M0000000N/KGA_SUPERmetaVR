using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooPCDieState : PeekabooCharacterState
{
    [SerializeField]
    private Renderer myRenderer;
    [SerializeField]
    private Material fadeMaterial;

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
        myRenderer.material = fadeMaterial;
        float decreaseValue = 1 / _time;
        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= decreaseValue * Time.deltaTime;
            myRenderer.material.color = myColor;

            yield return null;
        }
        PeekabooGameManager.Instance.PlayerGameOver();
        photonView.RPC("PlayerDie", RpcTarget.All);
        Destroy(gameObject);
    }
    
    [PunRPC]
    private void PlayerDie()
    {
        PeekabooGameManager.Instance.NumberOfPlayers--;
    }
}