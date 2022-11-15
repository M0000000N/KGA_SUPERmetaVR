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

        int random = Random.Range(0, 100);
        if(random == 21)
        {
            RewardManager.Instance.GetItem();
        }
        // ³·Àº È®·ü·Î º¸»ó
    }

    [PunRPC]
    private void PlaySound()
    {
        myAudioSource.Stop();
        myAudioSource.Play();
    }
}