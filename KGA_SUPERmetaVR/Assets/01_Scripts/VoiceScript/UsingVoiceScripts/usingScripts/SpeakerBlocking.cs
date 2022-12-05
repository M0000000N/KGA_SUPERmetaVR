using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpeakerBlocking : MonoBehaviourPun
{
    [SerializeField] Toggle SpeakerMuteonoff;
    [SerializeField] AudioSource audiosource;

    private void Start()
    {
        SpeakerMuteonoff.onValueChanged.AddListener(Speakermute);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (photonView.IsMine == false) return;
            audiosource = other.transform.GetChild(2).GetComponent<AudioSource>();
        }
    }

    public void Speakermute(bool _ison)
    {
        if (_ison)
        {
            audiosource.volume = 0;
        }
        else
        {
            audiosource.volume = 1;
        }
    }

}
