using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PKB_RankingDataUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;
    public TextMeshProUGUI UserName { get { return userName; } set { userName = value; } }

    [SerializeField] private TextMeshProUGUI score;
    public TextMeshProUGUI Score { get { return score; } set { score = value; } }

    [SerializeField] private TextMeshProUGUI time;
    public TextMeshProUGUI Time { get { return time; } set { time = value; } }
}
