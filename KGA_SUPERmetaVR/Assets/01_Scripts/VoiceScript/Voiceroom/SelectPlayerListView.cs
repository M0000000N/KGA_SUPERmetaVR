using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerListView : MonoBehaviour
{
    [SerializeField]
    Dropdown dropDown;

    int beforePlayerListLength = 0;

    private void Update()
    {
        if (beforePlayerListLength != PhotonNetwork.PlayerList.Length)
        {
            beforePlayerListLength = PhotonNetwork.PlayerList.Length;
            dropDown.ClearOptions();

            List<string> names = new List<string>();
            names.Add("All");
            foreach (var player in PhotonNetwork.PlayerList)
            {
                names.Add(player.ActorNumber.ToString());
            }

            dropDown.AddOptions(names);
        }
    }
}
