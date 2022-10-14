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

    // Ŀ���� ������Ƽ�� �Ͻ� �����Ѵ�
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
        // �������� �Խ� ������ ���, ���� �ۼ�
        // �� �ɼ��� �ۼ�
        RoomOptions.IsVisible = true;
        RoomOptions.IsOpen = true;
        RoomOptions.MaxPlayers = 4;

        RoomOptions.CustomRoomProperties = new Hashtable() { { "CustomProperties", "Ŀ���� ������Ƽ" } };
        RoomOptions.CustomRoomPropertiesForLobby = new string[] { "CustomProperties" };
        // ���� �ۼ�
        PhotonNetwork.CreateRoom("CustomPropertiesRoom", RoomOptions, null);
    }

    void OnGUI()
    {
        // �뿡 �ִ� ��츸
        if (PhotonNetwork.InRoom)
        {
            // ���� ���¸� ���
            Room room = PhotonNetwork.CurrentRoom;
            if (room == null)
            {
                return;
            }
            // ���� Ŀ���� ������Ƽ�� ���
            Hashtable customProperties = room.CustomProperties;
            GUILayout.Label((string)customProperties["CustomProperties"], GUILayout.Width(150));
            text = GUILayout.TextField(text, 100, GUILayout.Width(150));

            // Ŀ���� ������Ƽ�� ����
            if (GUILayout.Button("����"))
            {
                customProperties["CustomProperties"] = text;
                room.SetCustomProperties(customProperties);
            }
        }
    }
}
