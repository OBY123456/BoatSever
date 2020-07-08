using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Rpm,Pitch,Azimuth数据组合控制脚本
/// </summary>
public class RPAControl : MonoBehaviour
{
    public Text RpmText1, RpmText2;
    public RectTransform RpmImage;

    public Text PitchText1, PitchText2;
    public RectTransform PitchImage;

    public Text AzimuthText1, AzimuthText2;
    public RectTransform AzimuthImage;

    private float MaxHight = 175.0f;
    private float Hight = 159.0f;

    // Start is called before the first frame update
    void Start()
    {
        RpmText1 = transform.Find("Rpm/RpmText1").GetComponent<Text>();
        RpmText2 = transform.Find("Rpm/RpmText2").GetComponent<Text>();

        RpmImage = transform.Find("Rpm/RpmImage").GetComponent<RectTransform>();

        PitchText1 = transform.Find("Pitch/RpmText1").GetComponent<Text>();
        PitchText2 = transform.Find("Pitch/RpmText2").GetComponent<Text>();

        PitchImage = transform.Find("Pitch/RpmImage").GetComponent<RectTransform>();

        AzimuthText1 = transform.Find("Azimuth/AzimuthText1").GetComponent<Text>();
        AzimuthText2 = transform.Find("Azimuth/AzimuthText2").GetComponent<Text>();

        AzimuthImage = transform.Find("Azimuth/AzimuthImage").GetComponent<RectTransform>();
    }

    /// <summary>
    /// 设置Rpm,Pitch,Azimuth的值
    /// </summary>
    /// <param name="value1">Rpm</param>
    /// <param name="value2">Pitch</param>
    /// <param name="value3">Azimuth</param>
    public void RPASet(float value1,float value2,float value3)
    {
        int a, b;
        RpmImage.sizeDelta = new Vector2(10, SetData(value1));
        SplitDecimal(value1, out a, out b);
        RpmText1.text = a.ToString();
        RpmText2.text = b.ToString();

        PitchImage.sizeDelta = new Vector2(10,SetData(value2));
        SplitDecimal(value2, out a, out b);
        PitchText1.text = a.ToString();
        PitchText2.text = b.ToString();

        if(value3 > 180)
        {
            value3 = 180;
        }

        if(value3 < -180)
        {
            value3 = -180;
        }

        AzimuthImage.localEulerAngles = Vector3.back * value3;

        SplitDecimal(value3, out a, out b);
        AzimuthText1.text = Mathf.Abs(a).ToString();
        AzimuthText2.text = Mathf.Abs(b).ToString();
    }

    /// <summary>
    /// 分离一个小数的整数部分和小数部分
    /// </summary>
    /// <param name="value"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void SplitDecimal(float value,out int a,out int b)
    {
        a = (int)value;
        b = (int)((value - a) * 100);
    }

    /// <summary>
    /// 对数据进行处理
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private float SetData(float value)
    {
        if(value > 110)
        {
            value = 110;
        }

        if(value < 0)
        {
            value = 0;
        }

        if(value <= 100)
        {
            return value / 100 * Hight;
        }
        else
        {
            return Hight + ((value - 100) / 10f) * (MaxHight - Hight);
        }
    }
}
