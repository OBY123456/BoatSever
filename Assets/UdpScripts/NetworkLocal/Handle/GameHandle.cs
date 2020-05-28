using System;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using Proto;
using NetworkCommonTools.LiteNetLibEngine;

public class GameHandle : HandleBase
{
    private string index;
    MTFrame.MTEvent.EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();
    public override byte OperateHandleCode => (byte)OperateCodes.Game;

    public override void OnReceiveProcess(INetEngine netEngine, NetPeer netPeer, OperationResponse operation)
    {
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

        //index = operation.GetParemater<string>(ParmaterCodes.PuGuanCameraData);
        //if (index != null)
        //{
        //    SentDataToState(index, ParmaterCodes.PuGuanCameraData);
        //    return;
        //}
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