using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pumpkin : MonoBehaviourPun
{
    private AudioSource myAudioSource;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision _collision)
    {
        photonView.RPC("PlaySound", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void PlaySound()
    {
        myAudioSource.Stop();
        myAudioSource.Play();
    }
}