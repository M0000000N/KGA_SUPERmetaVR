using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_GameManager : OnlyOneSceneSingleton<FFF_GameManager>
{
    public int flow; // 0 : ���� ��, 1: ����1(�Ѽ� ���), 2 : ����2(������), 3 : ����3(gui�� �� ���߱�)

    public void initioalize()
    {
        flow = 0;
    }
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
