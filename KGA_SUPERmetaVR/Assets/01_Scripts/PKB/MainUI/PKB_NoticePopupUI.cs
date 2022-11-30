using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PKB_NoticePopupUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI yesButtonText;
    [SerializeField] Button yesButton;

    public void OpenNotificationPopupUI(int _id)
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        titleName.text = StaticData.GetNotificationData(_id).Title;
        description.text = StaticData.GetNotificationData(_id).Description;
        yesButtonText.text = StaticData.GetNotificationData(_id).Buttontext;
        gameObject.SetActive(true);
    }

    public void SetNoticePopup(string _titleName, string _description, string _yesButtonText)
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        titleName.text = _titleName;
        description.text = _description;
        yesButtonText.text = _yesButtonText;
        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
        gameObject.SetActive(false);
    }
}
