using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_GameManager : OnlyOneSceneSingleton<FFF_GameManager>
{
    public int flow; // 0 : 레디 전, 1: 퍼즐1(한손 잡기), 2 : 퍼즐2(양손잡기), 3 : 퍼즐3(gui에 손 맞추기)

    public void initioalize()
    {
        flow = 0;
    }
    private void OnTriggerExit(Collider other)
    {
        // 경고 UI 출력
        GuideUI.Instance.ActiveGuideUI("들어가라이놈아", 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        // 경고 UI사라짐
    }
}
