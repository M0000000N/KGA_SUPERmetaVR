using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PKB_MainUIManager : SingletonBehaviour<PKB_MainUIManager>
{
    public PKB_CreateRoomUI CreateRoomUI;
    public PKB_CustomizingUI CustomizingUI;
    public PKB_ExitUI ExitUI;
    public PKB_FindRoomUI FindRoomUI;
    public PKB_MainUI MainUI;
    public PKB_NoticePopupUI NoticePopupUI;
    public PKB_PlayRoomUI PlayRoomUI;
    public PKB_RankingUI RankingUI;
    public PKB_SettingUI SettingUI;

    private void Awake()
    {
        CreateRoomUI     = GetComponentInChildren<PKB_CreateRoomUI>();
        CustomizingUI    = GetComponentInChildren<PKB_CustomizingUI>();
        ExitUI           = GetComponentInChildren<PKB_ExitUI>();
        FindRoomUI       = GetComponentInChildren<PKB_FindRoomUI>();
        MainUI           = GetComponentInChildren<PKB_MainUI>();
        NoticePopupUI    = GetComponentInChildren<PKB_NoticePopupUI>();
        PlayRoomUI       = GetComponentInChildren<PKB_PlayRoomUI>();
        RankingUI        = GetComponentInChildren<PKB_RankingUI>();
        SettingUI        = GetComponentInChildren<PKB_SettingUI>();
    }
    private void Start()
    {
        Initionalize();
    }
    public void Initionalize()
    {
        CreateRoomUI.gameObject.SetActive(false);
        CustomizingUI.gameObject.SetActive(false);
        ExitUI.gameObject.SetActive(false);
        FindRoomUI.gameObject.SetActive(false);
        MainUI.gameObject.SetActive(true);
        NoticePopupUI.gameObject.SetActive(false);
        PlayRoomUI.gameObject.SetActive(false);
        RankingUI.gameObject.SetActive(false);
        SettingUI.gameObject.SetActive(false);
    }

    // ¾Ë¸² ÆË¾÷ UI
    public void OpenNotificationPopupUI(int _id)
    {
        NoticePopupUI.OpenNotificationPopupUI(_id);
    }
}
