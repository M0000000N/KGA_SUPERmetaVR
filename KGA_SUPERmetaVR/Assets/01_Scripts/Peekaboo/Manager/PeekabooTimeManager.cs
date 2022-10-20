using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        if (gameTimer >= 0 && PeekabooGameManager.Instance.IsGameOver == false)
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
