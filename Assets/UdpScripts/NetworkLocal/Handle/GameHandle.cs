using System;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using Proto;
using NetworkCommonTools.LiteNetLibEngine;

public class GameHandle : HandleBase
{
    MTFrame.MTEvent.EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();
    public override byte OperateHandleCode => (byte)OperateCodes.Game;

    public override void OnReceiveProcess(INetEngine netEngine, NetPeer netPeer, OperationResponse operation)
    {
        //字符串数据
        string index = operation.GetParemater<string>(ParmaterCodes.index);
        //引用类型判断是否为null
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.index);
            return;
        }

        //页面切换数据
        string PanelData = operation.GetParemater<string>(ParmaterCodes.PanelSwitchData);
        //引用类型判断是否为null
        if (PanelData != null)
        {
            SentDataToState(PanelData, ParmaterCodes.PanelSwitchData);
            return;
        }

        /*船体展示页数据*/
        string rotateX = operation.GetParemater<string>(ParmaterCodes.BoatRotateX);
        if(rotateX!=null)
        {
            SentDataToState(rotateX, ParmaterCodes.BoatRotateX);
            return;
        }

        string rotateY = operation.GetParemater<string>(ParmaterCodes.BoatRotateY);
        if (rotateX != null)
        {
            SentDataToState(rotateY, ParmaterCodes.BoatRotateY);
            return;
        }

        string rotateZ = operation.GetParemater<string>(ParmaterCodes.BoatRotateZ);
        if (rotateX != null)
        {
            SentDataToState(rotateZ, ParmaterCodes.BoatRotateZ);
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
        UnityEngine.Debug.Log(msg);
        EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, type.ToString(), eventParamete);
    }
}