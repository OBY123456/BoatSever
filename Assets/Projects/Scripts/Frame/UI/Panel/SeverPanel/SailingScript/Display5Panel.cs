using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class Display5Panel : BasePanel
{
    public static Display5Panel Instance;

    public Text WeatherText;
    public Text DayText;
    public Text OceanText;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void InitFind()
    {
        base.InitFind();
        WeatherText = FindTool.FindChildComponent<Text>(transform, "WeatherText");
        DayText = FindTool.FindChildComponent<Text>(transform, "DayText");
        OceanText = FindTool.FindChildComponent<Text>(transform, "OceanText");
    }

    protected override void Start()
    {
        base.Start();
        Reset();
    }

    /// <summary>
    /// 天气
    /// </summary>
    /// <param name="msg"></param>
    public void SetWeatherText(string msg)
    {
        WeatherText.text = msg;
    }

    /// <summary>
    /// 日夜
    /// </summary>
    /// <param name="msg"></param>
    public void SetDayText(string msg)
    {
        DayText.text = msg;
    }

    /// <summary>
    /// 海况等级
    /// </summary>
    /// <param name="msg"></param>
    public void SetOceanText(string msg)
    {
        OceanText.text = msg;
    }

    public void Reset()
    {
        SetWeatherText("晴天");
        SetDayText("白天");
        SetOceanText("0");
    }
}
