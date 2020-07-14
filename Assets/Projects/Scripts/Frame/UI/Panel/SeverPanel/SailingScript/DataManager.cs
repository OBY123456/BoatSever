using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using MTFrame.MTEvent;
using System;
using Newtonsoft.Json;
using DG.Tweening;

public enum DataPanelName
{
    PropellerStatePanel,
    FirstPersonDataPanel,
    RearViewDataPanel,
}

public class DataManager : MonoBehaviour
{
    //public Display6FPPanel[] display6FPPanels;
    //private Rigidbody BoatRb;
    //private Transform Boat;

    MTFrame.MTEvent.EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();

    /// <summary>
    /// 单独控制左右两个推进器是否旋转
    /// </summary>
    private bool IsRotate1, IsRotate2;

    /// <summary>
    /// 分别判断两个推进器是否产生了向后的推进力
    /// </summary>
    private bool IsBack1, IsBack2;

    /// <summary>
    /// 两个推进器的转速
    /// </summary>
    private float Current_Speed1, Current_Speed2;

    private float RotateSpeed_Min = 30;
    private float RatateSpeed_Max = 70;
    

    // Start is called before the first frame update
    void Start()
    {
        //BoatRb = SailingSceneManage.Instance.boatProbes.gameObject.GetComponent<Rigidbody>();
        //Boat = BoatRb.gameObject.transform;
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TimeData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TemperatureData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.DepthData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.RoteOfTurnData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.LogData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.RAPData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.GyroData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.RollData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.RelDirectionData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.RelSpeedData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.PropellerState.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.GeneratorData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.WindData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TrueData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.RelativeData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.RotData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.BoatPositionData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.BoatRotateData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.BoatSpeedData.ToString(), Callback);
    }

    private void Callback(EventParamete parameteData)
    {

        ParmaterCodes codes = (ParmaterCodes)Enum.Parse(typeof(ParmaterCodes), parameteData.EvendName);
        string msg = parameteData.GetParameter<string>()[0];
        switch (codes)
        {
            case ParmaterCodes.TimeData:
                SentDataToPanel(msg, ParmaterCodes.TimeData, DataPanelName.FirstPersonDataPanel);
                break;
            case ParmaterCodes.TemperatureData:
                SentDataToPanel(msg, ParmaterCodes.TemperatureData, DataPanelName.FirstPersonDataPanel);
                break;
            case ParmaterCodes.DepthData:
                SentDataToPanel(msg, ParmaterCodes.DepthData, DataPanelName.FirstPersonDataPanel);
                SentDataToPanel(msg, ParmaterCodes.DepthData, DataPanelName.RearViewDataPanel);
                break;
            case ParmaterCodes.RoteOfTurnData:
                SentDataToPanel(msg, ParmaterCodes.RoteOfTurnData, DataPanelName.FirstPersonDataPanel);
                break;
            case ParmaterCodes.LogData:
                SentDataToPanel(msg, ParmaterCodes.LogData, DataPanelName.FirstPersonDataPanel);
                break;
            case ParmaterCodes.RAPData:
                SentDataToPanel(msg, ParmaterCodes.RAPData, DataPanelName.FirstPersonDataPanel);
                SentDataToPanel(msg, ParmaterCodes.RAPData, DataPanelName.PropellerStatePanel);
                //SetPropeller(msg);
                break;
            case ParmaterCodes.GyroData:
                SentDataToPanel(msg, ParmaterCodes.GyroData, DataPanelName.FirstPersonDataPanel);
                break;
            case ParmaterCodes.RollData:
                SentDataToPanel(msg, ParmaterCodes.RollData, DataPanelName.FirstPersonDataPanel);
                SentDataToPanel(msg, ParmaterCodes.RollData, DataPanelName.RearViewDataPanel);
                break;
            case ParmaterCodes.RelDirectionData:
                SentDataToPanel(msg, ParmaterCodes.RelDirectionData, DataPanelName.FirstPersonDataPanel);
                break;
            case ParmaterCodes.RelSpeedData:
                SentDataToPanel(msg, ParmaterCodes.RelSpeedData, DataPanelName.FirstPersonDataPanel);
                break;
            case ParmaterCodes.PropellerState:
                SentDataToPanel(msg, ParmaterCodes.PropellerState, DataPanelName.PropellerStatePanel);
                break;
            case ParmaterCodes.GeneratorData:
                SentDataToPanel(msg, ParmaterCodes.GeneratorData, DataPanelName.PropellerStatePanel);
                break;
            case ParmaterCodes.WindData:
                SentDataToPanel(msg, ParmaterCodes.WindData, DataPanelName.RearViewDataPanel);
                break;
            case ParmaterCodes.TrueData:
                SentDataToPanel(msg, ParmaterCodes.TrueData, DataPanelName.RearViewDataPanel);
                break;
            case ParmaterCodes.RelativeData:
                SentDataToPanel(msg, ParmaterCodes.RelativeData, DataPanelName.RearViewDataPanel);
                break;
            case ParmaterCodes.RotData:
                SentDataToPanel(msg, ParmaterCodes.RotData, DataPanelName.RearViewDataPanel);
                break;
            case ParmaterCodes.BoatPositionData:
                if (SailingSceneManage.Instance.CurrentTrainModel != TrainModel.Lifting)
                    SetBoatPosition(msg);
                break;
            case ParmaterCodes.BoatRotateData:
                if (SailingSceneManage.Instance.CurrentTrainModel != TrainModel.Lifting)
                    SetBoatRotate(msg);
                break;
            case ParmaterCodes.BoatSpeedData:
                if (SailingSceneManage.Instance.CurrentTrainModel != TrainModel.Lifting)
                    SetBoatSpeed(msg);
                break;
            default:
                break;
        }
    }

    private void SetBoatSpeed(string msg)
    {
        BoatSpeedData boatspeed = new BoatSpeedData();
        boatspeed = JsonConvert.DeserializeObject<BoatSpeedData>(msg);
        if (boatspeed.value > 20)
        {
            boatspeed.value = 20;
        }

        if (boatspeed.value < -20)
        {
            boatspeed.value = -20;
        }
        SailingSceneManage.Instance.boatProbes._enginePower = boatspeed.value;

    }

    private void SetBoatPosition(string msg)
    {
        BoatPositionData data = new BoatPositionData();
        data = JsonConvert.DeserializeObject<BoatPositionData>(msg);

        if(data.x < -4000)
        {
            data.x = -4000;
        }

        if(data.x > 4000)
        {
            data.x = 4000;
        }

        if (data.z < -4000)
        {
            data.z = -4000;
        }

        if (data.z > 4000)
        {
            data.z = 4000;
        }
        SailingSceneManage.Instance.boatProbes.gameObject.GetComponent<Transform>().position = new Vector3(data.x, SailingSceneManage.Instance.boatProbes.gameObject.GetComponent<Transform>().position.y, data.z);
    }

    private void SetBoatRotate(string msg)
    {
        BoatRotateData data = new BoatRotateData();
        data = JsonConvert.DeserializeObject<BoatRotateData>(msg);
        if (data.value < 0)
        {
            data.value = 0;
        }

        if (data.value > 360)
        {
            data.value = 360;
        }
        SailingSceneManage.Instance.boatProbes.gameObject.GetComponent<Transform>().localEulerAngles = new Vector3(SailingSceneManage.Instance.boatProbes.gameObject.GetComponent<Transform>().localEulerAngles.x, data.value, SailingSceneManage.Instance.boatProbes.gameObject.GetComponent<Transform>().localEulerAngles.z);
    }

    /// <summary>
    /// 根据接收到的推进器数据改变推进器状态
    /// </summary>
    /// <param name="msg"></param>
    private void SetPropeller(string msg)
    {
        RAPData data = new RAPData();
        data = JsonConvert.DeserializeObject<RAPData>(msg);
        Direction direction = (Direction)Enum.Parse(typeof(Direction), data.direction);

        if(data.RPMValue > 110)
        {
            data.RPMValue = 110;
        }

        if(data.PitchValue > 110)
        {
            data.PitchValue = 110;
        }

        if(data.AngleValue < -180)
        {
            data.AngleValue = -180;
        }

        if(data.AngleValue > 180)
        {
            data.AngleValue = 180;
        }

        switch (direction)
        {
            case Direction.Left:
                /*推进力部分*/
                if(data.RPMValue > 0)
                {
                    IsRotate1 = true;
                    Current_Speed1 = RotateSpeed_Min + data.RPMValue / 110f * (RatateSpeed_Max - RotateSpeed_Min);
                    if (IsBack1)
                    {
                        SailingSceneManage.Instance.boatProbes._enginePower = -data.RPMValue / 110f * 10;      
                    }
                    else
                    {
                        SailingSceneManage.Instance.boatProbes._enginePower = data.RPMValue / 110f * 10;
                    }
                }
                else
                {
                    IsRotate1 = false;
                    SailingSceneManage.Instance.boatProbes._enginePower = 0;
                }

                /*转向部分*/
                SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[0].DOKill();
                SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[0].DOLocalRotate(Vector3.forward * data.AngleValue, 1.5f).SetEase(Ease.Linear);
                if(data.AngleValue > 0)
                {
                    SailingSceneManage.Instance.boatProbes._turnPower = 0.5f;
                    if(data.AngleValue > 90 && !IsBack1)
                    {
                        IsBack1 = true;
                        if (SailingSceneManage.Instance.boatProbes._enginePower > 0)
                            SailingSceneManage.Instance.boatProbes._enginePower = -SailingSceneManage.Instance.boatProbes._enginePower;
                    }

                    if (data.AngleValue < 90 && IsBack1)
                    {
                        IsBack1 = false;
                        if (SailingSceneManage.Instance.boatProbes._enginePower < 0)
                            SailingSceneManage.Instance.boatProbes._enginePower = -SailingSceneManage.Instance.boatProbes._enginePower;
                    }
                }
                else if(data.AngleValue < 0)
                {
                    SailingSceneManage.Instance.boatProbes._turnPower = -0.5f;
                    if (data.AngleValue < -90 && !IsBack1)
                    {
                        IsBack1 = true;
                        if (SailingSceneManage.Instance.boatProbes._enginePower > 0)
                            SailingSceneManage.Instance.boatProbes._enginePower = -SailingSceneManage.Instance.boatProbes._enginePower;
                    }

                    if (data.AngleValue > -90 && IsBack1)
                    {
                        IsBack1 = false;
                        if (SailingSceneManage.Instance.boatProbes._enginePower < 0)
                            SailingSceneManage.Instance.boatProbes._enginePower = -SailingSceneManage.Instance.boatProbes._enginePower;
                    }
                }
                else if(data.AngleValue == 0)
                {
                    IsBack1 = false;
                    SailingSceneManage.Instance.boatProbes._turnPower = 0;
                }

                break;
            case Direction.Right:
                /*推进力部分*/
                if (data.RPMValue > 0)
                {
                    IsRotate2 = true;
                    Current_Speed2 = RotateSpeed_Min + data.RPMValue / 110f * (RatateSpeed_Max - RotateSpeed_Min);
                    if (IsBack2)
                    {
                        SailingSceneManage.Instance.boatProbes._enginePower2 = -data.RPMValue / 110f * 10;
                    }
                    else
                    {
                        SailingSceneManage.Instance.boatProbes._enginePower2 = data.RPMValue / 110f * 10;
                    }
                }
                else
                {
                    IsRotate2 = false;
                    SailingSceneManage.Instance.boatProbes._enginePower2 = 0;
                }

                /*转向部分*/
                SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[1].DOKill();
                SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[1].DOLocalRotate(Vector3.forward * data.AngleValue, 1.5f).SetEase(Ease.Linear);
                if (data.AngleValue > 0)
                {
                    SailingSceneManage.Instance.boatProbes._turnPower2 = 0.5f;
                    if (data.AngleValue > 90 && !IsBack2)
                    {
                        IsBack2 = true;
                        if (SailingSceneManage.Instance.boatProbes._enginePower2 > 0)
                            SailingSceneManage.Instance.boatProbes._enginePower2 = -SailingSceneManage.Instance.boatProbes._enginePower2;
                    }

                    if (data.AngleValue < 90 && IsBack2)
                    {
                        IsBack2 = false;
                        if (SailingSceneManage.Instance.boatProbes._enginePower2 < 0)
                            SailingSceneManage.Instance.boatProbes._enginePower2 = -SailingSceneManage.Instance.boatProbes._enginePower2;
                    }
                }
                else if (data.AngleValue < 0)
                {
                    SailingSceneManage.Instance.boatProbes._turnPower2 = -0.5f;
                    if (data.AngleValue < -90 && !IsBack2)
                    {
                        IsBack2 = true;
                        if (SailingSceneManage.Instance.boatProbes._enginePower2 > 0)
                            SailingSceneManage.Instance.boatProbes._enginePower2 = -SailingSceneManage.Instance.boatProbes._enginePower2;
                    }

                    if (data.AngleValue > -90 && IsBack2)
                    {
                        IsBack2 = false;
                        if (SailingSceneManage.Instance.boatProbes._enginePower2 < 0)
                            SailingSceneManage.Instance.boatProbes._enginePower2 = -SailingSceneManage.Instance.boatProbes._enginePower2;
                    }
                }
                else if (data.AngleValue == 0)
                {
                    IsBack2 = false;
                    SailingSceneManage.Instance.boatProbes._turnPower2 = 0;
                }
                break;
            case Direction.LeftDown:
                break;
            default:
                break;
        }
    }

    private void SentDataToPanel(string msg, ParmaterCodes parmaterCodes, DataPanelName dataPanelName)
    {
        QueueData data = new QueueData();
        data.msg = msg;
        data.parmaterCodes = parmaterCodes;
        eventParamete.AddParameter(data);
        EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, dataPanelName.ToString(), eventParamete);
    }

    private void Update()
    {
        if(IsRotate1)
        {
            SailingSceneManage.Instance.autoDrive.animationControl.Engine_Propeller[0].Rotate(Vector3.right * Current_Speed1);
        }

        if (IsRotate2)
        {
            SailingSceneManage.Instance.autoDrive.animationControl.Engine_Propeller[1].Rotate(Vector3.right * Current_Speed2);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if(display6FPPanels != null && BoatRb!=null)
    //    {
    //        display6FPPanels[0].SetRateOfTurnValue(BoatRb.angularVelocity.y * 1000);
    //        display6FPPanels[0].SetGyroValue(Boat.eulerAngles.y);
    //        display6FPPanels[0].MagnText.text = (Boat.eulerAngles.y + Random.Range(-2f, 2f)).ToString("#0.0");
    //        display6FPPanels[0].SetRPMValue(80 + Random.Range(-2, 2));
    //        display6FPPanels[0].SetDepthValue(4500 + Boat.transform.position.y);
    //        //display6FPPanels[0].SetRelWindValue(WeatherManager.Instance.Wind_Rotate);
    //        display6FPPanels[0].SetRollValue((Boat.eulerAngles.y - 360)/360 * 20);
    //        display6FPPanels[0].SetLogValue(BoatRb.angularVelocity.x *1000, BoatRb.angularVelocity.y * 1000, BoatRb.angularVelocity.x *1000);
    //    }
    //}

    private void OnDestroy()
    {
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TimeData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TemperatureData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.DepthData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.RoteOfTurnData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.LogData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.RAPData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.GyroData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.RollData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.RelDirectionData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.RelSpeedData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.PropellerState.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.GeneratorData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.WindData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TrueData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.RelativeData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.RotData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.BoatPositionData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.BoatRotateData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.BoatSpeedData.ToString(), Callback);
    }

    public void Reset()
    {
        IsBack1 = IsBack2 = IsRotate1 = IsRotate2 = false;

        SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[0].DOKill();
        SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[1].DOKill();

        SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[0].localEulerAngles = Vector3.zero;
        SailingSceneManage.Instance.autoDrive.animationControl.Engine_Shaft[1].localEulerAngles = Vector3.zero;

        SailingSceneManage.Instance.boatProbes._turnPower = SailingSceneManage.Instance.boatProbes._turnPower2 = 0;
        SailingSceneManage.Instance.boatProbes._enginePower = SailingSceneManage.Instance.boatProbes._enginePower2 = 0;

        SailingSceneManage.Instance.boatProbes.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Current_Speed1 = Current_Speed2 = 0;

        WeatherManager.Instance.SetWindIntensity(0);
        WeatherManager.Instance.SetWindRotate(-103.6f);
    }
}
