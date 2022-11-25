using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class JoinCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputID;
    [SerializeField] private TMP_InputField inputPW;

    public Button Login { get { return login; } set { login = value; } }
    [SerializeField] private Button login;
    public Button SignUp { get { return signUp; } set { signUp = value; } }
    [SerializeField] private Button signUp;
    public Button ForgetPassword { get { return forgetPassword; } set { forgetPassword = value; } }
    [SerializeField] private Button forgetPassword;

    private void Awake()
    {
        login.onClick.AddListener(OnPressLoginButton);
        signUp.onClick.AddListener(OnPressSignUpButton);
        forgetPassword.onClick.AddListener(OnPressForgetPasswordButton);
        login.interactable = false;
        signUp.interactable = false;
        forgetPassword.interactable = false;
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
            StopCoroutine(ButtonActive());
            StartCoroutine(ButtonActive());
            LobbyManager.Instance.JoinOrCreateRoom(null, true);
            LobbyManager.Instance.CurrentSceneIndex = 3;
        }
        else
        {
            // �����˾� : DataBase.instance.Login ���� ������ �Ǵ��Ͽ� �˾��� ���
        }
    }

    IEnumerator ButtonActive()
    {
        login.interactable = false;
        signUp.interactable = false;
        forgetPassword.interactable = false;

        yield return new WaitForSecondsRealtime(5f);

        login.interactable = true;
        signUp.interactable = true;
        forgetPassword.interactable = true;
    }

    public void OnPressSignUpButton()
    {
        // ȸ������ UI ��ü
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.CreateCanvas);
    }

    public void OnPressForgetPasswordButton()
    {
        // ����ã�� UI ��ü
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.FindCanvas);
    }
}
