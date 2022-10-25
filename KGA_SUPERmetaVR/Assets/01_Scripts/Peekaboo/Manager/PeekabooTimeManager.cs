using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class PeekabooTimeManager : OnlyOneSceneSingleton<PeekabooTimeManager>
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private float gameTimer;
    public float GameTimer { get { return gameTimer; } }

    private float survivalTime;
    public float SurvivalTime { get { return survivalTime; } }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Timer", RpcTarget.All,gameTimer,survivalTime);
        }
        
    }

    [PunRPC]
    private void Timer(float _gameTimer, float _survivalTime)
    {
        if (PeekabooGameManager.Instance.IsGameOver == false)
        {
            if (_gameTimer <= 0f)
            {
                PeekabooGameManager.Instance.PlayerGameOver();
            }
            if (_gameTimer >= 0f)
            {
                _gameTimer -= Time.deltaTime;
            }
            
            _survivalTime += Time.deltaTime;
        }
        gameTimer = _gameTimer;
        survivalTime = _survivalTime;
        int hour = (int)(_gameTimer / 3600);
        int min = (int)((_gameTimer - hour * 3600) / 60);
        int second = (int)_gameTimer % 60;
        timerText.text = hour + ":" + min + ":" + second;
    }

    
  
}
