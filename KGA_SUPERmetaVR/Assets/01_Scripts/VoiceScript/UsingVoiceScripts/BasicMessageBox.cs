using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime; 

public class BasicMessageBox : MonoBehaviourPun 
{
    Button Check_ConfirmBtn;
    Button Check_RejectBtn;
    Text Check_ContentText;

    public void SetBtn(UnityAction okAction, UnityAction cancelAction, string contentText)
    {
        if (Check_ConfirmBtn == null)
        {
            Check_ConfirmBtn = transform.Find("Check_ConfirmBtn").GetComponent<Button>();
            Check_RejectBtn = transform.Find("Check_RejectBtn").GetComponent<Button>();
            Check_ContentText = transform.Find("Check_ContentText").GetComponent<Text>();
        }

        Check_ContentText.text = "";
        Check_ConfirmBtn.onClick.RemoveAllListeners();
        Check_RejectBtn.onClick.RemoveAllListeners();

        Check_ContentText.text = contentText;

        Check_ConfirmBtn.onClick.AddListener(() =>
        {
            okAction.Invoke();
            gameObject.SetActive(false);
            //수락하였습니다 

        });

        Check_RejectBtn.onClick.AddListener(() =>
        {
            cancelAction.Invoke();
            gameObject.SetActive(false);
            // 거부하였습니다 
        });

        gameObject.SetActive(true);
    }

}
