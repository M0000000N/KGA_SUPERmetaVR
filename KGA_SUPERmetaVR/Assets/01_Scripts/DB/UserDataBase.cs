using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;
using Photon.Pun;

public static class UserTableInfo
{
    public static readonly string table_name = "userdata";

    public static readonly string id = "id";

    public static readonly string user_id = "user_id";
    public static readonly string user_pw = "user_pw";

    public static readonly string nickname = "nickname";
    public static readonly string name = "name";
    public static readonly string birth = "birth";
    public static readonly string hint = "hint";
    public static readonly string hint_answer = "hint_answer";

    public static readonly string coin = "coin";
    public static readonly string customize = "customize";
    public static readonly string item = "item";
    public static readonly string friend = "friend";
    public static readonly string is_connect = "is_connect";

    public static readonly string clear_tutorial = "clear_tutorial";

    public static readonly string create_at = "create_at"; // join_date�� ����
    public static readonly string update_at = "update_at"; // �ֱ� ������ ����� ����
    public static readonly string is_delete = "is_delete";

    public static readonly string peekaboo = "peekaboo";
}

public class UserDataBase : SingletonBehaviour<UserDataBase>
{
    //[Header("�α���")]
    //public TMP_InputField LoginID;
    //public TMP_InputField LoginPW;
    //public GameObject LoginUI;

    //[Header("ȸ������")]
    //public TMP_InputField CreateID;
    //public TMP_InputField CreatePW;
    //public TMP_InputField CreatePWCheck;
    //public TMP_InputField CreateNickName;
    //public GameObject CreateUI;

    private PlayerData playerData;
    private string playerItemList;
    private string friendList;

    // �׽�Ʈ �ڵ�
    private PeekabooDataBase peekabooLogin;

    private void Awake()
    {
        peekabooLogin = transform.GetComponent<PeekabooDataBase>();
        playerData = GameManager.Instance.PlayerData;
    }
    // �׽�Ʈ �ڵ�

    public void Create(string _id, string _pw, string _name, string _birth, string _hint, string _hintAnswer)
    {
        DataBase.Instance.CreateUser(_id, _pw, _name, _birth, _hint, _hintAnswer);

        // --------------------------------------------------------------------------------------------- �׽�Ʈ �ڵ�
        // ���� �� PlayerData�� ���� ���� ������ CreateID�� ���� �����͸� �־��ְ� �ִ�.

        // [������]
        // DB ����
        FeeFawFumDataBase.Instance.CreateFeefawfumData(_id);
        FlyDragonDataBase.Instance.CreateFlyDragonData(_id);
        CloverColonyDataBase.Instance.CreateCloverColonyData(_id);
        PeekabooDataBase.Instance.CreatePeekabooData(_id);

        // TestCode ����
        playerItemList = JsonUtility.ToJson(playerData.ItemSlotData);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.item, playerItemList, UserTableInfo.user_id, _id);

        // [ģ��]
        friendList = JsonUtility.ToJson(playerData.Friends);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.friend, friendList, UserTableInfo.user_id, _id);

        // --------------------------------------------------------------------------------------------- �׽�Ʈ �ڵ�

        GetDataBase(_id);
        // JoinPage();
        
    }

    public bool Join(string _id, string _pw)
    {
        if (DataBase.Instance.Login(_id, _pw))
        {
            UnityEngine.Debug.Log("�α��ο� �����߽��ϴ�.");
            GetDataBase(_id);

            // �׽�Ʈ �ڵ�
            peekabooLogin.SaveCharacterList();

            // �׽�Ʈ �ڵ�
            PeekabooDataBase.Instance.LoadPeekabooData();
            FeeFawFumDataBase.Instance.LoadFeefawfumData();
            FlyDragonDataBase.Instance.LoadFlyDragonData();
            CloverColonyDataBase.Instance.LoadCloverColonyData();

            DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.is_connect, "1", UserTableInfo.user_id, playerData.ID);
            

            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void OnApplicationQuit()
    {
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.is_connect, "0", UserTableInfo.user_id, playerData.ID);
    }

    //public void CreatePage()
    //{
    //    LoginUI.SetActive(false);
    //    CreateUI.SetActive(true);
    //}

    //public void JoinPage()
    //{
    //    LoginUI.SetActive(true);
    //    CreateUI.SetActive(false);
    //}

    public void SaveItemData() // Local -> DB
    {
        playerItemList = JsonUtility.ToJson(playerData.ItemSlotData);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.item, playerItemList, UserTableInfo.user_id, playerData.ID);
    }

    public void LoadItemData() // DB -> Local
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, playerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                playerItemList = row[UserTableInfo.item].ToString();
                playerData.ItemSlotData = JsonUtility.FromJson<ItemSlotData>(playerItemList);
            }
        }
    }

    public void SaveFriend()
    {
        friendList = JsonUtility.ToJson(playerData.Friends);
        UnityEngine.Debug.Log("friendList : " + friendList);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.friend, friendList, UserTableInfo.user_id, playerData.ID);
    }

    public void LoadFriend()
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, playerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                friendList = row[UserTableInfo.friend].ToString();
                playerData.Friends = JsonUtility.FromJson<FriendData>(friendList);
            }
        }
    }

    public void UpdateConnect(int _isConnect)
    {
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.is_connect, _isConnect.ToString(), UserTableInfo.user_id, playerData.ID); ;
    }

    // �׽�Ʈ �ڵ�
    public void GetDataBase(string _userID)
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, _userID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                playerData.UID = int.Parse(row[UserTableInfo.id].ToString());
                // ���̵�
                playerData.ID = row[UserTableInfo.user_id].ToString();
                // �г���
                playerData.Nickname = row[UserTableInfo.nickname].ToString();
                PhotonNetwork.NickName = playerData.Nickname;
                // ���� (�����)
                playerData.Coin =  int.Parse(row[UserTableInfo.coin].ToString());
                // ������
                playerItemList = row[UserTableInfo.item].ToString();
                playerData.ItemSlotData = JsonUtility.FromJson<ItemSlotData>(playerItemList);
                // ģ��
                friendList = row[UserTableInfo.friend].ToString();
                playerData.Friends = JsonUtility.FromJson<FriendData>(friendList);
            }
        }
    }
    // �׽�Ʈ �ڵ�

    public string FindUserID(string _name, string _birth)
    {
        string output = string.Empty;

        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*" ,UserTableInfo.name, _name);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if(row[UserTableInfo.birth].ToString() == _birth)
                {
                    output = row[UserTableInfo.id].ToString();
                }
                else
                {
                    // ������ ��ġ���� ����
                }
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            // �����Ͱ� ����
        }
        return output;        
    }

    public bool FindUserPW(string _id, string _hint, string _hintAnswer)
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, _id);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                string hint = row[UserTableInfo.hint].ToString();
                string hintAnswer = row[UserTableInfo.hint_answer].ToString();

                if (hint == _hint && hintAnswer  == _hintAnswer)
                {
                    return true;
                }
                else
                {
                    // ��Ʈ�� ��ġ���� ����
                }
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            // �����Ͱ� ����
        }
        return false;
    }
}
