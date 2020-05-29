﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;

public class OceanManager : MonoBehaviour
{
    public static OceanManager Instance;

    public ShapeGerstnerBatched gerstnerBatched;
    public OceanRenderer oceanRenderer;

    [Range(0.2f,1.7f)]
    public float WaveSize = 0.2f;

    [Range(0, 1)]
    public float OceanLight = 0.5f;

    public Material[] OceanMaterial;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetOcean();
    }

    // Update is called once per frame
    void Update()
    {
        SetWaveSize(WaveSize);
        //SetOceanLight(OceanLight);
    }

    /// <summary>
    /// 设置海浪大小
    /// </summary>
    /// <param name="value"></param>
    public void SetWaveSize(float value)
    {
        gerstnerBatched._spectrum._multiplier = value;
        //SailingSceneManage.Instance.boatProbes._forceMultiplier = 30 - (value - 0.2f) / 1.5f * 25;
        //SailingSceneManage.Instance.boatProbes._minSpatialLength = 100 - (value - 0.2f) / 1.5f * 60;
        gerstnerBatched._spectrum._powerLog[13] = 1.5f + (value - 0.2f) / 1.5f * 2.0f;
        oceanRenderer.GravityOffect = 1.0f + (value - 0.2f) / 1.5f * 0.1f;
    }

    /// <summary>
    /// 设置海面亮度和晚上船在海面的倒影亮度
    /// </summary>
    /// <param name="value"></param>
    public void SetOceanLight(float value)
    {
        oceanRenderer.OceanMaterial.SetFloat("_Specular", value);
        //oceanRenderer.OceanMaterial.SetFloat("_CausticsTextureAverage", 0.37f - (0.37f - 0.07f) * value);
    }

    public void ResetOcean()
    {
        SetWaveSize(WaveSize);
        SetOceanLight(OceanLight);
    }

    public void SetDayOceanMaterial()
    {
        oceanRenderer._material = OceanMaterial[0];
    }

    public void SetNightOceanMaterial()
    {
        oceanRenderer._material = OceanMaterial[1];
    }

    public void SetNightRainOceanMaterial()
    {
        oceanRenderer._material = OceanMaterial[2];
    }
}
