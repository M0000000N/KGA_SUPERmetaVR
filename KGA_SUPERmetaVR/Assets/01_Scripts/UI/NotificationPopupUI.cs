using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationPopupUI : MonoBehaviour
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

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
