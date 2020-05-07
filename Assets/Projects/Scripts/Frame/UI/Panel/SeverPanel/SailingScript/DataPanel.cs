using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using DG.Tweening;

public class DataPanel : BasePanel
{
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
    public RectTransform RPMRect;
    public RectTransform DepthRect;
    public RectTransform RudderAngleImage, RudderAngleImage1, RudderAngleImage2, RudderAngleImage3, RudderAngleImage4;
    public RectTransform DBKRect;
    public RectTransform PitchRect;
    public RectTransform RateOfTurnRect;
    public RectTransform RollRect;

    public override void InitFind()
    {
        base.InitFind();
        RelWindRect = FindTool.FindChildComponent<RectTransform>(transform, "table2/PointImage2");
        GyroRect = FindTool.FindChildComponent<RectTransform>(transform, "table3/PointImage3");
        RPMRect = FindTool.FindChildComponent<RectTransform>(transform, "table4/PointImage4");
        DepthRect = FindTool.FindChildComponent<RectTransform>(transform, "table5/left");
        RudderAngleImage = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5");
        RudderAngleImage1 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (1)");
        RudderAngleImage2 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (2)");
        RudderAngleImage3 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (3)");
        RudderAngleImage4 = FindTool.FindChildComponent<RectTransform>(transform, "table5/PointImage5 (4)");
        DBKRect = FindTool.FindChildComponent<RectTransform>(transform, "table5/right");
        PitchRect = FindTool.FindChildComponent<RectTransform>(transform, "table6/PointImage6");
        RateOfTurnRect = FindTool.FindChildComponent<RectTransform>(transform, "table7/PointImage7");
        RollRect = FindTool.FindChildComponent<RectTransform>(transform, "table8/PointImage8");
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
    }

    public void SetGyroValue(float value)
    {
        GyroRect.localEulerAngles = Vector3.back * value;
    }

    public void SetRPMValue(float value)
    {
        float factor = value / 100 * 32;
        RPMRect.localEulerAngles = Vector3.back * factor;
    }

    public void SetDepthValue(float value)
    {
        DepthRect.sizeDelta = new Vector2(88.6f, value);
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
    }

    public void SetPitchValue(float value)
    {
        PitchRect.localEulerAngles = Vector3.back * (value / 100 * 32);
    }

    public void SetRateOfTurnValue(float value)
    {
        RateOfTurnRect.localEulerAngles = Vector3.back * (value / 30 * 134);
    }

    public void SetRollValue(float value)
    {
        RollRect.localEulerAngles = Vector3.forward * (value / 20 * 35);
    }
}
