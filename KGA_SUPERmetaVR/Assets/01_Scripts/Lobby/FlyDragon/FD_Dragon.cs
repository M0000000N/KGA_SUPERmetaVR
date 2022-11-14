using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FD_Dragon : MonoBehaviour
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

    public void ParticleOn()
    {
        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].Play();
        }
    }
}
