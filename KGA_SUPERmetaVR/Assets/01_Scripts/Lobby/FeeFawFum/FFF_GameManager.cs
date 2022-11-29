#define 로비용
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
    private int flow; // 0 : 레디 전, 1: 퍼즐1(한손 잡기), 2 : 퍼즐2(양손잡기), 3 : 퍼즐3(gui에 손 맞추기)
    public int ClearCount { get { return clearCount; } set { clearCount = value; } }
    private int clearCount;
    public int FailCount { get { return failCount; } set { failCount = value; } }
    private int failCount;
    public int TryCount { get { return tryCount; } set { tryCount = value; } }
    private int tryCount;

    private int round;

    private void Start()
    {
        Initioalize();
    }

    public void Initioalize()
    {
        flow = 0;
        ClearCount = 0;
        round = 0;
        tryCount = 0;
    }

    private void Update()
    {
        if (flow == 2)
        {
            if (clearCount >= 15)
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

    public void PlusClearCount(bool _isClear)
    {
        if (_isClear)
        {
            clearCount++;
        }
        else
        {
            failCount++;
        }
        tryCount++;
        if (tryCount > 0 && tryCount % 2 == 0)
        {
            SetButton(round, false);
            round++;
            SetButton(round, false);
        }
    }
    private void SetButton(int _round, bool _isActive)
    {
        if(_round >=15)
        {
        button[_round].gameObject.SetActive(_isActive);

        }
        button[_round].gameObject.SetActive(_isActive);
        button[_round + 1].gameObject.SetActive(_isActive);
    }

    public void StartDance()
    {
        flow = 2;
        // SoundManager.Instance.PlayBGM("fee_faw_fum_bgm.mp3");
        SetBoolFFFNPCAnimation("DanceStart", true);
        SetButton(0, true);
        itemSelect.HideRightRay(true);
    }

    private void FinishDance()
    {
        SetBoolFFFNPCAnimation("DanceStart", false);
        Initioalize();
        // SoundManager.Instance.PlayBGM("ROBEE_bgm.mp3");
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