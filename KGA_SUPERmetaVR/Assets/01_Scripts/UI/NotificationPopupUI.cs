using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationPopupUI : MonoBehaviour
{
    TextMeshProUGUI TitleName;

    [SerializeField] TextMeshProUGUI titleName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Button yesButton;
    TextMeshProUGUI yesButtonText;

    // Start is called before the first frame update
    void Start()
    {
        yesButtonText = yesButton.GetComponent<TextMeshProUGUI>();
    }

    public void OpenNotificationPopupUI(string _titleName, string _description, string _buttonText)
    {
        titleName.text = _titleName;
        description.text = _description;
        yesButtonText.text = _buttonText;
        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
