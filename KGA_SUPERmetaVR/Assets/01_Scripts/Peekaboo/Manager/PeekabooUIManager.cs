using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeekabooUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerCount;

    private void Update()
    {
        playerCount.text = PeekabooGameManager.Instance.NumberOfPlayers.ToString();
    }
}
