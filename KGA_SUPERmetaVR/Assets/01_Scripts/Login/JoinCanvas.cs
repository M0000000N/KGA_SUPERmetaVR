#define �κ�����
//#define ��ī������
//#define Ʃ�丮��
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
#if Ʃ�丮��
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

#if ��ī������
                PhotonNetwork.LoadLevel("PKB_Main");
                PhotonNetwork.JoinLobby();
            
#endif
#if �κ�����
            LobbyManager.Instance.JoinOrCreateRoom(null, true);
#endif

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
        SoundManager.Instance.PlaySE("popup_click.wav");

        // ȸ������ UI ��ü
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.CreateCanvas);
    }

    public void OnPressForgetPasswordButton()
    {
        SoundManager.Instance.PlaySE("popup_click.wav");

        // ����ã�� UI ��ü
        LoginManager.Instance.SetUICanvas(LoginManager.Instance.FindCanvas);
    }
}
