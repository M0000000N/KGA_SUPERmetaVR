using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CreateNickName : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputNickName;

    [SerializeField] private Button createNickNameButton;

    private bool isCheckID;

    private void Start()
    {
        createNickNameButton.onClick.AddListener(CreateeButton);
    }

    // TODO : 클래스로 모듈화...
    private void OnEnable()
    {
        inputNickName.text = string.Empty;
        SoundManager.Instance.PlaySE("popup_open.wav");
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
    }

    public void CreateeButton()
    {
        SoundManager.Instance.PlaySE("popup_click.wav");

        // TODO : 1자 미만 || 8자 초과할 경우
        if (inputNickName.text.Length < 1 || inputNickName.text.Length > 8)
        {
            // ID 입력 길이 ( 2 ~ 12 )
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.NopCreateNickNamePopup);
            return;
        }
        else if (StaticData.CheckBadNickname(inputNickName.text) == false)
        {
            // BadNickname
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.NopCreateNickNamePopup);
        }

        if (DataBase.Instance.CheckUse(UserTableInfo.nickname, inputNickName.text))
        {
            // 사용할 수 있는 닉네임
            GameManager.Instance.PlayerData.Nickname = inputNickName.text;
            DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.nickname, inputNickName.text, UserTableInfo.user_id, LoginManager.Instance.UserID);
            LoginManager.Instance.SetUICanvas(LoginManager.Instance.SelectCharacter);
            gameObject.SetActive(false);
        }
        else
        {
            // 중복 닉네임
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.NopCreateNickNamePopup);
        }
    }

}
