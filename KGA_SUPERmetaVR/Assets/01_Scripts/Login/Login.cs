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
    PeekabooLogin peekabooLogin;

    private void Awake()
    {
        peekabooLogin = transform.GetComponent<PeekabooLogin>();
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

            // �׽�Ʈ �ڵ�
            peekabooLogin.SavePeekabooData();
            // �׽�Ʈ �ڵ�
        }
    }

    public void Join()
    {
        if (DataBase.Instance.Login(LoginID.text, LoginPW.text))
        {
            UnityEngine.Debug.Log("�α��ο� �����߽��ϴ�.");
            GameManager.Instance.PlayerData.ID = LoginID.text;
            GetDataBase();

            PhotonNetwork.LoadLevel("Peekaboo_WaitingRoom");
            // �׽�Ʈ �ڵ�
            //peekabooLogin.LoadPeekabooData();
            //UnityEngine.Debug.Log($"Peekaboo : {GameManager.Instance.PlayerData.PlayerPeekabooData.SelectCharacter}");
            //for (int i = 0; i < GameManager.Instance.PlayerData.PlayerPeekabooData.Character.Length; i++)
            //{
            //    UnityEngine.Debug.Log($"PeekabooCharacter : {GameManager.Instance.PlayerData.PlayerPeekabooData.Character[i]}");
            //}
            // �׽�Ʈ �ڵ�
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
    // �׽�Ʈ �ڵ�
}
