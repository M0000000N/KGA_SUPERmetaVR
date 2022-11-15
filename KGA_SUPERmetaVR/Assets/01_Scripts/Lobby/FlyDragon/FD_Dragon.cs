using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FD_Dragon : MonoBehaviourPun
{
    private ParticleSystem[] particle;
    private Rigidbody rigidbody;

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

    public void ParticlePUN(string _name)
    {
        photonView.RPC(_name, RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void ParticleOn()
    {
        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].Play();
        }
    }

    public void DestroyStar()
    {
        gameObject.SetActive(false);
    }
}