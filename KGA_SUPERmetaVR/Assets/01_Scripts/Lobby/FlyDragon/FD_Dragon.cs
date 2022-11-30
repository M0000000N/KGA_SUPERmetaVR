using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class FD_Dragon : MonoBehaviourPun
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material fadeMaterial;
    [SerializeField] ParticleSystem[] grabParticle;
    [SerializeField] ParticleSystem[] flyParticle;
    private Rigidbody rigidbody;
    private float fadeoutTime = 2f;
    private bool isStartFadedout = false;
    private bool isFlyParticlePlay = false;
    private bool isGrabParticlePlay = false;

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

    public bool IsFlyParticlePlay
    {
        get
        {
            return isFlyParticlePlay;
        }
        set
        {
            isFlyParticlePlay = value;
            if (isFlyParticlePlay && gameObject.activeSelf)
            {
                photonView.RPC("GrabParticleOn", RpcTarget.AllViaServer);
            }
        }
    }

    public bool IsGrabParticlePlay
    {
        get
        {
            return isGrabParticlePlay;
        }
        set
        {
            isGrabParticlePlay = value;
            if (isGrabParticlePlay && gameObject.activeSelf)
            {
                photonView.RPC("FlyParticleOn", RpcTarget.AllViaServer);
            }
        }
    }

    void Start()
    {
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
    public void GrabParticleOn()
    {
        rigidbody.isKinematic = false;
        for (int i = 0; i < grabParticle.Length; i++)
        {
            grabParticle[i].Play();
        }
    }

    [PunRPC]
    public void FlyParticleOn()
    {
        rigidbody.isKinematic = false;
        for (int i = 0; i < flyParticle.Length; i++)
        {
            flyParticle[i].Play();
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