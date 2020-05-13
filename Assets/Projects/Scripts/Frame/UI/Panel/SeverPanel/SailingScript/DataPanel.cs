using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using DG.Tweening;

public class DataPanel : BasePanel
{

    public static DataPanel Instance;

    [SerializeField, Range(-180, 180)]
    private float RelWind = 0;

    [SerializeField, Range(0, 360)]
    private float Gyro = 0;

    [SerializeField, Range(-100, 100)]
    private float RPM = 0;

    [SerializeField, Range(0, 58)]
    private float Depth = 0;

    [SerializeField, Range(-60, 60)]
    private int RudderAngle = 0;

    [SerializeField, Range(0, 58)]
    private float DBK = 0;

    [SerializeField, Range(-100, 100)]
    private float Pitch = 0;

    [SerializeField, Range(-30, 30)]
    private float RateOfTurn = 0;

    [SerializeField, Range(-20, 20)]
    private float Roll = 0;

    public RectTransform RelWindRect;
    public RectTransform GyroRect;
    public RectTransform RPMRect, RPMRect2;
    public RectTransform DepthRect;
    public RectTransform RudderAngleImage, RudderAngleImage1, RudderAngleImage2, RudderAngleImage3, RudderAngleImage4;
    public RectTransform DBKRect;
    public RectTransform PitchRect, PitchRect2;
    public RectTransform RateOfTurnRect;
    public RectTransform RollRect;

    public Text Tiletle;
    public Text WindSpeedText, RelWindSpeedText;
    public Text GyroText, MagnText;
    public Text RPMText;
    public Text DepthText, DbkText;
    public Text PitchText;
    public Text RateOfTurnText;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }


    public override void InitFind()
    {
        base.InitFind();
        RelWindRect = FindTool.FindChildComponent<RectTransform>(transform, "table2/PointImage2");
        GyroRect = FindTool.FindChildComponent<RectTransform>(transform, "table3/PointImage3");
        RPMRect = FindTool.FindChildComponent<RectTransform>(transform, "table4/PointImage4");
        RPMRect2 = FindTool.FindChildComponent<RectTransform>(transform, "table10/tab9/PointImage4 (1)");
        DepthRect = FindTool.FindChildComponent<RectTransform>(transform, "table5/left");
        RudderAngleImage = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5");
        RudderAngleImage1 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (1)");
        RudderAngleImage2 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (2)");
        RudderAngleImage3 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (3)");
        RudderAngleImage4 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (4)");
        DBKRect = FindTool.FindChildComponent<RectTransform>(transform, "table5/right");
        PitchRect = FindTool.FindChildComponent<RectTransform>(transform, "table6/PointImage6");
        PitchRect2 = FindTool.FindChildComponent<RectTransform>(transform, "table10/tab10/PointImage4 (2)");
        RateOfTurnRect = FindTool.FindChildComponent<RectTransform>(transform, "table7/PointImage7");
        RollRect = FindTool.FindChildComponent<RectTransform>(transform, "table8/PointImage8");

        Tiletle = FindTool.FindChildComponent<Text>(transform, "TileTleText");
        WindSpeedText = FindTool.FindChildComponent<Text>(transform, "table2/WindSpeedText");
        RelWindSpeedText = FindTool.FindChildComponent<Text>(transform, "table2/RelWindSpeedText");
        GyroText = FindTool.FindChildComponent<Text>(transform, "table3/GyroText");
        MagnText = FindTool.FindChildComponent<Text>(transform, "table3/MagnText");
        RPMText = FindTool.FindChildComponent<Text>(transform, "table4/RPMText");
        DepthText = FindTool.FindChildComponent<Text>(transform, "table5/DepthText");
        DbkText = FindTool.FindChildComponent<Text>(transform, "table5/DbkText");
        PitchText = FindTool.FindChildComponent<Text>(transform, "table6/PitchText");
        RateOfTurnText = FindTool.FindChildComponent<Text>(transform, "table7/RateOfTurnText");

    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    private void Update()
    {
        //SetRelWindValue(RelWind);
        //SetGyroValue(Gyro);
        //SetRPMValue(RPM);
        //SetDepthValue(Depth);
        //SetRudderAngleValue(RudderAngle);
        //SetDBKValue(DBK);
        //SetPitchValue(Pitch);
        //SetRateOfTurnValue(RateOfTurn);
        //SetRollValue(Roll);
    }

    public void SetRelWindValue(float value)
    {
        RelWindRect.localEulerAngles = Vector3.back * value;
        RelWindSpeedText.text = Mathf.Abs(value).ToString();
    }

    public void SetGyroValue(float value)
    {
        GyroRect.localEulerAngles = Vector3.back * value;
        GyroText.text = Mathf.Abs(value).ToString();
    }

    public void SetRPMValue(float value)
    {
        float factor = value / 100 * 32;
        float factor2 = value / 100 * 25.2f;
        RPMRect.localEulerAngles = Vector3.back * factor;
        RPMRect2.localEulerAngles = Vector3.back * factor2;
        RPMText.text = Mathf.Abs(value).ToString();
    }

    public void SetDepthValue(float value)
    {
        DepthRect.sizeDelta = new Vector2(88.6f, value);
        DepthText.text = Mathf.Abs(value).ToString();

    }

    public void SetRudderAngleValue(float value)
    {
        int factor = (int)(value / 5)*10;
        RudderAngleImage.DOAnchorPosX(factor - 1,0.5f);
        RudderAngleImage1.DOAnchorPosX(factor - 1, 0.6f);
        RudderAngleImage2.DOAnchorPosX(factor - 1, 0.7f);
        RudderAngleImage3.DOAnchorPosX(factor - 1, 0.8f);
        RudderAngleImage4.DOAnchorPosX(factor - 1, 0.9f);
    }

    public void SetDBKValue(float value)
    {
        DBKRect.sizeDelta = new Vector2(88.6f, value);
        DbkText.text = Mathf.Abs(value).ToString();
    }

    public void SetPitchValue(float value)
    {
        PitchRect.localEulerAngles = Vector3.back * (value / 100 * 32);
        PitchRect2.localEulerAngles = Vector3.back * (value / 100 * 24.9f);
        PitchText.text = Mathf.Abs(value).ToString();
    }

    public void SetRateOfTurnValue(float value)
    {
        RateOfTurnRect.localEulerAngles = Vector3.back * (value / 30 * 134);
        RateOfTurnText.text = Mathf.Abs(value).ToString();
    }

    public void SetRollValue(float value)
    {
        RollRect.localEulerAngles = Vector3.forward * (value / 20 * 35);
    }
}
