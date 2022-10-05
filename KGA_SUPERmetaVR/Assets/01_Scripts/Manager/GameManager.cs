using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject PlayerPrefeb;
    public Button exitButton;
    public void Start()
    {
        float randomPosX = Random.Range(-30f, 30f);
        float randomPosZ = Random.Range(-30f, 30f);
        Vector3 randomPos = new Vector3(randomPosX, 1f, randomPosZ);

        GameObject playerObject = PhotonNetwork.Instantiate(PlayerPrefeb.name, randomPos, Quaternion.identity);

        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }
}
