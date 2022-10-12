using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform playerNameParent;
    [SerializeField] PlayerName playerName;

    private List<PlayerName> playerNameList = new List<PlayerName>();

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerName listing = Instantiate(playerName, playerNameParent);

        if (listing != null)
        {
            listing.SetPlayerInfo(newPlayer);
            playerNameList.Add(listing);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerNameList.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(playerNameList[index].gameObject);
            playerNameList.RemoveAt(index);
        }
    }
}
