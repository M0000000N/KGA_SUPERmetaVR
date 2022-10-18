using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PKB_ExitUI : MonoBehaviour
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYesButton);
        noButton.onClick.AddListener(OnClickNoButton);
    }
    public void OnClickYesButton()
    {
        gameObject.SetActive(false);
        PhotonNetwork.LoadLevel("DKBB");
    }

    public void OnClickNoButton()
    {
        gameObject.SetActive(false);
    }
}

