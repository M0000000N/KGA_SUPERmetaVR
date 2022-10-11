using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class ExitUI : MonoBehaviour
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYesButton);
        noButton.onClick.AddListener(OnClickNoButton);
    }
    public void OnClickYesButton()
    {
        gameObject.SetActive(false);
        Peekaboo_WatingRoomUIManager.Instance.WaitingRoomUI.gameObject.SetActive(false);
        // DKBB∑Œ ¿Ãµø
    }

    public void OnClickNoButton()
    {
        gameObject.SetActive(false);
    }
}

