using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;
using Newtonsoft.Json;

public class FirstPersonDataPanel : BasePanel
{
    [Header("table1")]
    public Text TimeText, TemperatureText;

    [Header("table2")]
    public Text DepthText;
    public RectTransform DepthImage;

    [Header("table3")]
    public Text RateText;
    public RectTransform RateImage;

    [Header("table4")]
    public Text LogText, LogText2, LogText3;

    [Header("table5")]
    public RPAControl RPAControl_table5;

    [Header("table6")]
    public RectTransform GyroImage;
    public RectTransform BgImage;

    [Header("table7")]
    public Text RollText;
    public RectTransform RollImage;

    [Header("table8")]
    public RPAControl RPAControl_table8;

    [Header("table9")]
    public Text RelDirectionText, RelSpeedText;
    public RectTransform WindImage;

    private EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();

    protected override void Start()
    {
        base.Start();
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, DataPanelName.FirstPersonDataPanel.ToString(), callback);
    }

    private void callback(EventParamete parameteData)
    {
        QueueData queueData = parameteData.GetParameter<QueueData>()[0];
        ParmaterCodes codes = queueData.parmaterCodes;
        string msg = queueData.msg;
        switch (codes)
        {
            case ParmaterCodes.TimeData:
                TimeData timeData = new TimeData();
                timeData = JsonConvert.DeserializeObject<TimeData>(msg);
                string temp = timeData.Hours + ":" + timeData.Minutes + ":" + timeData.Seconds;
                SetTable1_Time(temp);
                break;
            case ParmaterCodes.TemperatureData:
                TemperatureData temperatureData = new TemperatureData();
                temperatureData = JsonConvert.DeserializeObject<TemperatureData>(msg);
                SetTable1_Temperature(temperatureData.value.ToString("#0.0"));
                break;
            case ParmaterCodes.DepthData:
                DepthData depthData = new DepthData();
                depthData = JsonConvert.DeserializeObject<DepthData>(msg);
                SetTable2(depthData.value);
                break;
            case ParmaterCodes.RoteOfTurnData:
                RoteOfTurnData roteOfTurnData = new RoteOfTurnData();
                roteOfTurnData = JsonConvert.DeserializeObject<RoteOfTurnData>(msg);
                SetTable3(roteOfTurnData.value);
                break;
            case ParmaterCodes.LogData:
                LogData logData = new LogData();
                logData = JsonConvert.DeserializeObject<LogData>(msg);
                SetTable4(logData.value1, logData.value2, logData.value3);
                break;
            case ParmaterCodes.RAPData:
                RAPData data = new RAPData();
                data = JsonConvert.DeserializeObject<RAPData>(msg);
                Direction direction = (Direction)Enum.Parse(typeof(Direction), data.direction);
                switch (direction)
                {
                    case Direction.Left:
                        SetTable5(data.RPMValue, data.PitchValue, data.AngleValue);
                        break;
                    case Direction.Right:
                        SetTable8(data.RPMValue, data.PitchValue, data.AngleValue);
                        break;
                    default:
                        break;
                }
                break;
            case ParmaterCodes.GyroData:
                GyroData gyroData = new GyroData();
                gyroData = JsonConvert.DeserializeObject<GyroData>(msg);
                SetTable6(gyroData.value);
                break;
            case ParmaterCodes.RollData:
                RollData rollData = new RollData();
                rollData = JsonConvert.DeserializeObject<RollData>(msg);
                SetTable7(rollData.value);
                break;
            case ParmaterCodes.RelDirectionData:
                RelDirectionData Reldata = new RelDirectionData();
                Reldata = JsonConvert.DeserializeObject<RelDirectionData>(msg);
                SetTable9_direction(Reldata.value);
                break;
            case ParmaterCodes.RelSpeedData:
                RelSpeedData relSpeedData = new RelSpeedData();
                relSpeedData = JsonConvert.DeserializeObject<RelSpeedData>(msg);
                SetTable9_speed(relSpeedData.value);
                break;
            default:
                break;
        }
    }

    public override void InitFind()
    {
        base.InitFind();
        TimeText = FindTool.FindChildComponent<Text>(transform, "table1/TimeText");
        TemperatureText = FindTool.FindChildComponent<Text>(transform, "table1/TemperatureText");

        DepthText = FindTool.FindChildComponent<Text>(transform, "table2/DepthText");
        DepthImage = FindTool.FindChildComponent<RectTransform>(transform, "table2/DepthImage");

        RateText = FindTool.FindChildComponent<Text>(transform, "table3/RateText");
        RateImage = FindTool.FindChildComponent<RectTransform>(transform, "table3/RateImage");

        LogText = FindTool.FindChildComponent<Text>(transform, "table4/LogText");
        LogText2 = FindTool.FindChildComponent<Text>(transform, "table4/LogText (1)");
        LogText3 = FindTool.FindChildComponent<Text>(transform, "table4/LogText (2)");

        RPAControl_table5 = FindTool.FindChildComponent<RPAControl>(transform, "table5");

        GyroImage = FindTool.FindChildComponent<RectTransform>(transform, "table6/GyroImage");
        BgImage = FindTool.FindChildComponent<RectTransform>(transform, "table6/bgImage");

        RollText = FindTool.FindChildComponent<Text>(transform, "table7/RollText");
        RollImage = FindTool.FindChildComponent<RectTransform>(transform, "table7/RollImage");

        RPAControl_table8 = FindTool.FindChildComponent<RPAControl>(transform, "table8");

        RelDirectionText = FindTool.FindChildComponent<Text>(transform, "table9/RelDirectionText");
        RelSpeedText = FindTool.FindChildComponent<Text>(transform, "table9/RelSpeedText");
        WindImage = FindTool.FindChildComponent<RectTransform>(transform, "table9/WindImage");
    }

    /// <summary>
    /// 时间
    /// </summary>
    /// <param name="msg">时间</param>
    public void SetTable1_Time(string msg)
    {
        TimeText.text = msg;   
    }

    /// <summary>
    /// 温度
    /// </summary>
    /// <param name="msg">温度</param>
    public void SetTable1_Temperature(string msg)
    {
        TemperatureText.text = msg;
    }

    /// <summary>
    /// 深度
    /// </summary>
    /// <param name="value">depth</param>
    public void SetTable2(float value)
    {
        DepthText.text = value.ToString("#0.0");

        if (value < 0)
        {
            value = 0;
        }

        if(value > 500)
        {
            value = 500;
        }

        DepthImage.GetComponent<Image>().fillAmount = 0.5f + value / 500f * 0.5f;
    }

    /// <summary>
    /// Rate of turn
    /// </summary>
    /// <param name="value">Rate of turn</param>
    public void SetTable3(float value)
    {
        if (value > 30)
        {
            value = 30;
        }

        if (value < -30)
        {
            value = -30;
        }

        RateImage.localEulerAngles = Vector3.back * (value / 30f * 134f);
        RateText.text = Mathf.Abs(value).ToString("#0.0") + "°";
    }

    /// <summary>
    /// LOG
    /// </summary>
    /// <param name="value1">艏部横向速度</param>
    /// <param name="value2">尾部横向速度</param>
    /// <param name="value3">纵向速度</param>
    public void SetTable4(float value1,float value2,float value3)
    {
        LogText.text = Mathf.Abs(value1).ToString("#0.0");
        LogText2.text = Mathf.Abs(value2).ToString("#0.0");
        LogText3.text = Mathf.Abs(value3).ToString("#0.0");
    }

    /// <summary>
    /// Rpm,Pitch,Azimuth 主左
    /// </summary>
    /// <param name="value1">Rpm</param>
    /// <param name="value2">Pitch</param>
    /// <param name="value3">Azimuth</param>
    public void SetTable5(float value1, float value2, float value3)
    {
        RPAControl_table5.RPASet(value1, value2, value3);
    }

    /// <summary>
    /// Gyro
    /// </summary>
    /// <param name="value">Gyro</param>
    public void SetTable6(float value)
    {
        if(value < 0)
        {
            value = 0;
        }

        if(value > 360)
        {
            value = 360;
        }

        GyroImage.localEulerAngles = Vector3.back * value;
        BgImage.localEulerAngles = Vector3.back * (value + UnityEngine.Random.Range(-0.5f,0.5f));
    }

    /// <summary>
    /// Roll
    /// </summary>
    /// <param name="value">Roll</param>
    public void SetTable7(float value)
    {
        if (value < -20)
        {
            value = -20;
        }

        if (value > 20)
        {
            value = 20;
        }

        RollImage.localEulerAngles = Vector3.forward * (value / 20f * 28.5f);
        RollText.text = Mathf.Abs(value).ToString("#0.0") + "°";
    }

    /// <summary>
    /// Rpm,Pitch,Azimuth 主右
    /// </summary>
    /// <param name="value1">Rpm</param>
    /// <param name="value2">Pitch</param>
    /// <param name="value3">Azimuth</param>
    public void SetTable8(float value1,float value2,float value3)
    {
        RPAControl_table8.RPASet(value1, value2, value3);
    }

    /// <summary>
    /// Wind Indicator
    /// </summary>
    /// <param name="value">Rel.direction</param>
    public void SetTable9_direction(float value)
    {
        if (value < -180)
        {
            value = -180;
        }

        if (value > 180)
        {
            value = 180;
        }

        WindImage.localEulerAngles = Vector3.back * value;

        RelDirectionText.text = Mathf.Abs(value).ToString("#0.0");
        
    }

    /// <summary>
    /// Wind Indicator
    /// </summary>
    /// <param name="value">Rel.speed</param>
    public void SetTable9_speed(float value)
    {
        RelSpeedText.text = value.ToString("#0.0");
        if (value < 0)
        {
            value = 0;
        }

        if (value > 100)
        {
            value = 100;
        }

        WeatherManager.Instance.SetWindIntensity(value / 100f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, DataPanelName.FirstPersonDataPanel.ToString(), callback);
    }

    private void SentDataToPanel(string msg, ParmaterCodes parmaterCodes, DataPanelName dataPanelName)
    {
        QueueData data = new QueueData();
        data.msg = msg;
        data.parmaterCodes = parmaterCodes;
        eventParamete.AddParameter(data);
        EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, dataPanelName.ToString(), eventParamete);
    }
}
