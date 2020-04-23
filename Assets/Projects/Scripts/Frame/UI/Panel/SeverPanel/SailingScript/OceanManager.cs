using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;

public class OceanManager : MonoBehaviour
{
    public static OceanManager Instance;

    public ShapeGerstnerBatched gerstnerBatched;
    public OceanRenderer oceanRenderer;

    [Range(0,1.33f)]
    public float WaveSize = 0.2f;

    [Range(0, 1)]
    public float OceanLight = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //SetWaveSize(WaveSize);
        //SetOceanLight(OceanLight);
    }

    /// <summary>
    /// 设置海浪大小
    /// </summary>
    /// <param name="value"></param>
    public void SetWaveSize(float value)
    {
        gerstnerBatched._spectrum._multiplier = value;
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
}
