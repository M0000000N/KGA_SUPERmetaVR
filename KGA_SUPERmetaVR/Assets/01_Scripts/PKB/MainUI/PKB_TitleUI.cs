using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PKB_TitleUI : MonoBehaviour
{
    [SerializeField] Button GameStartButton;
    [SerializeField] Button GameExitButton;

    private void Awake()
    {
        GameStartButton.onClick.AddListener(OnClickGameStartButton);
        GameExitButton.onClick.AddListener(OnClickGameExitButton);
    }

    public void OnClickGameStartButton()
    {
        PKB_MainUIManager.Instance.MainUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickGameExitButton()
    {
        LobbyManager.Instance.JoinOrCreateRoom(null, true);
        gameObject.SetActive(false);
    }
}
