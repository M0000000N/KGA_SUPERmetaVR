using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_GameManager : OnlyOneSceneSingleton<FFF_GameManager>
{
    [SerializeField] ItemSelect itemSelect;
    [SerializeField] Animator[] fffAnimationController;
    [SerializeField] FFF_Button[] button;
    public Sprite[] ImagePool { get { return imagePool; } set { imagePool = value; } }
    [SerializeField] Sprite[] imagePool;
    public int Flow { get { return flow; } set { flow = value; } }
    private int flow; // 0 : ���� ��, 1: ����1(�Ѽ� ���), 2 : ����2(������), 3 : ����3(gui�� �� ���߱�)
    public int Score { get { return score; } set { score = value; } }
    private int score;
    public int FailCount { get { return failCount; } set { failCount = value; } }
    private int failCount;
    public int DoneCount { get { return doneCount; } set { doneCount = value; } }
    private int doneCount;

    private int round;

    private void Start()
    {
        Initioalize();
    }

    public void Initioalize()
    {
        flow = 0;
        Score = 0;
        round = 0;
        doneCount = 0;
    }

    private void Update()
    {
        if (flow == 2)
        {
            if (doneCount > 0 && doneCount % 2 == 0)
            {
                SetButton(round, false);
                round++;
                SetButton(round, true);
            }

            if (score >= 15)
            {
                FinishDance();
                SetTriggerFFFNPCAnimation("MissionClear");
            }
            if (failCount >= 3)
            {
                FinishDance();
                SetBoolFFFNPCAnimation("DanceStart", false);
            }
        }
    }

    private void SetButton(int _round, bool _isActive)
    {
        button[_round].gameObject.SetActive(_isActive);
        button[_round + 1].gameObject.SetActive(_isActive);
    }

    public void StartDance()
    {
        flow = 2;
        SoundManager.Instance.PlayBGM("fee_faw_fum_bgm.mp3");
        SetBoolFFFNPCAnimation("DanceStart", true);
        SetButton(0, true);
        itemSelect.HideRightRay(true);
    }

    private void FinishDance()
    {
        SetBoolFFFNPCAnimation("DanceStart", false);
        Initioalize();
        SoundManager.Instance.PlayBGM("ROBEE_bgm.mp3");
        itemSelect.HideRightRay(false);

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