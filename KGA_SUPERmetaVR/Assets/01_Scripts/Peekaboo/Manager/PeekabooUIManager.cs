using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PeekabooUIManager : OnlyOneSceneSingleton<PeekabooUIManager>
{
    [SerializeField]
    private TextMeshProUGUI playerCountText;
    [SerializeField]
    private GameObject gameResultUI;
    [SerializeField]
    private TextMeshProUGUI playerRankingText;
    [SerializeField]
    private TextMeshProUGUI surprisedEnemyNumbersText;
    [SerializeField]
    private TextMeshProUGUI survivalTimeText;
    [SerializeField]
    private TextMeshProUGUI NumberOfCoinsAcquiredText;
    [SerializeField]
    private Button watchingButton;


    private void Update()
    {
        playerCountText.text = PeekabooGameManager.Instance.NumberOfPlayers.ToString();
        
    }

    public void GameOverUI(int _nowPlayerCount)
    {
        gameResultUI.SetActive(true);
        PeekabooGameManager.Instance.IsGameOver = true;
        if (PeekabooGameManager.Instance.NumberOfPlayers == 1)
        {
            watchingButton.interactable = false;
        }
        // �÷��̾ 2������ �Ͻ� �����ϱ� ��ư ��Ȱ��ȭ
        // ���ӽð��� 0�����Ͻ� ��� �����ϱ� ��ư ��Ȱ��ȭ
        playerRankingText.text = "# " + PeekabooGameManager.Instance.NumberOfPlayers.ToString() + " / " + PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom.ToString();
        // surprisedEnemyNumbers = �÷��̾ �Ų ��
        survivalTimeText.text = "���� �ð� : " + ((int)(PeekabooTimeManager.Instance.SurvivalTime / 60)).ToString() + "��" + ((int)(PeekabooTimeManager.Instance.SurvivalTime % 60)).ToString() + "��";
        NumberOfCoinsAcquiredText.text = "ȹ�� ���� :       X " + (PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom - PeekabooGameManager.Instance.NumberOfPlayers).ToString();
    }
}
