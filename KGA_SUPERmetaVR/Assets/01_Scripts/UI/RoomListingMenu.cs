using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [Header("½ºÅ©·Ñ ºä")]
    [SerializeField] Transform roomBtnParent;
    [SerializeField] RoomButton roomBtnPref;

    private List<RoomButton> roomButtonList = new List<RoomButton>();


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList) // ·ë Áö¿üÀ» ¶§
            {
                int index = roomButtonList.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(roomButtonList[index].gameObject);
                    roomButtonList.RemoveAt(index);
                }
            }
            else // ·ë Ãß°¡ÇßÀ» ¶§
            {
                RoomButton listing = (RoomButton)Instantiate(roomBtnPref, roomBtnParent);
                if (listing != null)
                {
                    listing.SetRoomInfo(info);
                }
            }
        }
    }
}
