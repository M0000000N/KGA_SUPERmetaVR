using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_GameManager : OnlyOneSceneSingleton<FFF_GameManager>
{
    public bool isReady;

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
