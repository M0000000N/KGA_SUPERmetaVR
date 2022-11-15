using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasicMessageBox : MonoBehaviour
{
    Button okButton;
    Button cancelButton;
    Text text;
    public void SetBtn(UnityAction okAction, UnityAction cancelAction, string contentText)
    {
        if (okButton == null)
        {
            okButton = transform.Find("okButton").GetComponent<Button>();
            cancelButton = transform.Find("cancelButton").GetComponent<Button>();
            text = transform.Find("contentText").GetComponent<Text>();
        }

        text.text = "";
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        text.text = contentText;

        okButton.onClick.AddListener(() =>
        {
            okAction.Invoke();
            gameObject.SetActive(false);

        });
        cancelButton.onClick.AddListener(() =>
        {
            cancelAction.Invoke();
            gameObject.SetActive(false);
        });

        gameObject.SetActive(true);
    }
}
