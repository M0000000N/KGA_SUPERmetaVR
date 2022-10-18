using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class PKB_RankingUI : MonoBehaviour
{
    // [SerializeField] int sortCount;
    [SerializeField] Button[] menuButton;
    private int selectButton;

    [Header("랭킹테이블")]

    [SerializeField] PKB_RankingDataUI[] rankingData;

    string[] menu = { "play_count", "win_count", "die_count", "survive_time", "attack_player", "attack_npc" };

    void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        selectButton = 0;
        RefreshUI();
    }

    public void OnButton(int _button)
    {
        selectButton = _button;
        RefreshUI();
    }

    public void RefreshUI()
    {
        // 일단 버튼 UI 모두 꺼진 상태
        for (int i = 0; i < menuButton.Length; i++)
        {
            SetMenuColor(menuButton[i], false);
        }
        // 선택된 버튼만 켜진 상태
        SetMenuColor(menuButton[selectButton], true);

        // 반복하면서 정렬된 랭킹정보 보여줌
        DataTable data = PeekabooDataBase.Instance.SortRanking(menu[selectButton]);

        for (int i = 0; i < 10; i++)
        {
            rankingData[i].gameObject.SetActive(false);

            if (data.Rows.Count > i)
            {
                // TODO : 프로토타입용 하드코딩으로 추후 변경 필요
                rankingData[i].gameObject.SetActive(true);
                rankingData[i].UserName.text = data.Rows[i][2].ToString();
                rankingData[i].Score.text = data.Rows[i][5 + selectButton].ToString();
                rankingData[i].Time.text = data.Rows[i][12].ToString();
            }
        }
    }

    public void SetMenuColor(Button _menu, bool _isActive)
    {
        if(_isActive)
        {
            _menu.GetComponent<Image>().color = new Color(0, 0, 0, 150f / 255f);
        }
        else
        {
            _menu.GetComponent<Image>().color = new Color(0, 0, 0, 50f / 255f);
        }
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
