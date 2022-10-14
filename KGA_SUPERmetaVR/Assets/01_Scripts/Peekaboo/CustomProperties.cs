using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CustomProperties : MonoBehaviourPunCallbacks
{

    // 커스텀 프로퍼티를 일시 저장한다
    private string text = "";


    public readonly RoomOptions RoomOptions = new RoomOptions()
    {
        IsOpen = true,
        IsVisible = true,
        MaxPlayers = 14
    };


    private ExitGames.Client.Photon.Hashtable myCustoProperties = new ExitGames.Client.Photon.Hashtable();
    void OnPhotonRandomJoinFailed()
    {
        // 랜덤으로 입실 실패한 경우, 룸을 작성
        // 룸 옵션의 작성
        RoomOptions.IsVisible = true;
        RoomOptions.IsOpen = true;
        RoomOptions.MaxPlayers = 4;

        RoomOptions.CustomRoomProperties = new Hashtable() { { "CustomProperties", "커스텀 프로퍼티" } };
        RoomOptions.CustomRoomPropertiesForLobby = new string[] { "CustomProperties" };
        // 룸의 작성
        PhotonNetwork.CreateRoom("CustomPropertiesRoom", RoomOptions, null);
    }

    void OnGUI()
    {
        // 룸에 있는 경우만
        if (PhotonNetwork.InRoom)
        {
            // 룸의 상태를 취득
            Room room = PhotonNetwork.CurrentRoom;
            if (room == null)
            {
                return;
            }
            // 룸의 커스텀 프로퍼티를 취득
            Hashtable customProperties = room.CustomProperties;
            GUILayout.Label((string)customProperties["CustomProperties"], GUILayout.Width(150));
            text = GUILayout.TextField(text, 100, GUILayout.Width(150));

            // 커스텀 프로퍼티를 갱신
            if (GUILayout.Button("갱신"))
            {
                customProperties["CustomProperties"] = text;
                room.SetCustomProperties(customProperties);
            }
        }
    }
}
