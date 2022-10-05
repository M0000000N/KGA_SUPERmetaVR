using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class NicknameTextUI : MonoBehaviourPunCallbacks, IPunObservable
{
    private TextMeshProUGUI nickname;
    private int _clickCount = 0;
    private string _nickname;

    public int ClickCount
    {
        get { return _clickCount; }
        set
        {
            _clickCount = value;
            nickname.text = $"{Nickname} ���� : {_clickCount} ";
        }
    }

    public string Nickname
    {
        get { return _nickname; }
        set { _nickname = value; }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ����ȭ -> �������� �����͸� ������ ��
        if (stream.IsWriting)
        {
            stream.SendNext(ClickCount);
        }
        else // ������ȭ -> �����κ��� �����͸� ���� ��
        {
            ClickCount = (int)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        nickname = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Data data = FindObjectOfType<Data>();
        if (photonView.IsMine)
        {
            Nickname = data.Nickname;
            ClickCount = 0;
            photonView.RPC("SetNickname", RpcTarget.Others, Nickname);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && photonView.IsMine)
        {
            ++ClickCount;
        }
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        Nickname = nickname;
    }

    // �濡 �������� ��
    public override void OnJoinedRoom()
    {
        // �濡 �ִ� �÷��̾� ��ο��� �� �̸��� �����Ѵ�.
        photonView.RPC("SetNickname", RpcTarget.All, Nickname);
    }

    // ���ο� �÷��̾ �������� ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���ο� �÷��̾�� �� �̸��� �����Ѵ�.
        photonView.RPC("SetNickname", newPlayer, Nickname);
    }
}
