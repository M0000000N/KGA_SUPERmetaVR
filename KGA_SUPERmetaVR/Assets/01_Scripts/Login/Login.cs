using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;

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

    public void Create()
    {
        if (CreatePW.text != CreatePWCheck.text)
        {
            UnityEngine.Debug.Log("비밀번호가 일치하지 않습니다.");
            return;
        }
        else if(DataBase.Instance.CheckUse(UserTableInfo.NickName,CreateNickName.text))
        DataBase.Instance.CreateUser(CreateID.text, CreatePW.text, CreateNickName.text);
    }

    public void Join()
    {
        if (DataBase.Instance.Login(LoginID.text, LoginPW.text))
        {
            UnityEngine.Debug.Log("로그인에 성공했습니다.");
            playerData.ID = LoginID.text;
            GetDataBase();
            UnityEngine.Debug.Log($"ID : {playerData.ID}");
            UnityEngine.Debug.Log($"NickName : {playerData.Nickname}");
            UnityEngine.Debug.Log($"Coin : {playerData.Coin}");
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

    // 테스트를 위한 코드
    PlayerData playerData = new PlayerData();
    public void GetDataBase()
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.TableName, "*", UserTableInfo.ID, playerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                playerData.Nickname = row[UserTableInfo.NickName].ToString();
                playerData.Coin =  int.Parse(row[UserTableInfo.Coin].ToString());
            }
        }
    }
    //
}
