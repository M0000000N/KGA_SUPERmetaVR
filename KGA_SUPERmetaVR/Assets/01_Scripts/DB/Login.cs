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
    public static readonly string item = "item";

    public static readonly string create_at = "create_at"; // join_date�� ����
    public static readonly string update_at = "update_at"; // �ֱ� ������ ����� ����
    public static readonly string is_delete = "is_delete";

    public static readonly string peekaboo = "peekaboo";
}

public class Login : MonoBehaviour
{
    [Header("�α���")]
    public TMP_InputField LoginID;
    public TMP_InputField LoginPW;
    public GameObject LoginUI;

    [Header("ȸ������")]
    public TMP_InputField CreateID;
    public TMP_InputField CreatePW;
    public TMP_InputField CreatePWCheck;
    public TMP_InputField CreateNickName;
    public GameObject CreateUI;

    // �׽�Ʈ �ڵ�
    PeekabooDataBase peekabooLogin;

    private void Awake()
    {
        peekabooLogin = transform.GetComponent<PeekabooDataBase>();
    }
    // �׽�Ʈ �ڵ�

    public void Create()
    {
        if (CreatePW.text != CreatePWCheck.text)
        {
            UnityEngine.Debug.Log("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            return;
        }
        else if(DataBase.Instance.CheckUse(UserTableInfo.nickname,CreateNickName.text))
        {
            DataBase.Instance.CreateUser(CreateID.text, CreatePW.text, CreateNickName.text);
            GetDataBase(CreateNickName.text);
            FeefawfumDataBase.Instance.CreateFeefawfumData();
            JoinPage();
        }
    }

    public void Join()
    {
        if (DataBase.Instance.Login(LoginID.text, LoginPW.text))
        {
            UnityEngine.Debug.Log("�α��ο� �����߽��ϴ�.");
            GetDataBase(LoginID.text);

            // �׽�Ʈ �ڵ�
            peekabooLogin.SaveCharacterList();
            // �׽�Ʈ �ڵ�

            PeekabooDataBase.Instance.LoadPeekabooData();
            FeefawfumDataBase.Instance.LoadFeefawfumData();

            // PhotonNetwork.LoadLevel("PKB_Main");
            PhotonNetwork.LoadLevel("FeeFawFum_TestDB_AJJ");
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

    // �׽�Ʈ �ڵ�
    public void GetDataBase(string _userID)
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, _userID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                GameManager.Instance.PlayerData.ID = row[UserTableInfo.user_id].ToString();
                GameManager.Instance.PlayerData.Nickname = row[UserTableInfo.nickname].ToString();
                PhotonNetwork.NickName = GameManager.Instance.PlayerData.Nickname;
                GameManager.Instance.PlayerData.Coin =  int.Parse(row[UserTableInfo.coin].ToString());
                // TODO : ������ ����Ʈ ���� �ʿ�
            }
        }
    }
    // �׽�Ʈ �ڵ�
}
