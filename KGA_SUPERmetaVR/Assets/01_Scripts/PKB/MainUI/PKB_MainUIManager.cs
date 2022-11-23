using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PKB_MainUIManager : OnlyOneSceneSingleton<PKB_MainUIManager>
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
    public GameObject Loading { get { return loading; } set { loading = value; } }
    [SerializeField] GameObject loading;

    private void Awake()
    {
        CreateRoomUI = GetComponentInChildren<PKB_CreateRoomUI>();
        CustomizingUI = GetComponentInChildren<PKB_CustomizingUI>();
        ExitUI = GetComponentInChildren<PKB_ExitUI>();
        FindRoomUI = GetComponentInChildren<PKB_FindRoomUI>();
        MainUI = GetComponentInChildren<PKB_MainUI>();
        NoticePopupUI = GetComponentInChildren<PKB_NoticePopupUI>();
        PlayRoomUI = GetComponentInChildren<PKB_PlayRoomUI>();
        RankingUI = GetComponentInChildren<PKB_RankingUI>();
        SettingUI = GetComponentInChildren<PKB_SettingUI>();
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

    // 알림 팝업 UI
    public void OpenNotificationPopupUI(int _id)
    {
        NoticePopupUI.OpenNotificationPopupUI(_id);
    }

    // 로딩
    public void Fade(bool _isFadeIn)
    {
        if (_isFadeIn)
        {
            StopCoroutine(LoadingFadeInCoroutine());
            StartCoroutine(LoadingFadeInCoroutine());
        }
        else
        {
            StopCoroutine(LoadingFadeOutCoroutine());
            StartCoroutine(LoadingFadeOutCoroutine());
        }
    }

    IEnumerator LoadingFadeInCoroutine()
    {
        Image image = Loading.GetComponent<Image>();
        float alpha = 1f;
        image.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        while (true)
        {
            alpha -= 0.1f;
            image.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.05f);
            if (alpha <= 0) break;
        }
        image.gameObject.SetActive(false);
        yield break;
    }

    IEnumerator LoadingFadeOutCoroutine()
    {
        Image image = Loading.GetComponent<Image>();
        image.gameObject.SetActive(true);
        float alpha = 0f;
        image.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        while (true)
        {
            alpha += 0.1f;
            image.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.05f);
            if (alpha >= 1) break;
        }
        yield break;
    }
}
