using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PeekabooUIManager : OnlyOneSceneSingleton<PeekabooUIManager>
{
    [SerializeField]
    private TextMeshProUGUI playerCountText;
    private void Update()
    {
        playerCountText.text = PeekabooGameManager.Instance.NumberOfPlayers.ToString();
    }
}
