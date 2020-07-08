using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;
using Newtonsoft.Json;

public class PropellerStatePanel : BasePanel
{
    [Header("Table1")]
    public Text Propeller_Msg1;
    public Text Propeller_Msg2;

    [Header("Table2")]
    public Text Generator_Msg1;
    public Text Generator_Msg2;
    public Text Generator_Msg3;
    public Text Generator_Msg4;

    [Header("Table3")]
    public Text RpmText;
    public RectTransform RpmImage;
    public Text AzimuthText;
    public Text PitchText;
    public RectTransform PitchImage;

    protected override void Start()
    {
        base.Start();
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, DataPanelName.PropellerStatePanel.ToString(), callback);
    }

    private void callback(EventParamete parameteData)
    {
        QueueData queueData = parameteData.GetParameter<QueueData>()[0];
        ParmaterCodes codes = queueData.parmaterCodes;
        string msg = queueData.msg;
        switch (codes)
        {
            case ParmaterCodes.PropellerState:
                PropellerState propellerState = new PropellerState();
                propellerState = JsonConvert.DeserializeObject<PropellerState>(msg);
                SetTable1(propellerState.msg1, propellerState.msg2);
                break;
            case ParmaterCodes.GeneratorData:
                GeneratorData generatorData = new GeneratorData();
                generatorData = JsonConvert.DeserializeObject<GeneratorData>(msg);
                SetTable2(generatorData.msg, generatorData.state, generatorData.Generatorvalue, generatorData.PeiDianBanValue);
                break;
            case ParmaterCodes.RAPData:
                RAPData data = new RAPData();
                data = JsonConvert.DeserializeObject<RAPData>(msg);
                SetTable3(data.RPMValue, data.PitchValue, data.AngleValue);
                break;
            default:
                break;
        }
    }

    public override void InitFind()
    {
        base.InitFind();
        Propeller_Msg1 = FindTool.FindChildComponent<Text>(transform, "table1/MsgText1");
        Propeller_Msg2 = FindTool.FindChildComponent<Text>(transform, "table1/MsgText2");

        Generator_Msg1 = FindTool.FindChildComponent<Text>(transform, "table2/MsgText1");
        Generator_Msg2 = FindTool.FindChildComponent<Text>(transform, "table2/MsgText2");
        Generator_Msg3 = FindTool.FindChildComponent<Text>(transform, "table2/MsgText3");
        Generator_Msg4 = FindTool.FindChildComponent<Text>(transform, "table2/MsgText4");

        RpmText = FindTool.FindChildComponent<Text>(transform, "table3/RpmText");
        RpmImage = FindTool.FindChildComponent<RectTransform>(transform, "table3/RPMImage");
        AzimuthText = FindTool.FindChildComponent<Text>(transform, "table3/AzimuthText");
        PitchText = FindTool.FindChildComponent<Text>(transform, "table3/PitchText");
        PitchImage = FindTool.FindChildComponent<RectTransform>(transform, "table3/PitchImage");
    }

    /// <summary>
    /// 推进器
    /// </summary>
    /// <param name="msg1">推进器准备好</param>
    /// <param name="msg2">推进器运行</param>
    public void SetTable1(string msg1,string msg2)
    {
        Propeller_Msg1.text = msg1;
        Propeller_Msg2.text = msg2;
    }

    /// <summary>
    /// 发电机
    /// </summary>
    /// <param name="msg1">发电机组在线</param>
    /// <param name="msg2">开关状态</param>
    /// <param name="value1">发电机组当前功率</param>
    /// <param name="value2">配电板当前功率</param>
    public void SetTable2(string msg1,string msg2,float value1,float value2)
    {
        Generator_Msg1.text = msg1;
        Generator_Msg2.text = msg2;

        Generator_Msg3.text = value1.ToString("#0.0");
        Generator_Msg4.text = value2.ToString("#0.0");
    }

    /// <summary>
    /// 设置Rpm,Pitch,Azimuth的值
    /// </summary>
    /// <param name="value1">Rpm</param>
    /// <param name="value2">Pitch</param>
    /// <param name="value3">Azimuth</param>
    public void SetTable3(float value1,float value2,float value3)
    {
        if(value1 < -100)
        {
            value1 = -100;
        }

        if(value1 > 100)
        {
            value1 = 100;
        }

        RpmText.text = value1.ToString("#0.0");
        RpmImage.localEulerAngles = Vector3.back * (value1 / 100f * 29f);

        AzimuthText.text = value2.ToString("#0.0");

        if (value3 < -100)
        {
            value3 = -100;
        }

        if (value3 > 100)
        {
            value3 = 100;
        }

        PitchText.text = value3.ToString("#0.0");
        PitchImage.localEulerAngles = Vector3.back * (value3 / 100f * 29f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, DataPanelName.PropellerStatePanel.ToString(), callback);
    }
}
