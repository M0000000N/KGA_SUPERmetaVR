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

        isCheckID = false;
    }

    public void CheckID()
    {
        // TODO : 2�� �̸� || 12�� �ʰ��� ���
        if(inputID.text.Length < 2 || inputID.text.Length > 12)
        {
            // ID �Է� ���� ( 2 ~ 12 )
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
            return;
        }

        if (DataBase.Instance.CheckUse(UserTableInfo.user_id, inputID.text))
        {
            // ����� �� �ִ� ID
            LoginManager.Instance.SetPopupButtonUICanvas(LoginManager.Instance.TheIDCanBeUsedPopupCanvas);
            isCheckID = true;
        }
        else
        {
            // ����� �� ���� ID (�ߺ� ���̵�)
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.DuplicateIDPopupCanvas);
        }
    }

    public void Create()
    {
        if (isCheckID == false)
        {
            // �ߺ� Ȯ���� ���ּ���
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
            // �� ������ �ֽ��ϴ�.
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
        }
        else
        {
            // ��й�ȣ Ȯ��
            if (inputPW.text != inputCheckPW.text)
            {
                // ��й�ȣ Ȯ���� ��ġ���� �ʽ��ϴ�.
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
                return;
            }
            else if (inputPW.text.Length < 8 || inputPW.text.Length > 16)
            {
                // ��й�ȣ �Է� ���� ( 8 ~ 16 )
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
                return;
            }

            // ������� Ȯ��
            string birth = inputBirthYear.text + inputBirthMonth.text + inputBirthDay.text;
            if (birth.Length != 8)
            {
                // ������� �Է� ���� ( �� 8 ���� )
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.DuplicateIDPopupCanvas);
            }

            // �ߺ� ID Ȯ��
            if (DataBase.Instance.CheckUse(UserTableInfo.user_id, inputID.text) == false)
            {
                // ������� ID�Դϴ�.
                LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.DuplicateIDPopupCanvas);
                return;
            }

            // ȸ������ �Ϸ�
            LoginManager.Instance.SetPopupButtonUICanvas(LoginManager.Instance.SignUpCompletePopupCanvas); // Ȯ�ν� �˾��̵�
            UserDataBase.Instance.Create(inputID.text, inputPW.text, inputName.text, birth, inputHint.text, inputHintAnswer.text);
        }
    }

    public void Cancel()
    {
        // �α��� UI ��ü
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.JoinCanvas);
    }
}
