using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Peekaboo_WatingRoomUIManager : SingletonBehaviour<Peekaboo_WatingRoomUIManager>
{
    public WaitingRoomUI WaitingRoomUI;
    public CreateRoomUI CreateRoomUI;
    public FindRoomUI FindRoomUI;
    public CustomizingUI CustomizingUI;
    public ExitUI ExitUI;
    public NotificationPopupUI notificationPopupUI;

    private void Awake()
    {
        Initionalize();
    }
    public void Initionalize()
    {
        WaitingRoomUI.gameObject.SetActive(true);
        CreateRoomUI.gameObject.SetActive(false);
        FindRoomUI.gameObject.SetActive(false);
        CustomizingUI.gameObject.SetActive(false);
        ExitUI.gameObject.SetActive(false);
    }

    // ¾Ë¸² ÆË¾÷ UI
    public void OpenNotificationPopupUI(int _id)
    {
        notificationPopupUI.OpenNotificationPopupUI(_id);
    }
}
