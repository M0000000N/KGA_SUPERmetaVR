using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_GameManager : OnlyOneSceneSingleton<FFF_GameManager>
{
    public int flow; // 0 : 레디 전, 1: 퍼즐1(한손 잡기), 2 : 퍼즐2(양손잡기), 3 : 퍼즐3(gui에 손 맞추기)

    public int Score;
    public int DoneCount;
    [SerializeField] Animator[] fffAnimationController;
    [SerializeField] FFF_ButtonList FFF_ButtonList;

    private void Start()
    {
        Initioalize();
    }

    public void Initioalize()
    {
        flow = 0;
        Score = 0;
        DoneCount = 0;
    }

    public void StartDanceMode()
    {
        FFF_GameManager.Instance.flow = 2;
        SoundManager.Instance.PlayBGM("fee_faw_fum_bgm.mp3");
        SetBoolFFFNPCAnimation("DanceStart", true);
        FFF_ButtonList.SetNextButtonList(0, true);
    }
    private void Update()
    {
        if (FFF_GameManager.Instance.flow == 2)
        {
            if (DoneCount >= 15)
            {
                if (Score >= 15)
                {
                    SetBoolFFFNPCAnimation("DanceStart", false);
                    SetTriggerFFFNPCAnimation("MissionClear");
                    Initioalize();
                    SoundManager.Instance.PlayBGM("ROBEE_bgm.mp3");
                }
                //if() 실패했을 때
            }
        }
    }

    private void SetBoolFFFNPCAnimation(string _name, bool _value)
    {
        for (int i = 0; i < fffAnimationController.Length; i++)
        {
            fffAnimationController[i].SetBool(_name, _value);
        }
    }

    private void SetTriggerFFFNPCAnimation(string _name)
    {
        for (int i = 0; i < fffAnimationController.Length; i++)
        {
            fffAnimationController[i].SetTrigger(_name);
        }
    }
}
