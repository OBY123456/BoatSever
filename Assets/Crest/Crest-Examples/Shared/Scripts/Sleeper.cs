﻿// Crest Ocean System for LWRP

// Copyright 2019 Huw Bowles

using UnityEngine;

/// <summary>
/// Adds a sleep/freeze into the update, can be used to inflate the frame time.
/// </summary>
public class Sleeper : MonoBehaviour
{
    public int _sleepMs = 0;
    public bool _jitter = false;
    public int _sleepStride = 1;

    void Update()
    {
        if (Time.frameCount % _sleepStride == 0)
        {
            var sleep = _jitter ? (int)(Random.value * _sleepMs) : _sleepMs;
            System.Threading.Thread.Sleep(sleep);
        }
    }
}
