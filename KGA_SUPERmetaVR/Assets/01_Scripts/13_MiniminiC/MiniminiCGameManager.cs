using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniminiCGameManager : SingletonBehaviour<MiniminiCGameManager>
{
    public MiniminiCPlayerData PlayerData;

    public bool isGameStart;

    [SerializeField] float timmer = 15;
    float nowTime = 0;

    void Update()
    {
        if(isGameStart)
        {
            nowTime += Time.deltaTime;
            if(nowTime >= timmer)
            {
                GameOver();
                nowTime = 0;
            }
        }
    }

    public void GameStart()
    {
        isGameStart = true;
    }

    public void GameOver()
    {
        isGameStart = false;
    }
}
