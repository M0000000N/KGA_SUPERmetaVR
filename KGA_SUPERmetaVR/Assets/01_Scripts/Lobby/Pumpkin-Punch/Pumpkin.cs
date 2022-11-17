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
        Debug.Log(_collision.gameObject.name);
        
        if (_collision.gameObject.tag == "PPun_Hand")
        {
            photonView.RPC("PlaySound", RpcTarget.AllViaServer);

            int random = Random.Range(0, 100);
            if(random < 10) // QA CODE : == 21
            {
                RewardManager.Instance.GetItem();
            }
        }
    }

    [PunRPC]
    private void PlaySound()
    {
        myAudioSource.Stop();
        myAudioSource.Play();
    }
}