using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class CharacterInfomation : MonoBehaviourPunCallbacks
{
    [SerializeField] private int uid;
    public int UID { get { return uid; } set { uid = value; } }

    [SerializeField] private TextMeshProUGUI nickNameText;

    [SerializeField] private string nickName;
    public string NickName { get { return nickName; } set { nickName = value; } }

    [SerializeField] private Button friendButton;

    PhotonView photonView;

    private void OnEnable()
    {
        photonView = PhotonView.Get(this);

        if (photonView.IsMine)
        {
            int id = GameManager.Instance.PlayerData.UID;
            string name = GameManager.Instance.PlayerData.Nickname;

            photonView.RPC("setInfo", RpcTarget.All, id, name);
        }

        friendButton.onClick.AddListener(() => { FriendManager.Instance.AddFriend(UID); } );
    }

    public void SetNickName()
    {
        nickNameText.text = nickName;

        List<int> friendList = GameManager.Instance.PlayerData.Friends.Friend;
        int friendCount = friendList.Count;
        for (int i = 0; i < friendCount; i++)
        {
            if(friendList[i] == UID)
            {
                nickNameText.color = new Color(40/255f, 60/255f, 252/255f);
                return;
            }
        }
        nickNameText.color = new Color(0, 0, 0);
    }

    // 이거 해봐야함...
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UnityEngine.Debug.Log("OnPlayerEnteredRoom은 : " + newPlayer);

        if (photonView.IsMine)
        {
            UnityEngine.Debug.Log("OnPlayerEnteredRoom은 : " + newPlayer);
            int id = GameManager.Instance.PlayerData.UID;
            string name = GameManager.Instance.PlayerData.Nickname;

            photonView.RPC("setInfo", newPlayer, id, name);
        }
    }

    [PunRPC]
    public void setInfo(int _id, string _name)
    {
        UID = _id;
        NickName = _name;
    }
}
