using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class FirendManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject friendListObject;
    [SerializeField] private Transform viewportTransform;
    [SerializeField] private GameObject friendContent;

    private PlayerData playerData;
    private string targetUID;
    private string targetNickName;
    private string requestTargetUID;
    private string requestTargetNickName;
    private PhotonView photonView;

    private void Update()
    {

    }

    private void Awake()
    {
        playerData = GameManager.Instance.PlayerData;
        photonView = PhotonView.Get(this);
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
    public void CheckRequest(string _targetUID, string _targetNickname)
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
    public void SendRequestMessage(string _playerUID, string _targetUID, string _targetNickName)
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
        AddFriend(int.Parse(targetUID));
        // 친구등록
    }

    public void Reject()
    {
        photonView.RPC("RejectMessage", RpcTarget.Others, targetUID, playerData.Nickname);
    }

    [PunRPC]
    public void ApproveMessage(string _uid, string _targetUID, string _nickname)
    {
        if (playerData.UID == _uid)
        {
            RequestPopupUI.Instance.SetPopup(52002, _nickname);
            AddFriend(int.Parse(_targetUID));
            // 친구등록
        }
    }

    [PunRPC]
    public void RejectMessage(string _uid, string _nickname)
    {
        if (playerData.UID == _uid)
        {
            RequestPopupUI.Instance.SetPopup(52003, _nickname);
        }
    }
}
