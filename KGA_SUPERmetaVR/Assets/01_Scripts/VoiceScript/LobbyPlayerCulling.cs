 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyPlayerCulling : MonoBehaviourPun
{

    [SerializeField]
    private GameObject LobbyPlayerModeling;
    private SkinnedMeshRenderer[] LobbyPlayerModelings;

    private void Awake()
    {
        LobbyPlayerModelings = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var children in LobbyPlayerModelings)
        {
            children.gameObject.SetActive(false);
        }
        LobbyPlayerModelings[0].gameObject.SetActive(true);
        LobbyPlayerModeling = LobbyPlayerModelings[0].gameObject;
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;
    }

    private void Start()
    {
        if (photonView.IsMine == true && PhotonNetwork.IsConnected == true)
            CullingModeling();
    }

    public void CullingModeling()
    {
        LobbyPlayerModeling.SetActive(false);
    }
}
