using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;
using Photon.Pun;

public static class UserTableInfo
{
    public static readonly string table_name = "UserData";

    public static readonly string user_id = "user_id";
    public static readonly string user_pw = "user_pw";

    public static readonly string nickname = "nickname";
    public static readonly string coin = "coin";

    public static readonly string create_at = "create_at"; // join_date와 동일
    public static readonly string update_at = "update_at"; // 최근 정보가 변경된 시점
    public static readonly string is_delete = "is_delete";

    public static readonly string peekaboo = "peekaboo";
}

public class Login : MonoBehaviour
{
    [Header("로그인")]
    public TMP_InputField LoginID;
    public TMP_InputField LoginPW;
    public GameObject LoginUI;

    [Header("회원가입")]
    public TMP_InputField CreateID;
    public TMP_InputField CreatePW;
    public TMP_InputField CreatePWCheck;
    public TMP_InputField CreateNickName;
    public GameObject CreateUI;

    // 테스트 코드
    PeekabooLogin peekabooLogin;

    private void Awake()
    {
        peekabooLogin = transform.GetComponent<PeekabooLogin>();
    }
    // 테스트 코드

    public void Create()
    {
        if (CreatePW.text != CreatePWCheck.text)
        {
            UnityEngine.Debug.Log("비밀번호가 일치하지 않습니다.");
            return;
        }
        else if(DataBase.Instance.CheckUse(UserTableInfo.nickname,CreateNickName.text))
        {
            DataBase.Instance.CreateUser(CreateID.text, CreatePW.text, CreateNickName.text);

            // 테스트 코드
            peekabooLogin.SavePeekabooData();
            // 테스트 코드
        }
    }

    public void Join()
    {
        if (DataBase.Instance.Login(LoginID.text, LoginPW.text))
        {
            UnityEngine.Debug.Log("로그인에 성공했습니다.");
            GameManager.Instance.PlayerData.ID = LoginID.text;
            GetDataBase();

            PhotonNetwork.LoadLevel("Peekaboo_WaitingRoom");
            // 테스트 코드
            //peekabooLogin.LoadPeekabooData();
            //UnityEngine.Debug.Log($"Peekaboo : {GameManager.Instance.PlayerData.PlayerPeekabooData.SelectCharacter}");
            //for (int i = 0; i < GameManager.Instance.PlayerData.PlayerPeekabooData.Character.Length; i++)
            //{
            //    UnityEngine.Debug.Log($"PeekabooCharacter : {GameManager.Instance.PlayerData.PlayerPeekabooData.Character[i]}");
            //}
            // 테스트 코드
        }
    }

    public void CreatePage()
    {
        LoginUI.SetActive(false);
        CreateUI.SetActive(true);
    }

    public void JoinPage()
    {
        LoginUI.SetActive(true);
        CreateUI.SetActive(false);
    }

    // 테스트 코드
    public void GetDataBase()
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                GameManager.Instance.PlayerData.Nickname = row[UserTableInfo.nickname].ToString();
                GameManager.Instance.PlayerData.Coin =  int.Parse(row[UserTableInfo.coin].ToString());
            }
        }
    }
    // 테스트 코드
}
