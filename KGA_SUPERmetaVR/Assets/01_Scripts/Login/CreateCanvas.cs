using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CreateCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputID;
    [SerializeField] private TMP_InputField inputName;

    [SerializeField] private TMP_InputField inputPW;
    [SerializeField] private TMP_InputField inputCheckPW;

    [SerializeField] private TMP_InputField inputHint;
    [SerializeField] private TMP_InputField inputHintAnswer;

    [SerializeField] private TMP_InputField inputBirthYear;
    [SerializeField] private TMP_InputField inputBirthMonth;
    [SerializeField] private TMP_InputField inputBirthDay;

    [SerializeField] private Button checkIDButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button cancelButton;

    private bool isCheckID;

    private void Start()
    {
        checkIDButton.onClick.AddListener(CheckID);
        createButton.onClick.AddListener(Create);
        cancelButton.onClick.AddListener(Cancel);
    }

    // TODO : 클래스로 모듈화...
    private void OnEnable()
    {
        inputID.text = string.Empty;
        inputName.text = string.Empty;
        inputPW.text = string.Empty;
        inputCheckPW.text = string.Empty;
        inputHint.text = string.Empty;
        inputHintAnswer.text = string.Empty;
        inputBirthYear.text = string.Empty;
        inputBirthMonth.text = string.Empty;
        inputBirthDay.text = string.Empty;

        isCheckID = false;
    }

    public void CheckID()
    {
        // TODO : 2자 미만 || 12자 초과할 경우
        if(inputID.text.Length < 2 || inputID.text.Length > 12)
        {
            // ID 입력 길이 ( 2 ~ 12 )
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
            return;
        }

        if (DataBase.Instance.CheckUse(UserTableInfo.user_id, inputID.text))
        {
            // 사용할 수 있는 ID
            LoginManager.Instance.SetPopupButtonUICanvas(LoginManager.Instance.TheIDCanBeUsedPopupCanvas);
            isCheckID = true;
        }
        else
        {
            // 사용할 수 없는 ID (중복 아이디)
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.DuplicateIDPopupCanvas);
        }
    }

    public void Create()
    {
        if (isCheckID == false)
        {
            // 중복 확인을 해주세요
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
        }
        else if (
                inputID.text == string.Empty ||
                inputName.text == string.Empty ||
                inputPW.text == string.Empty ||
                inputCheckPW.text == string.Empty ||
                inputHint.text == string.Empty ||
                inputHintAnswer.text == string.Empty ||
                inputBirthYear.text == string.Empty ||
                inputBirthMonth.text == string.Empty ||
                inputBirthDay.text == string.Empty
                )
        {
            // 빈 공간이 있습니다.
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
        }
        else
        {
            // 비밀번호 확인
            if (inputPW.text != inputCheckPW.text)
            {
                // 비밀번호 확인이 일치하지 않습니다.
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
                return;
            }
            else if (inputPW.text.Length < 8 || inputPW.text.Length > 16)
            {
                // 비밀번호 입력 길이 ( 8 ~ 16 )
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
                return;
            }

            // 생년월일 확인
            string birth = inputBirthYear.text + inputBirthMonth.text + inputBirthDay.text;
            if (birth.Length != 8)
            {
                // 생년월일 입력 길이 ( 총 8 글자 )
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.DuplicateIDPopupCanvas);
            }

            // 중복 ID 확인
            if (DataBase.Instance.CheckUse(UserTableInfo.user_id, inputID.text) == false)
            {
                // 사용중인 ID입니다.
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.DuplicateIDPopupCanvas);
                return;
            }

            // 회원가입 완료
            LoginManager.Instance.SetPopupButtonUICanvas(LoginManager.Instance.SignUpCompletePopupCanvas); // 확인시 팝업이동
            UserDataBase.Instance.Create(inputID.text, inputPW.text, inputName.text, birth, inputHint.text, inputHintAnswer.text);
        }
    }

    public void Cancel()
    {
        // 로그인 UI 교체
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.JoinCanvas);
    }
}
