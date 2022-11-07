using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FirendManager : MonoBehaviour
{
    [SerializeField] private GameObject friendListObject;
    [SerializeField] private Transform viewportTransform;
    [SerializeField] private GameObject friendContent;

    PlayerData playerData;

    private void Awake()
    {
        playerData = GameManager.Instance.PlayerData;
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 1; i < 20; i++) // TODO : 친구 최대 수 입력 필요
        {
            Instantiate(friendContent, viewportTransform);
        }
    }

    public void OpenList()
    {
        UpdateFriendData();
        friendListObject.SetActive(true);
    }

    public void CloseList()
    {
        friendListObject.SetActive(false);
    }

    public void AddFriend(int _id)
    {
        int index = FindFriend(_id);

        if (index < 0)
        {
            playerData.Friends.Friend.Add(_id);
        }
        else
        {
            UnityEngine.Debug.Log("이미 등록된 친구입니다.");
        }
    }

    public void DeleteFriend(FriendList _friendList)
    {
        int index = FindFriend(_friendList.ID);

        if (index > 0)
        {
            playerData.Friends.Friend.RemoveAt(index);
        }
    }

    public int FindFriend(int _id)
    {
        for (int i = 0; i < playerData.Friends.Friend.Count; i++)
        {
            if(playerData.Friends.Friend[i] == _id)
            {
                return i;
            }
        }
        return -1;
    }

    public void UpdateFriendData()
    {
        int contentCount = viewportTransform.childCount;

        for (int i = 0; i < contentCount; i++)
        {
            GameObject content = viewportTransform.GetChild(i).gameObject;
            if (content.activeSelf)
            {
                content.SetActive(false);
            }
        }

        DataTable dataTable = DataBase.Instance.selsql($"SELECT {UserTableInfo.id}, {UserTableInfo.nickname}, {UserTableInfo.is_connect} FROM {UserTableInfo.table_name}");

        for (int i = 0; i < playerData.Friends.Friend.Count; i++)
        {
            if (i > contentCount) break;

            GameObject content = viewportTransform.GetChild(i).gameObject;
            FriendList friendList = content.GetComponent<FriendList>();

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (playerData.Friends.Friend[i] == int.Parse(row[UserTableInfo.id].ToString()))
                    {
                        friendList.ID = int.Parse(row[UserTableInfo.id].ToString());
                        friendList.NickNameText.text = row[UserTableInfo.nickname].ToString();

                        if(int.Parse(row[UserTableInfo.is_connect].ToString()) > 0)
                        {
                            friendList.ConnectImage.SetActive(true);
                        }
                        else
                        {
                            friendList.ConnectImage.SetActive(false);
                        }
                    }
                }
            }
            content.SetActive(true);
        }
    }

    public bool IsConnect(DataTable _dataTable, int _userID)
    {
        if (_dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in _dataTable.Rows)
            {
                    if (_userID == int.Parse(row[UserTableInfo.user_id].ToString()))
                    {
                        return true;
                    }
            }
        }
        return false;
    }
}
