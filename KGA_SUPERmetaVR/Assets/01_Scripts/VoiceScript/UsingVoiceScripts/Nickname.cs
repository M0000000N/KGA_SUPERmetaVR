using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nickname : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI Owner_Nickname;

    private void Update()
    {
        Owner_Nickname.transform.forward = Camera.main.transform.forward;
    }
}
