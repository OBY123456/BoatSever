﻿// Crest Ocean System for LWRP

// Copyright 2019 Huw Bowles

namespace Crest
{
    /// <summary>
    /// This time provider fixes the ocean time at a custom value which is usable for testing/debugging.
    /// </summary>
    public class TimeProviderCustom : TimeProviderBase
    {
        public float _time = 0f;
        public float _deltaTime = 0f;

        public override float CurrentTime => _time;
        public override float DeltaTime => _deltaTime;
    }
}
