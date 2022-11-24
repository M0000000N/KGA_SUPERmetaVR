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

    [SerializeField] private Button login;
    [SerializeField] private Button signUp;
    [SerializeField] private Button forgetPassword;

    private void Start()
    {
        login.onClick.AddListener(OnPressLoginButton);
        signUp.onClick.AddListener(OnPressSignUpButton);
        forgetPassword.onClick.AddListener(OnPressForgetPasswordButton);
#if 해줘
        login.interactable = false;
        signUp.interactable = false;
#endif
        forgetPassword.interactable = false;
    }

#if 해줘
    public override void OnConnectedToMaster()
    {
        login.interactable = true;
        signUp.interactable = true;
        forgetPassword.interactable = true;
    }
#endif

    private void OnEnable()
    {
        inputID.text = string.Empty;
        inputPW.text = string.Empty;
    }

    public void OnPressLoginButton()
    {
        if(UserDataBase.Instance.Join(inputID.text, inputPW.text))
        {
#if 해줘
            LobbyManager.Instance.JoinOrCreateRoom(null, true);
            LobbyManager.Instance.CurrentSceneIndex = 3;
#endif
            PhotonNetwork.LoadLevel("TestMakeRoom");
        }
        else
        {
            // 오류팝업 : DataBase.instance.Login 에서 조건을 판단하여 팝업을 출력
        }
    }

    public void OnPressSignUpButton()
    {
        // 회원가입 UI 교체
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.CreateCanvas);
    }

    public void OnPressForgetPasswordButton()
    {
        // 정보찾기 UI 교체
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.FindCanvas);
    }
}
