﻿using System;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using Proto;
using NetworkCommonTools.LiteNetLibEngine;
using MTFrame;

public class GameHandle : HandleBase
{
    private string index;
    MTFrame.MTEvent.EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();
    public override byte OperateHandleCode => (byte)OperateCodes.Game;

    public override void OnReceiveProcess(INetEngine netEngine, NetPeer netPeer, OperationResponse operation)
    {
        index = operation.GetParemater<string>(ParmaterCodes.ControlSwitchData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.ControlSwitchData);
            return;
        }

        if (SailingSceneManage.Instance!=null && SailingSceneManage.Instance.ControlState == ControlSwitch.Open)
            return;
        //字符串数据
        index = operation.GetParemater<string>(ParmaterCodes.index);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.index);
            return;
        }

        //场景切换数据
        index = operation.GetParemater<string>(ParmaterCodes.SceneSwitch);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.SceneSwitch);
            return;
        }

        /*船体展示页数据*/
        index = operation.GetParemater<string>(ParmaterCodes.BoatRotate);
        if(index != null)
        {
            SentDataToState(index, ParmaterCodes.BoatRotate);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.Display_PlayVideo);
        if (index != null)
        {

            SentDataToState(index, ParmaterCodes.Display_PlayVideo);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.Display_VideoControl);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.Display_VideoControl);
            return;
        }

        /*模拟航行页数据*/
        index = operation.GetParemater<string>(ParmaterCodes.WeatherType);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.WeatherType);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.WeatherIntensity);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.WeatherIntensity);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.DayNightTime);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.DayNightTime);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.BoatSpeed);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.BoatSpeed);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.OceanWaveSize);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.OceanWaveSize);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.OceanLightData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.OceanLightData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.CameraState);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.CameraState);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.TargetPosition);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.TargetPosition);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.TrainModelData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.TrainModelData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.PuGuanCameraData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.PuGuanCameraData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.AutoDriveData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.AutoDriveData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.DriveTurnData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.DriveTurnData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.DriveSpeed);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.DriveSpeed);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.PuGuanSwitchData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.PuGuanSwitchData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.TurnTableData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.TurnTableData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.CraneHandData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.CraneHandData);
            return;
        }

        index = operation.GetParemater<string>(ParmaterCodes.HookData);
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.HookData);
            return;
        }
    }

    public override void OnUnconnectedRequestProcess(INetEngine netEngine, IPEndPoint endPoint, OperationResponse operation)
    {

    }

    public override void OnUnconnectedResponseProcess(INetEngine netEngine, IPEndPoint endPoint, OperationResponse operation)
    {

    }

    private void SentDataToState(string msg, ParmaterCodes parmater,TransportType type = TransportType.UdpToState)
    {
        QueueData queueData = new QueueData();
        queueData.msg = msg;
        queueData.parmaterCodes = parmater;
        eventParamete.AddParameter(queueData);
        //UnityEngine.Debug.Log(msg);
        EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, type.ToString(), eventParamete);
    }
}