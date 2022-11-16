using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CharacterInfomation : MonoBehaviourPunCallbacks
{
    [SerializeField] private int uid;
    public int UID { get { return uid; } set { uid = value; } }

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
