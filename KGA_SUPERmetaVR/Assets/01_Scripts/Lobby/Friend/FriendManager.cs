using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class FriendManager : SingletonBehaviour<FriendManager>
{
    [SerializeField] private GameObject friendListObject;
    [SerializeField] private Transform viewportTransform;
    [SerializeField] private GameObject friendContent;

    private PlayerData playerData;
    private int objectUID;

    private int targetUID;
    private string targetNickName;
    private int requestTargetUID;
    private string requestTargetNickName;
    private PhotonView photonView;

    private void Awake()
    {
        playerData = GameManager.Instance.PlayerData;
        photonView = PhotonView.Get(this);
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

    // 각 플레이어의 데이터를 갖고 있어야한다.
    // 버튼을 눌렀을 때
    // 해당하는 오브젝트의 플레이어 데이터를 불러와야 한다.

    
    public void AddFriend(int _uid)
    {
        int index = FindFriend(_uid);

        if (index < 0)
        {
            playerData.Friends.Friend.Add(_uid);
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
        UpdateFriendData();
    }

    public int FindFriend(int _id)
    {
        for (int i = 0; i < playerData.Friends.Friend.Count; i++)
        {
            if (playerData.Friends.Friend[i] == _id)
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

                        if (int.Parse(row[UserTableInfo.is_connect].ToString()) > 0)
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

    // 친구 추가 요청
    public void CheckRequest(int _targetUID, string _targetNickname)
    {
        requestTargetUID = _targetUID;
        requestTargetNickName = _targetNickname;
        RequestPopupUI.Instance.SetPopup(52000, _targetNickname, SendRequest);
    }

    public void SendRequest()
    {
        photonView.RPC("SendRequestMessage", RpcTarget.Others, requestTargetUID, playerData.UID, playerData.Nickname);
        RequestPopupUI.Instance.SetPopup(52001, requestTargetNickName);
    }

    [PunRPC]
    public void SendRequestMessage(int _playerUID, int _targetUID, string _targetNickName)
    {
        UnityEngine.Debug.Log("메크로");

        targetUID = _targetUID;
        targetNickName = _targetNickName;

        if (playerData.UID == _playerUID)
        {
            UnityEngine.Debug.Log("메크로");
            RequestPopupUI.Instance.SetPopup(52004, _targetNickName, Approve, Reject);
        }
    }

    public void Approve()
    {
        photonView.RPC("ApproveMessage", RpcTarget.Others, targetUID, playerData.UID, playerData.Nickname);
        AddFriend(targetUID);
        // 친구등록
    }

    public void Reject()
    {
        photonView.RPC("RejectMessage", RpcTarget.Others, targetUID, playerData.Nickname);
    }

    [PunRPC]
    public void ApproveMessage(int _uid, int _targetUID, string _nickname)
    {
        if (playerData.UID == _uid)
        {
            RequestPopupUI.Instance.SetPopup(52002, _nickname);
            AddFriend(_targetUID);
            // 친구등록
        }
    }

    [PunRPC]
    public void RejectMessage(string _uid, string _nickname)
    {
        if (playerData.UID.ToString() == _uid)
        {
            RequestPopupUI.Instance.SetPopup(52003, _nickname);
        }
    }
}
