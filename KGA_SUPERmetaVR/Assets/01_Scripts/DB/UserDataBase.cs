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

    public static readonly string create_at = "create_at"; // join_date와 동일
    public static readonly string update_at = "update_at"; // 최근 정보가 변경된 시점
    public static readonly string is_delete = "is_delete";

    public static readonly string peekaboo = "peekaboo";
}

public class UserDataBase : SingletonBehaviour<UserDataBase>
{
    //[Header("로그인")]
    //public TMP_InputField LoginID;
    //public TMP_InputField LoginPW;
    //public GameObject LoginUI;

    //[Header("회원가입")]
    //public TMP_InputField CreateID;
    //public TMP_InputField CreatePW;
    //public TMP_InputField CreatePWCheck;
    //public TMP_InputField CreateNickName;
    //public GameObject CreateUI;

    private PlayerData playerData;
    private string playerItemList;
    private string friendList;

    // 테스트 코드
    private PeekabooDataBase peekabooLogin;

    private void Awake()
    {
        peekabooLogin = transform.GetComponent<PeekabooDataBase>();
        playerData = GameManager.Instance.PlayerData;
    }
    // 테스트 코드

    public void Create(string _id, string _pw, string _name, string _birth, string _hint, string _hintAnswer)
    {
        DataBase.Instance.CreateUser(_id, _pw, _name, _birth, _hint, _hintAnswer);

        // --------------------------------------------------------------------------------------------- 테스트 코드
        // 만들 때 PlayerData에 값이 없기 때문에 CreateID를 통해 데이터를 넣어주고 있다.

        // [아이템]
        // DB 생성
        FeeFawFumDataBase.Instance.CreateFeefawfumData(_id);
        FlyDragonDataBase.Instance.CreateFlyDragonData(_id);
        CloverColonyDataBase.Instance.CreateCloverColonyData(_id);
        PeekabooDataBase.Instance.CreatePeekabooData(_id);

        // TestCode 저장
        playerItemList = JsonUtility.ToJson(playerData.ItemSlotData);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.item, playerItemList, UserTableInfo.user_id, _id);

        // [친구]
        friendList = JsonUtility.ToJson(playerData.Friends);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.friend, friendList, UserTableInfo.user_id, _id);

        // --------------------------------------------------------------------------------------------- 테스트 코드

        GetDataBase(_id);
        // JoinPage();
        
    }

    public bool Join(string _id, string _pw)
    {
        if (DataBase.Instance.Login(_id, _pw))
        {
            UnityEngine.Debug.Log("로그인에 성공했습니다.");
            GetDataBase(_id);

            // 테스트 코드
            peekabooLogin.SaveCharacterList();

            // 테스트 코드
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

    // 테스트 코드
    public void GetDataBase(string _userID)
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, _userID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                playerData.UID = int.Parse(row[UserTableInfo.id].ToString());
                // 아이디
                playerData.ID = row[UserTableInfo.user_id].ToString();
                // 닉네임
                playerData.Nickname = row[UserTableInfo.nickname].ToString();
                PhotonNetwork.NickName = playerData.Nickname;
                // 코인 (사라짐)
                playerData.Coin =  int.Parse(row[UserTableInfo.coin].ToString());
                // 아이템
                playerItemList = row[UserTableInfo.item].ToString();
                playerData.ItemSlotData = JsonUtility.FromJson<ItemSlotData>(playerItemList);
                // 친구
                friendList = row[UserTableInfo.friend].ToString();
                playerData.Friends = JsonUtility.FromJson<FriendData>(friendList);
            }
        }
    }
    // 테스트 코드

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
                    // 생일이 일치하지 않음
                }
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            // 데이터가 없음
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
                    // 힌트가 일치하지 않음
                }
            }
        }
        else if (dataTable.Rows.Count <= 0)
        {
            // 데이터가 없음
        }
        return false;
    }
}
