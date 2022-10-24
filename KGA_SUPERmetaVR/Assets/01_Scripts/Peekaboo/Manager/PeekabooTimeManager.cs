using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PeekabooTimeManager : OnlyOneSceneSingleton<PeekabooTimeManager>
{
    [SerializeField]
    private TextMeshProUGUI textTimer;

    [SerializeField]
    private float gameTimer;
    public float GameTimer { get { return gameTimer; } }

    private float survivalTime;
    public float SurvivalTime { get { return survivalTime; } }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Timer", RpcTarget.All,gameTimer);
        }
    }

    [PunRPC]
    private void Timer(float gameTimer)
    {
        if (gameTimer <= 0f)
        {
            PeekabooGameManager.Instance.IsGameOver = true;
        }
        if (gameTimer >= 0f && PeekabooGameManager.Instance.IsGameOver == false)
        {
            gameTimer -= Time.deltaTime;
        }
        if (PeekabooGameManager.Instance.IsGameOver == false)
        {
            survivalTime += Time.deltaTime;
        }
        int hour = (int)(gameTimer / 3600);
        int min = (int)((gameTimer - hour * 3600) / 60);
        int second = (int)gameTimer % 60;
        textTimer.text = hour + ":" + min + ":" + second;
    }
}
