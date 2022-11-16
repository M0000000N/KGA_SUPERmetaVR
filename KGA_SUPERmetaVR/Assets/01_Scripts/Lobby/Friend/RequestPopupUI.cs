using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class RequestPopupUI : SingletonBehaviour<RequestPopupUI>
{
    [SerializeField] private GameObject popupUI;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private GameObject[] buttonType;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private string sendPlayer;

    
    public void SetPopup(int _PopupID, string _PlayerNickname, UnityAction _yesAction = null, UnityAction _noAction = null)
    {
        titleText.text = StaticData.GetNotificationData(_PopupID).Title;
        contentText.text = string.Format(StaticData.GetNotificationData(_PopupID).Description, _PlayerNickname);

        if (StaticData.GetNotificationData(_PopupID).Type == 1)
        {
            buttonType[0].SetActive(true);
            buttonType[1].SetActive(false);
        }
        else
        {
            buttonType[1].SetActive(true);
            buttonType[0].SetActive(false);

            yesButton.onClick.RemoveAllListeners();
            if(_yesAction != null) yesButton.onClick.AddListener(_yesAction);
            yesButton.onClick.AddListener(ClosePopup);
            noButton.onClick.RemoveAllListeners();
            if (_noAction != null) noButton.onClick.AddListener(_noAction);
            noButton.onClick.AddListener(ClosePopup);
        }
        OpenPopup();
    }

    public void OpenPopup()
    {
        popupUI.gameObject.SetActive(true);
    }

    public void ClosePopup() 
    {
        popupUI.gameObject.SetActive(false);
    }
}
