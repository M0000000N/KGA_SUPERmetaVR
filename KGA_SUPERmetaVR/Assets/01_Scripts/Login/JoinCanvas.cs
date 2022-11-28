#define 로비진입
//#define 피카부진입
//#define 튜토리얼
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class JoinCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerObject;

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
        SoundManager.Instance.PlaySE("popup_open.wav");
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
    }

    public void OnPressLoginButton()
    {
        SoundManager.Instance.PlaySE("popup_click.wav");

        if (UserDataBase.Instance.Join(inputID.text, inputPW.text))
        {
#if 튜토리얼
            if(UserDataBase.Instance.CheckUserNickName(inputID.text))
            {
                LobbyManager.Instance.JoinOrCreateRoom(null, true);
            }
            else
            {
                RenderSettings.skybox = LoginManager.Instance.NewSkybox;
                LoginManager.Instance.UserID = inputID.text;
                playerObject.transform.position = new Vector3(0, -1000f, 1.3f);
            }
#endif

#if 피카부진입
                PhotonNetwork.LoadLevel("PKB_Main");
                PhotonNetwork.JoinLobby();
            
#endif
#if 로비진입
            LobbyManager.Instance.JoinOrCreateRoom(null, true);
#endif

        }
        else
        {
            // 오류팝업 : DataBase.instance.Login 에서 조건을 판단하여 팝업을 출력
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
        SoundManager.Instance.PlaySE("popup_click.wav");

        // 회원가입 UI 교체
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.CreateCanvas);
    }

    public void OnPressForgetPasswordButton()
    {
        SoundManager.Instance.PlaySE("popup_click.wav");

        // 정보찾기 UI 교체
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.FindCanvas);
    }
}
