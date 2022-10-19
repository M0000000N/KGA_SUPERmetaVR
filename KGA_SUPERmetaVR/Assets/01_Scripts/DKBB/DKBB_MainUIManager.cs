using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DKBB_MainUIManager : MonoBehaviour
{
    [SerializeField] Button PKBButton;
    private void Start()
    {
        PKBButton.onClick.AddListener(OnclickPKBButton);
    }

    public void OnclickPKBButton()
    {
        PhotonNetwork.LoadLevel("PKB_MainUI");
    }
}
