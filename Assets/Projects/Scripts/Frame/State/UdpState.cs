﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using MTFrame.MTEvent;
using System;

public class QueueData
{
    public string msg;

    public ParmaterCodes parmaterCodes;
}

/// <summary>
/// 传输类型枚举，用来做事件名称
/// </summary>
public enum TransportType
{
    /// <summary>
    /// 将Udp接收到的数据传到当前脚本将其转为主线程数据
    /// </summary>
    UdpToState,
    /// <summary>
    /// 切换UI页面
    /// </summary>
    SwitchPanel,
}

/// <summary>
/// UI页面名字枚举
/// </summary>
public enum PanelName
{
    /// <summary>
    /// 待机页
    /// </summary>
    WaitPanel = 0,

    /// <summary>
    /// 船体展示页
    /// </summary>
    DisplayPanel = 1,

    /// <summary>
    /// 海上航行页
    /// </summary>
    SailingPanel = 2,
}

public class UdpState : BaseState
{
    //注意state一定要在get里面监听事件，没有的话就写成下面样子
    //这里一般用来监听Panel切换
    private Queue<QueueData> GetVs = new Queue<QueueData>();
    public override string[] ListenerMessageID
    {
        get
        {
            return new string[]
            {
                //事件名string类型
                TransportType.UdpToState.ToString(),
                TransportType.SwitchPanel.ToString(),
            };
        }
        set { }
    }

    public override void OnListenerMessage(EventParamete parameteData)
    {

        //接收监听事件的数据，然后用switch判断做处理

        //除此之外，也可以在这里监听UDP传输的数据，但是接收的数据是子线程数据，要通过队列接收，
        //然后在update转换成主线程数据，才能对数据进行处理

        if(parameteData.EvendName == TransportType.UdpToState.ToString())
        {
            QueueData queueData = parameteData.GetParameter<QueueData>()[0];
            GetVs.Enqueue(queueData);
        }

        //页面切换
        if(parameteData.EvendName == TransportType.SwitchPanel.ToString())
        {
            PanelName panelName = parameteData.GetParameter<PanelName>()[0];
            switch (panelName)
            {
                case PanelName.WaitPanel:
                    CurrentTask.ChangeTask(new WaitTask(this));
                    break;
                case PanelName.DisplayPanel:
                    CurrentTask.ChangeTask(new DisplayTask(this));
                    break;
                case PanelName.SailingPanel:
                    CurrentTask.ChangeTask(new SailingTask(this));
                    break;
                default:
                    break;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        CurrentTask.ChangeTask(new WaitTask(this));
        EventManager.AddUpdateListener(UpdateEventEnumType.Update,"OnUpdate",Onupdate);
    }

    private void Onupdate(float timeProcess)
    {
        //数据在这里转换
        lock(GetVs)
        {
            if(GetVs.Count > 0)
            {
                QueueData st = GetVs.Dequeue();
                //Debug.Log("状态类里接收到的数据：" + st.msg);
                EventParamete eventParamete = new EventParamete();
                eventParamete.AddParameter(st.msg);
                switch (st.parmaterCodes)
                {
                    case ParmaterCodes.index:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.index.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.SceneSwitch:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.SceneSwitch.ToString(), eventParamete);
                        break;
                    /* 船体展示页 */
                    case ParmaterCodes.BoatRotate:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.BoatRotate.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.Display_PlayVideo:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.Display_PlayVideo.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.Display_VideoControl:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.Display_VideoControl.ToString(), eventParamete);
                        break;
                    /* 模拟航行页 */
                    case ParmaterCodes.BoatSpeed:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.BoatSpeed.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.DayNightTime:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.DayNightTime.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.OceanLightData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.OceanLightData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.OceanWaveSize:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.OceanWaveSize.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.WeatherIntensity:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.WeatherIntensity.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.WeatherType:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.WeatherType.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.CameraState:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.CameraState.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.TargetPosition:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.TargetPosition.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.TrainModelData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.TrainModelData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.PuGuanCameraData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.PuGuanCameraData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.AutoDriveData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.AutoDriveData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.DriveTurnData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.DriveTurnData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.DriveSpeed:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.DriveSpeed.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.PuGuanSwitchData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.PuGuanSwitchData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.TurnTableData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.TurnTableData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.CraneHandData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.CraneHandData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.HookData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.HookData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.ControlSwitchData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.ControlSwitchData.ToString(), eventParamete);
                        break;
                    /*模拟航行页——UI接口数据类型*/
                    case ParmaterCodes.TimeData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.TimeData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.TemperatureData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.TemperatureData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.DepthData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.DepthData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.RoteOfTurnData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.RoteOfTurnData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.LogData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.LogData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.RAPData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.RAPData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.GyroData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.GyroData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.RollData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.RollData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.RelDirectionData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.RelDirectionData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.RelSpeedData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.RelSpeedData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.PropellerState:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.PropellerState.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.GeneratorData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.GeneratorData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.WindData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.WindData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.TrueData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.TrueData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.RelativeData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.RelativeData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.RotData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.RotData.ToString(), eventParamete);
                        break;
                    /*船体数据*/
                    case ParmaterCodes.BoatPositionData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.BoatPositionData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.BoatRotateData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.BoatRotateData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.BoatSpeedData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.BoatSpeedData.ToString(), eventParamete);
                        break;
                    default:
                        break;
                }

                
                //在这里进行switch对数据进行处理
            }
        }
    }
}
