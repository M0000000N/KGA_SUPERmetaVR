﻿using UnityEngine;
using System.Collections.Generic;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

/// <summary>
/// Component which will flicker a linked light while active by changing its
/// intensity between the min and max values given. The flickering can be
/// sharp or smoothed depending on the value of the smoothing parameter.
///
/// Just activate / deactivate this component as usual to pause / resume flicker
/// </summary>
public class Flickering : MonoBehaviour
{
    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    public new Light light;
    [Tooltip("Minimum random light intensity")]
    public float minIntensity = 0f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 1f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 5;
    [Range(0, 2)]
    public float lightPosRandomness = 0.5f;
    private Vector3 startPosition;
    Vector3 randomPos;
    float TimeSinceRandomRefresh = 9999.0f;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;


    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset()
    {
        smoothQueue.Clear();
        lastSum = 0;
    }

    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        startPosition = transform.position;
        // External or internal light?
        if (light == null)
        {
            light = GetComponent<Light>();
            
        }
    }

    void Update()
    {
        if (light == null)
            return;
        setRandomPos(0.15f);
        RandomLerpPos(2.5f);
        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;
        // Calculate new smoothed average
        //light.intensity = lastSum / (float)smoothQueue.Count;
    }

    void RandomLerpPos(float speed)
    {
        Vector3 newPos = Vector3.Lerp(transform.position, randomPos, Time.deltaTime * speed);
        transform.position = newPos;
    }
    void setRandomPos(float interval)
    {
        if (TimeSinceRandomRefresh > interval)
        {
            randomPos = Random.insideUnitSphere/(1/lightPosRandomness);
            randomPos += startPosition;
            Debug.Log(randomPos);
            TimeSinceRandomRefresh = 0.0f;
        }
        else
        {
            TimeSinceRandomRefresh += Time.deltaTime;
        }
    }

}
