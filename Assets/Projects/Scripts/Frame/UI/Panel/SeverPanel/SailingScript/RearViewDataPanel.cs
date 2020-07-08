using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;
using Newtonsoft.Json;

public class RearViewDataPanel : BasePanel
{
    [Header("table1")]
    public RectTransform WindImage;

    [Header("table2")]
    public Text TrueText1;
    public Text TrueText2;
    public Text RelativeText1;
    public Text RelativeText2;

    [Header("table3")]
    public RectTransform RollImage;
    public Text RollText;

    [Header("table4")]
    public RectTransform DepthImage;
    public Text DepthText;

    [Header("table5")]
    public Text RotText;
    public Text MaonText;
    public Text GyroText;
    public Text CobText;
    public Text BogText;
    public Text PitchText;
    public RectTransform PitchImage;

    protected override void Start()
    {
        base.Start();
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, DataPanelName.RearViewDataPanel.ToString(), callback);
    }

    private void callback(EventParamete parameteData)
    {
        QueueData queueData = parameteData.GetParameter<QueueData>()[0];
        ParmaterCodes codes = queueData.parmaterCodes;
        string msg = queueData.msg;
        switch (codes)
        {
            case ParmaterCodes.WindData:
                WindData windData = new WindData();
                windData = JsonConvert.DeserializeObject<WindData>(msg);
                SetTable1(windData.value);
                break;
            case ParmaterCodes.TrueData:
                TrueData trueData = new TrueData();
                trueData = JsonConvert.DeserializeObject<TrueData>(msg);
                SetTable2_True(trueData.value1, trueData.value2);
                break;
            case ParmaterCodes.RelativeData:
                RelativeData relativeData = new RelativeData();
                relativeData = JsonConvert.DeserializeObject<RelativeData>(msg);
                SetTable2_Relative(relativeData.value1, relativeData.value2);
                break;
            case ParmaterCodes.RollData:
                RollData rollData = new RollData();
                rollData = JsonConvert.DeserializeObject<RollData>(msg);
                SetTable3(rollData.value);
                break;
            case ParmaterCodes.DepthData:
                DepthData depthData = new DepthData();
                depthData = JsonConvert.DeserializeObject<DepthData>(msg);
                SetTable4(depthData.value);
                break;
            case ParmaterCodes.RotData:
                RotData rotData = new RotData();
                rotData = JsonConvert.DeserializeObject<RotData>(msg);
                SetTable5(rotData.Rotvalue, rotData.Maonvalue, rotData.Gyrovalue, rotData.Cobvalue, rotData.Bogvalue, rotData.Pitchvalue);
                break;
            default:
                break;
        }
    }

    public override void InitFind()
    {
        base.InitFind();
        WindImage = FindTool.FindChildComponent<RectTransform>(transform, "Window1/table1/WindImage");

        TrueText1 = FindTool.FindChildComponent<Text>(transform, "Window1/table2/TrueText1");
        TrueText2 = FindTool.FindChildComponent<Text>(transform, "Window1/table2/TrueText2");
        RelativeText1 = FindTool.FindChildComponent<Text>(transform, "Window1/table2/RelativeText1");
        RelativeText2 = FindTool.FindChildComponent<Text>(transform, "Window1/table2/RelativeText2");

        RollImage = FindTool.FindChildComponent<RectTransform>(transform, "Window1/table3/RollImage");
        RollText = FindTool.FindChildComponent<Text>(transform, "Window1/table3/RollText");

        DepthImage = FindTool.FindChildComponent<RectTransform>(transform, "Window1/table4/DepthImage");
        DepthText = FindTool.FindChildComponent<Text>(transform, "Window1/table4/DepthText");

        RotText = FindTool.FindChildComponent<Text>(transform, "Window1/table5/RotText");
        MaonText = FindTool.FindChildComponent<Text>(transform, "Window1/table5/MaonText");
        GyroText = FindTool.FindChildComponent<Text>(transform, "Window1/table5/GyroText");
        CobText = FindTool.FindChildComponent<Text>(transform, "Window1/table5/CobText");
        BogText = FindTool.FindChildComponent<Text>(transform, "Window1/table5/BogText");
        PitchText = FindTool.FindChildComponent<Text>(transform, "Window1/table5/PitchText");
        PitchImage = FindTool.FindChildComponent<RectTransform>(transform, "Window1/table5/PitchImage");
    }

    /// <summary>
    /// Wind
    /// </summary>
    /// <param name="value">Wind</param>
    public void SetTable1(float value)
    {
        if(value < -180)
        {
            value = -180;
        }

        if(value > 180)
        {
            value = 180;
        }

        WindImage.localEulerAngles = Vector3.back * value;
        WeatherManager.Instance.SetWindRotate(value);
    }

    /// <summary>
    /// True
    /// </summary>
    /// <param name="value1">Ture从上往下第一个数据</param>
    /// <param name="value2">Ture从上往下第二个数据</param>
    public void SetTable2_True(float value1,float value2)
    {
        TrueText1.text = ((int)value1).ToString() + "°";
        TrueText2.text = ((int)value2).ToString() ;
    }

    /// <summary>
    /// Relative
    /// </summary>
    /// <param name="value1">Relative从上往下第一个数据</param>
    /// <param name="value2">Relative从上往下第二个数据</param>
    public void SetTable2_Relative(float value1, float value2)
    {
        RelativeText1.text = ((int)value1).ToString() + "°";
        RelativeText2.text = ((int)value2).ToString();
    }

    /// <summary>
    /// Roll
    /// </summary>
    /// <param name="value">roll</param>
    public void SetTable3(float value)
    {
        if(value < -20)
        {
            value = -20;
        }

        if(value > 20)
        {
            value = 20;
        }

        RollText.text = value.ToString("#0.0") + "°";
        RollImage.localEulerAngles = Vector3.back * (value / 20 * 36.2f);
    }

    /// <summary>
    /// Depth
    /// </summary>
    /// <param name="value">Depth</param>
    public void SetTable4(float value)
    {
        DepthText.text = value.ToString("#0.0");
        if (value < 0)
        {
            value = 0;
        }

        if(value > 200)
        {
            value = 200;
        } 
        DepthImage.GetComponent<Image>().fillAmount = value / 200f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="RotValue">RotValue</param>
    /// <param name="MaonValue">MaonValue</param>
    /// <param name="GyroValue">GyroValue</param>
    /// <param name="CobValue">CobValue</param>
    /// <param name="BogValue">BogValue</param>
    /// <param name="PitchValue">PitchValue</param>
    public void SetTable5(float RotValue,float MaonValue,float GyroValue,float CobValue,float BogValue,float PitchValue)
    {
        RotText.text = ((int)RotValue).ToString();
        MaonText.text = MaonValue.ToString("#0.0") + "°";
        GyroText.text = GyroValue.ToString("#0.0") + "°";
        CobText.text = CobValue.ToString("#0.0") + "°";
        BogText.text = BogValue.ToString("#0.0");

        if(PitchValue < -10)
        {
            PitchValue = -10;
        }

        if(PitchValue > 10)
        {
            PitchValue = 10;
        }

        PitchText.text = PitchValue.ToString("#0.0");
        PitchImage.localEulerAngles = Vector3.forward * (PitchValue / 10 * 10.5f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, DataPanelName.RearViewDataPanel.ToString(), callback);
    }
}
