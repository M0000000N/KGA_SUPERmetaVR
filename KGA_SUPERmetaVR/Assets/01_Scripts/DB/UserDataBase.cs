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

    public static readonly string create_at = "create_at"; // join_date와 동일
    public static readonly string update_at = "update_at"; // 최근 정보가 변경된 시점
    public static readonly string is_delete = "is_delete";

    public static readonly string peekaboo = "peekaboo";
}

public class UserDataBase : MonoBehaviour
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

    private PlayerData playerData;
    private string playerItemList;
    
    // 테스트 코드
    private PeekabooDataBase peekabooLogin;

    private void Awake()
    {
        peekabooLogin = transform.GetComponent<PeekabooDataBase>();
        playerData = GameManager.Instance.PlayerData;
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

            // TestCode 저장
            playerItemList = JsonUtility.ToJson(playerData.ItemSlotData);
            UnityEngine.Debug.Log("ItemList : " + playerItemList);
            DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.item, playerItemList, UserTableInfo.user_id, CreateID.text);
            // TestCode


            GetDataBase(CreateNickName.text);
            
            FeeFawFumDataBase.Instance.CreateFeefawfumData();
            PaperSwanDataBase.Instance.CreatePaperswanData();
            CloverColonyDataBase.Instance.CreateCloverColonyData();

            JoinPage();


        }
    }

    public void Join()
    {
        if (DataBase.Instance.Login(LoginID.text, LoginPW.text))
        {
            UnityEngine.Debug.Log("로그인에 성공했습니다.");
            GetDataBase(LoginID.text);

            // 테스트 코드
            peekabooLogin.SaveCharacterList();
            // 테스트 코드

            PeekabooDataBase.Instance.LoadPeekabooData();
            FeeFawFumDataBase.Instance.LoadFeefawfumData();
            PaperSwanDataBase.Instance.LoadPaperswanData();
            CloverColonyDataBase.Instance.LoadCloverColonyData();

            PhotonNetwork.LoadLevel("InventoryRDScene_RWJ");
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

    public void SaveItemData() // Local -> DB
    {
        playerItemList = JsonUtility.ToJson(playerData.ItemSlotData);
        UnityEngine.Debug.Log("ItemList : " + playerItemList);
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

    // 테스트 코드
    public void GetDataBase(string _userID)
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, _userID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                playerData.ID = row[UserTableInfo.user_id].ToString();
                playerData.Nickname = row[UserTableInfo.nickname].ToString();
                PhotonNetwork.NickName = playerData.Nickname;
                playerData.Coin =  int.Parse(row[UserTableInfo.coin].ToString());
                playerItemList = row[UserTableInfo.item].ToString();
                playerData.ItemSlotData = JsonUtility.FromJson<ItemSlotData>(playerItemList);
            }
        }
    }
    // 테스트 코드


}
