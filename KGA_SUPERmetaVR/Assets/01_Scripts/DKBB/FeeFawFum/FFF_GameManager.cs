using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_GameManager : OnlyOneSceneSingleton<FFF_GameManager>
{
    public bool isReady;

    private void OnTriggerExit(Collider other)
    {
        // ��� UI ���
        GuideUI.Instance.ActiveGuideUI("�����̳��", 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        // ��� UI�����
    }
}
