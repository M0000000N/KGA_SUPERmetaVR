using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeekabooTimeManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTimer;

    [SerializeField]
    private float timer;

    private void Start()
    {
        
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        int hour = (int)(timer / 3600);
        int min = (int)((timer - hour * 3600) / 60);
        int second = (int)timer % 60;
        textTimer.text = hour + ":" + min + ":" + second;
    }
}
