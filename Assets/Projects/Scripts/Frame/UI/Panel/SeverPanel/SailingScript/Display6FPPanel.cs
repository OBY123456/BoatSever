using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using DG.Tweening;

public class Display6FPPanel : BasePanel
{

    [SerializeField, Range(-180, 180)]
    private float RelWind = 0;

    [SerializeField, Range(0, 360)]
    private float Gyro = 0;

    [SerializeField, Range(-100, 100)]
    private float RPM = 0;

    [SerializeField, Range(0, 103)]
    private float Depth = 0;

    //[SerializeField, Range(-60, 60)]
    //private int RudderAngle = 0;

    [SerializeField, Range(-100, 100)]
    private float Pitch = 0;

    [SerializeField, Range(-30, 30)]
    private float RateOfTurn = 0;

    [SerializeField, Range(-20, 20)]
    private float Roll = 0;

    public RectTransform RelWindRect;
    public RectTransform GyroRect;
    public RectTransform RPMRect;
    public RectTransform DepthRect;
    //public RectTransform RudderAngleImage, RudderAngleImage1, RudderAngleImage2, RudderAngleImage3, RudderAngleImage4;
    //public RectTransform DBKRect;
    public RectTransform PitchRect;
    public RectTransform RateOfTurnRect;
    public RectTransform RollRect;

    public Text TimeText, TemperotureText;
    public Text WindSpeedText, RelWindSpeedText;
    public Text GyroText1, GyroText2, MagnText;
    public Text RPMText;
    public Text DepthText;
    public Text PitchText;
    public Text RateOfTurnText;
    public Text RollText;
    public Text LOG1, LOG2, LOG3;

    protected override void Start()
    {
        base.Start();
        TimeText.gameObject.SetActive(false);
        TemperotureText.gameObject.SetActive(false);
    }

    public override void InitFind()
    {
        base.InitFind();
        TimeText = FindTool.FindChildComponent<Text>(transform, "Panel1/table1/TimeText");
        TemperotureText = FindTool.FindChildComponent<Text>(transform, "Panel1/table1/TemperotureText");

        RateOfTurnRect = FindTool.FindChildComponent<RectTransform>(transform, "Panel1/table2/PointImage2");
        RateOfTurnText = FindTool.FindChildComponent<Text>(transform, "Panel1/table2/RateOfTurnText");

        GyroRect = FindTool.FindChildComponent<RectTransform>(transform, "Panel1/table3/PointImage3");
        GyroText1 = FindTool.FindChildComponent<Text>(transform, "Panel1/table3/GyroText1");
        GyroText2 = FindTool.FindChildComponent<Text>(transform, "Panel1/table3/GyroText2");
        MagnText = FindTool.FindChildComponent<Text>(transform, "Panel1/table3/MagnText");

        RPMRect = FindTool.FindChildComponent<RectTransform>(transform, "Panel1/table4/PointImage4");
        RPMText = FindTool.FindChildComponent<Text>(transform, "Panel1/table4/RPMText");

        PitchRect = FindTool.FindChildComponent<RectTransform>(transform, "Panel1/table5/PointImage5");
        PitchText = FindTool.FindChildComponent<Text>(transform, "Panel1/table5/PitchText");

        DepthRect = FindTool.FindChildComponent<RectTransform>(transform, "Panel1/table6/PointImage6");
        DepthText = FindTool.FindChildComponent<Text>(transform, "Panel1/table6/DepthText");

        WindSpeedText = FindTool.FindChildComponent<Text>(transform, "Panel2/table1/WindSpeedText");

        RelWindRect = FindTool.FindChildComponent<RectTransform>(transform, "Panel2/table2/PointImage2");
        RelWindSpeedText = FindTool.FindChildComponent<Text>(transform, "Panel2/table2/RelWindSpeedText");

        RollRect = FindTool.FindChildComponent<RectTransform>(transform, "Panel2/table3/PointImage3");
        RollText = FindTool.FindChildComponent<Text>(transform, "Panel2/table3/RollText");

        LOG1 = FindTool.FindChildComponent<Text>(transform, "Panel2/table4/LOG1");
        LOG2 = FindTool.FindChildComponent<Text>(transform, "Panel2/table4/LOG2");
        LOG3 = FindTool.FindChildComponent<Text>(transform, "Panel2/table4/LOG3");
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void SetRateOfTurnValue(float value)
    {
        if(value > 30)
        {
            value = 30;
        }

        if(value < -30)
        {
            value = -30;
        }

        RateOfTurnRect.localEulerAngles = Vector3.back * (value / 30 * 134);
        RateOfTurnText.text = Mathf.Abs(value).ToString("#0.0") + "°";
    }

    public void SetGyroValue(float value)
    {
        GyroRect.localEulerAngles = Vector3.back * value;
        GyroText1.text = Mathf.Abs(value).ToString("#0");
    }

    public void SetRPMValue(float value)
    {
        if(value > 100)
        {
            value = 100;
        }

        if(value < -100)
        {
            value = -100;
        }

        float factor = value / 100 * 31;
        //float factor2 = value / 100 * 25.2f;
        RPMRect.localEulerAngles = Vector3.back * factor;
       // RPMRect2.localEulerAngles = Vector3.back * factor2;
        RPMText.text = Mathf.Abs(value).ToString();
    }

    public void SetPitchValue(float value)
    {
        PitchRect.localEulerAngles = Vector3.back * (value / 100 * 31);
        //PitchRect2.localEulerAngles = Vector3.back * (value / 100 * 24.9f);
        PitchText.text = Mathf.Abs(value).ToString("#0");
    }

    public void SetDepthValue(float value)
    {
        if(value > 5000)
        {
            value = 5000 + Random.Range(-2f, 0f);
        }

        DepthRect.sizeDelta = new Vector2(276, value/5000*103 + Random.Range(-20,0));
        DepthText.text = Mathf.Abs(value).ToString("#0.0");

    }

    public void SetRelWindValue(float value)
    {
        RelWindRect.localEulerAngles = Vector3.back * value;
        RelWindSpeedText.text = Mathf.Abs(value).ToString()+ "°";
    }

    public void SetRollValue(float value)
    {
        if(value > 20)
        {
            value = 20;
        }

        if(value < -20)
        {
            value = -20;
        }

        RollRect.localEulerAngles = Vector3.forward * (value / 20 * 36);
        RollText.text = value.ToString("#0.0") + "°";
    }

    public void SetLogValue(float log1,float log2,float log3)
    {
        LOG1.text = log1.ToString("#0.0");
        LOG2.text = log2.ToString("#0.0");
        LOG3.text = log3.ToString("#0.0");
    }
}
