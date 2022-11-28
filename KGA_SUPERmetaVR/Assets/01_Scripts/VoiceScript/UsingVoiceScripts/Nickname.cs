using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nickname : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI Owner_Nickname;
    PhotonView photonView;
    string OwnerNickname;
    string OtherNickname;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            OwnerNickname = photonView.Owner.NickName;
            Owner_Nickname.gameObject.SetActive(false);
        }
        else
        {
            OtherNickname = photonView.Owner.NickName;
            Owner_Nickname.text = OtherNickname;

            if (OtherNickname.Equals(null))
                return;
        }
    }

    private void Update()
    {
        Owner_Nickname.transform.forward = Camera.main.transform.forward;
    }
}
