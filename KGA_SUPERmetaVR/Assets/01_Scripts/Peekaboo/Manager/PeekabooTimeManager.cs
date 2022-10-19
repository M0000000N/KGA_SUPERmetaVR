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

    private void Start()
    {
        
    }

    private void Update()
    {
        gameTimer -= Time.deltaTime;
        int hour = (int)(gameTimer / 3600);
        int min = (int)((gameTimer - hour * 3600) / 60);
        int second = (int)gameTimer % 60;
        textTimer.text = hour + ":" + min + ":" + second;
    }
}
