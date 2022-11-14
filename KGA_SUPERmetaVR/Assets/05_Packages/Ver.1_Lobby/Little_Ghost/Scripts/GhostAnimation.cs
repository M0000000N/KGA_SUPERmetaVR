using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimation : MonoBehaviour
{
    public Animator[] animators;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void idlePress()
    {
        foreach(Animator i in animators)
        {
            i.Play("Idle");
        }
    }
    public void ANim2Press()
    {
        foreach (Animator i in animators)
        {
            i.Play("Look");
        }
    }
    public void ANim3Press()
    {
        foreach (Animator i in animators)
        {
            i.Play("Hit");
        }
    }
    public void ANim4Press()
    {
        foreach (Animator i in animators)
        {
            i.Play("Attack");
        }
    }
    public void ANim5Press()
    {
        foreach (Animator i in animators)
        {
            i.Play("Hide");
        }
    }
    public void ANim6Press()
    {
        foreach (Animator i in animators)
        {
            i.Play("WalkRun");
        }
    }
    public void ANim7Press()
    {
        foreach (Animator i in animators)
        {
            i.Play("Celeb");
        }
    }
}
