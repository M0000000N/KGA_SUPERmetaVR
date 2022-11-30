#define �κ��
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

    public void PlusClearCount(bool _isClear)
    {
        tryCount++;
        if (_isClear)
        {
            clearCount++;
        }
        else
        {
            failCount++;
        }

        if (tryCount % 2 == 0)
        {
            SetButton(round, false);
            round += 2;
            SetButton(round, true);
        }
        if (clearCount >= 15)
        {
            FinishDance();
            RewardManager.Instance.GetItem();
            SetTriggerFFFNPCAnimation("MissionClear");
        }
        if (failCount >= 3)
        {
            FinishDance();
            SetTriggerFFFNPCAnimation("MissionFailed");
        }
    }

    private void SetButton(int _round, bool _isActive)
    {
        if (_round >= 30 )
        {
            FinishDance();
            return;
        }
        else
        {
            button[_round].gameObject.SetActive(_isActive);
            button[_round + 1].gameObject.SetActive(_isActive);
        }
    }

    public void StartDance()
    {
        flow = 2;
#if �κ��
        SoundManager.Instance.PlayBGM("fee_faw_fum_bgm.mp3");
#endif
        SetBoolFFFNPCAnimation("DanceStart", true);
        SetButton(0, true);
        itemSelect.HideRightRay(true);
    }

    private void FinishDance()
    {
        SetBoolFFFNPCAnimation("DanceStart", false);
        SetButton(round, false);
        Initioalize();
#if �κ��
        SoundManager.Instance.PlayBGM("ROBEE_bgm.mp3");
#endif
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