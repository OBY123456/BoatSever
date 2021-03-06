﻿// Crest Ocean System for LWRP

// Copyright 2019 Huw Bowles

#if UNITY_2019

using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;

namespace Crest
{
    public class SampleShadows : ScriptableRendererFeature
    {
        public static bool Created { get; private set; }

        SampleShadowsPass renderObjectsPass;

        public override void Create()
        {
            renderObjectsPass = new SampleShadowsPass(RenderPassEvent.AfterRenderingSkybox);

            Created = true;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(renderObjectsPass);
        }
    }

    public class SampleShadowsPass : ScriptableRenderPass
    {
        public SampleShadowsPass(RenderPassEvent renderPassEvent)
        {
            this.renderPassEvent = renderPassEvent;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (OceanRenderer.Instance == null || OceanRenderer.Instance._lodDataShadow == null) return;

            if (context == null)
                throw new System.ArgumentNullException("context");

            if (renderingData.lightData.mainLightIndex == -1)
                return;

            var cmd = OceanRenderer.Instance._lodDataShadow.BufCopyShadowMap;
            if (cmd == null) return;

            context.ExecuteCommandBuffer(cmd);
        }
    }
}

#endif
