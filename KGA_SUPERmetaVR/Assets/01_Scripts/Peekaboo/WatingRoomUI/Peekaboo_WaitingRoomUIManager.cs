using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Peekaboo_WaitingRoomUIManager : SingletonBehaviour<Peekaboo_WaitingRoomUIManager>
{
    public WaitingRoomUI WaitingRoomUI;
    public CreateRoomUI CreateRoomUI;
    public FindRoomUI FindRoomUI;
    public Peekaboo_CustomizingUI CustomizingUI;
    public ExitUI ExitUI;
    public NoticePopupUI NoticePopupUI;
    public Peekaboo_SettingUI SettingUI;
    public PlayRoomUI PlayRoomUI;
    public Peekaboo_RankingUI RankingUI;

    // public bool IsPrivateRoom;

    private void Awake()
    {
        // IsPrivateRoom = false;
    }
    private void Start()
    {        
        WaitingRoomUI   = GetComponentInChildren<WaitingRoomUI>();
        CustomizingUI   = GetComponentInChildren<Peekaboo_CustomizingUI>();
        CreateRoomUI    = GetComponentInChildren<CreateRoomUI>();
        ExitUI          = GetComponentInChildren<ExitUI>();
        FindRoomUI      = GetComponentInChildren<FindRoomUI>();
        NoticePopupUI   = GetComponentInChildren<NoticePopupUI>();
        PlayRoomUI      = GetComponentInChildren<PlayRoomUI>();
        SettingUI       = GetComponentInChildren<Peekaboo_SettingUI>();
        RankingUI       = GetComponentInChildren<Peekaboo_RankingUI>();

        Initionalize();
    }
    public void Initionalize()
    {
        WaitingRoomUI.gameObject.SetActive(true);
        CustomizingUI.gameObject.SetActive(false);
        CreateRoomUI.gameObject.SetActive(false);
        ExitUI.gameObject.SetActive(false);
        FindRoomUI.gameObject.SetActive(false);
        NoticePopupUI.gameObject.SetActive(false);
        PlayRoomUI.gameObject.SetActive(false);
        SettingUI.gameObject.SetActive(false);
        RankingUI.gameObject.SetActive(false);
    }

    // ¾Ë¸² ÆË¾÷ UI
    public void OpenNotificationPopupUI(int _id)
    {
        NoticePopupUI.OpenNotificationPopupUI(_id);
    }
}
