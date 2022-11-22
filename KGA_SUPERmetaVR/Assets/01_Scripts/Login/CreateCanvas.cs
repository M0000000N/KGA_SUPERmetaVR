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

    // TODO : Ŭ������ ���ȭ...
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
            // ����� �� �ִ� ID
        }
        else
        {
            // ����� �� ���� ID
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
            // �� ������ �ֽ��ϴ�.
        }
        else
        {
            if(inputPW != inputCheckPW)
            {
                // ��й�ȣ Ȯ���� ��ġ���� �ʽ��ϴ�.
                return;
            }

            if (DataBase.Instance.CheckUse(UserTableInfo.nickname, inputName.text) == false)
            {
                // ������� �г����Դϴ�.
                return;
            }

            if (DataBase.Instance.CheckUse(UserTableInfo.user_id, inputID.text) == false)
            {
                // ������� ID�Դϴ�.
                return;
            }
            string birth = inputBirthYear.text + inputBirthMonth.text + inputBirthDay.text;
            UserDataBase.Instance.Create(inputID.text, inputPW.text, inputName.text, birth);
        }
    }

    public void Cancel()
    {
        //UI ��ü
    }
}
