using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class FindCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TMP_InputField inputBirthYear;
    [SerializeField] private TMP_InputField inputBirthMonth;
    [SerializeField] private TMP_InputField inputBirthDay;

    [SerializeField] private TMP_InputField inputID;
    [SerializeField] private TMP_InputField inputHint;
    [SerializeField] private TMP_InputField inputHintAnswer;


    [SerializeField] private Button findIDButton;
    [SerializeField] private Button findPWButton;
    [SerializeField] private Button cancelButton;

    private void Start()
    {
        findIDButton.onClick.AddListener(FindID);
        findPWButton.onClick.AddListener(FindPW);
        cancelButton.onClick.AddListener(Cancel);
    }

    private void OnEnable()
    {
        inputName.text = string.Empty;
        inputBirthYear.text = string.Empty;
        inputBirthMonth.text = string.Empty;
        inputBirthDay.text = string.Empty;

        inputID.text = string.Empty;
        inputHint.text = string.Empty;
        inputHintAnswer.text = string.Empty;
        SoundManager.Instance.PlaySE("popup_open.wav");
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
    }

    public void FindID()
    {
        SoundManager.Instance.PlaySE("popup_click.wav");

        if (
            inputName.text == string.Empty ||
            inputBirthYear.text == string.Empty ||
            inputBirthMonth.text == string.Empty ||
            inputBirthDay.text == string.Empty
            )
        {
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
            return;
        }

        string birth = inputBirthYear.text + inputBirthMonth.text + inputBirthDay.text;
        string findID = UserDataBase.Instance.FindUserID(inputName.text, birth);
        if(findID != string.Empty)
        {
            // ID 팝업
            LoginManager.Instance.SetPopupButtonUICanvas(LoginManager.Instance.FindIDPopupCanvas);
            LoginManager.Instance.FindIDPopupCanvas.GetComponent<FindIDPopupCanvas>().SetInfomation(findID);
        }
        else
        {
            // 일치하지 않습니다.
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
        }
    }

    public void FindPW()
    {
        SoundManager.Instance.PlaySE("popup_click.wav");

        if (UserDataBase.Instance.FindUserPW(inputID.text, inputHint.text, inputHintAnswer.text))
        {
            // 비밀번호 변경 팝업 출력
            LoginManager.Instance.ChangePWCanvas.GetComponent<ChangePWCanvas>().User_ID = inputID.text;
            LoginManager.Instance.SetUICanvas(LoginManager.Instance.ChangePWCanvas);
        }
        else
        {
            // 일치하지 않습니다.
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
        }
    }

    public void Cancel()
    {
        SoundManager.Instance.PlaySE("popup_click.wav");

        // 로그인 UI 교체
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.JoinCanvas);
    }
}
