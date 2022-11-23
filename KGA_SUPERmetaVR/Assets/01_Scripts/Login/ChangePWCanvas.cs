using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ChangePWCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputPW;
    [SerializeField] private TMP_InputField inputCheckPW;
    private string user_id;
    public string User_ID { get { return user_id; } set { user_id = value; } }

    [SerializeField] private Button okButton;

    private void Start()
    {
        okButton.onClick.AddListener(ChangePW);
    }

    private void OnEnable()
    {
        inputPW.text = string.Empty;
        inputCheckPW.text = string.Empty;
    }

    public void ChangePW()
    {
        if(inputPW.text == string.Empty || inputCheckPW.text == string.Empty)
        {
            // 입력해주세요
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
            return;
        }

        if(inputPW.text != inputCheckPW.text)
        {
            // 정보를 확인해주세요
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.IncorrectPWPopupCanvas);
            return;
        }

        if(user_id == string.Empty)
        {
            // 데이터가 잘 들어오지 않았다
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.CheckInfomationPopupCanvas);
            LoginManager.Instance.SetUICanvas(LoginManager.Instance.JoinCanvas);
            return;
        }

        string securityPW = DataBase.Instance.SHA512Hash(inputPW.text + DataBase.Instance.SecurityString);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.user_pw, securityPW, UserTableInfo.user_id, user_id);
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.JoinCanvas);
    }

}
