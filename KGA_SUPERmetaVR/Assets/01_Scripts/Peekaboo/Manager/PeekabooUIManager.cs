using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeekabooUIManager : OnlyOneSceneSingleton<PeekabooUIManager>
{
    [SerializeField]
    private TextMeshProUGUI playerCount;

    private void Update()
    {
        playerCount.text = PeekabooGameManager.Instance.NumberOfPlayers.ToString();
    }

    public void GameOverUI(int _nowPlayerCount)
    {
        // �÷��̾ 2������ �Ͻ� �����ϱ� ��ư ��Ȱ��ȭ
        // ���ӽð��� 0�����Ͻ� ��� �����ϱ� ��ư ��Ȱ��ȭ
    }
}
