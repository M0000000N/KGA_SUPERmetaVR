using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PKB_ExitUI : MonoBehaviour
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
        SoundManager.Instance.PlaySE("popup_click.wav");
        LobbyManager.Instance.JoinOrCreateRoom(null, true);
        gameObject.SetActive(false);
    }

    public void OnClickNoButton()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
        gameObject.SetActive(false);
    }
}

