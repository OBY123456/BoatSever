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
    /// 产品介绍页
    /// </summary>
    IntroductionPanel = 1,

    /// <summary>
    /// 船体展示页
    /// </summary>
    DisplayPanel = 2,

    /// <summary>
    /// DP动力定位页
    /// </summary>
    DpPanel = 3,

    /// <summary>
    /// 作业工况页
    /// </summary>
    WorkPanel = 4,

    /// <summary>
    /// 海上航行页
    /// </summary>
    SailingPanel = 5,

    /// <summary>
    /// 加载页
    /// </summary>
    LoadingPanel = 6,
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
                case PanelName.IntroductionPanel:
                    CurrentTask.ChangeTask(new IntroductionTask(this));
                    break;
                case PanelName.DisplayPanel:
                    CurrentTask.ChangeTask(new DisplayTask(this));
                    break;
                case PanelName.DpPanel:
                    CurrentTask.ChangeTask(new DpTask(this));
                    break;
                case PanelName.WorkPanel:
                    CurrentTask.ChangeTask(new WorkTask(this));
                    break;
                case PanelName.SailingPanel:
                    CurrentTask.ChangeTask(new SailingTask(this));
                    break;
                case PanelName.LoadingPanel:
                    CurrentTask.ChangeTask(new LoadingTask(this));
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
                Debug.Log("状态类里接收到的数据：" + st.msg);
                EventParamete eventParamete = new EventParamete();
                eventParamete.AddParameter(st.msg);
                switch (st.parmaterCodes)
                {
                    case ParmaterCodes.index:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.index.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.PanelSwitchData:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.PanelSwitchData.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.BoatRotate:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.BoatRotate.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.BoatRotateY:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.BoatRotateY.ToString(), eventParamete);
                        break;
                    default:
                        break;
                }

                
                //在这里进行switch对数据进行处理
            }
        }
    }
}
