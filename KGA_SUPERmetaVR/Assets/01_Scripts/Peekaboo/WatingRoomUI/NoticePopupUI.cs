using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticePopupUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Button yesButton;
    [SerializeField] TextMeshProUGUI yesButtonText;

    public void OpenNotificationPopupUI(int _id)
    {
        titleName.text = StaticData.GetNotificationData(_id).Title;
        description.text = StaticData.GetNotificationData(_id).Description;
        yesButtonText.text = StaticData.GetNotificationData(_id).Buttontext;
        gameObject.SetActive(true);
    }

    public void SetNoticePopup(string _titleName, string _description, string _yesButtonText)
    {
        titleName.text = _titleName;
        description.text = _description;
        yesButtonText.text = _yesButtonText;
        gameObject.SetActive(true);

    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
