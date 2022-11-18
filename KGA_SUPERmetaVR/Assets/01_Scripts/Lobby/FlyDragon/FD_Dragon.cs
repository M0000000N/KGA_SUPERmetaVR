using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class FD_Dragon : MonoBehaviourPun
{
    private ParticleSystem[] particle;
    private Rigidbody rigidbody;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material fadeMaterial;
    private float fadeoutTime = 2f;
    private bool isStartFadedout = false;
    private bool isParticlePlay = false;

    public bool IsStartFadedout
    {
        get
        {
            return isStartFadedout;
        }
        set
        {
            isStartFadedout = value;
            if (isStartFadedout && gameObject.activeSelf)
            {
                photonView.RPC("StartFadeOut", RpcTarget.AllViaServer);
            }
        }
    }

    public bool IsParticlePlay
    {
        get
        {
            return isParticlePlay;
        }
        set
        {
            isParticlePlay = value;
            if (isParticlePlay && gameObject.activeSelf)
            {
                photonView.RPC("ParticleOn", RpcTarget.AllViaServer);
            }
        }
    }

    void Start()
    {
        particle = GetComponentsInChildren<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y >= 40)
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    [PunRPC]
    public void ParticleOn()
    {
        rigidbody.isKinematic = false;
        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].Play();
        }
    }

    [PunRPC]
    private void StartFadeOut()
    {
        StartCoroutine(FadeoutCoroutine());
    }

    public IEnumerator FadeoutCoroutine()
    {
        gameObject.GetComponent<XRGrabInteractable>().enabled = false;

        MeshRenderer myRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        myRenderer.material = fadeMaterial;
        Color myColor = myRenderer.material.color;

        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= 0.1f / fadeoutTime;
            myRenderer.material.color = myColor;
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
        myRenderer.material = defaultMaterial;
        gameObject.GetComponent<XRGrabInteractable>().enabled = true;
        isStartFadedout = false;
    }
}