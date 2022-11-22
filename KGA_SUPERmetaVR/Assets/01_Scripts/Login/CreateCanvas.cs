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
    }

    public void CheckID()
    {
        if(DataBase.Instance.CheckUse(UserTableInfo.user_id, inputID.text))
        {
            // 사용할 수 있는 ID
        }
        else
        {
            // 사용할 수 없는 ID
        }
    }

    public void Create()
    {
        if (inputID.text == string.Empty ||
            inputName.text == string.Empty ||
            inputPW.text == string.Empty ||
            inputCheckPW.text == string.Empty ||
            inputHint.text == string.Empty ||
            inputHintAnswer.text == string.Empty ||
            inputBirthYear.text == string.Empty ||
            inputBirthMonth.text == string.Empty ||
            inputBirthDay.text == string.Empty)
        {
            // 빈 공간이 있습니다.
        }
        else
        {
            if(inputPW != inputCheckPW)
            {
                // 비밀번호 확인이 일치하지 않습니다.
                return;
            }

            if (DataBase.Instance.CheckUse(UserTableInfo.nickname, inputName.text) == false)
            {
                // 사용중인 닉네임입니다.
                return;
            }

            if (DataBase.Instance.CheckUse(UserTableInfo.user_id, inputID.text) == false)
            {
                // 사용중인 ID입니다.
                return;
            }
            string birth = inputBirthYear.text + inputBirthMonth.text + inputBirthDay.text;
            UserDataBase.Instance.Create(inputID.text, inputPW.text, inputName.text, birth);
        }
    }

    public void Cancel()
    {
        //UI 교체
    }
}
