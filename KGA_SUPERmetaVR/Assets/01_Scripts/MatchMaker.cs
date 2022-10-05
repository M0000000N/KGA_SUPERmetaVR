using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchMaker : MonoBehaviourPunCallbacks
{
	public GameObject photonObject;


	void Start () {

		Debug.Log ("start");
		PhotonNetwork.ConnectUsingSettings ();
        PhotonNetwork.GameVersion = "0.1";
	}


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");

        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");

		float randomX = Random.Range(-6f, 6f); 

        PhotonNetwork.Instantiate(
			photonObject.name,
			new Vector3(randomX, 1f, 0f),
			Quaternion.identity, 
			0
		);		
    }

	public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null);

		//PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 4});
    } 

}
