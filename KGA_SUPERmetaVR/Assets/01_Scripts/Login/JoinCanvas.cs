using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class JoinCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputID;
    [SerializeField] private TMP_InputField inputPW;

    [SerializeField] private Button login;
    [SerializeField] private Button signUp;
    [SerializeField] private Button forgetPassword;

    private void Start()
    {
        login.onClick.AddListener(OnPressLoginButton);
        signUp.onClick.AddListener(OnPressSignUpButton);
        forgetPassword.onClick.AddListener(OnPressForgetPasswordButton);
    }

    private void OnEnable()
    {
        inputID.text = string.Empty;
        inputPW.text = string.Empty;
    }

    public void OnPressLoginButton()
    {
        if(UserDataBase.Instance.Join(inputID.text, inputPW.text))
        {
            PhotonNetwork.LoadLevel("TestMakeRoom");
        }
        else
        {
            // UI 교체
        }
    }

    public void OnPressSignUpButton()
    {
        // UI 교체
    }

    public void OnPressForgetPasswordButton()
    {
        // UI 교체
    }
}
