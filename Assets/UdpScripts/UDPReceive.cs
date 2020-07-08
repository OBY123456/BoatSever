using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using MTFrame.MTEvent;
using System;
using Newtonsoft.Json;

public class DataProtocol
{
    //数据类型（类名）
    public string DataType;
    //该类转为Json字符串
    public string DataMsg;
}

public class UDPReceive : MonoBehaviour
{
    //public LeaderControll leaderControll; //外面要使用接收到信息的类

    private IPEndPoint ipEndPoint;
    private Socket socket;
    private Thread thread;

    private byte[] bytes;           //接收到的字节
    private int bytesLength;        //长度
    private string receiveMsg = "";   //接收到的信息

    EventParamete eventParamete = new EventParamete();

    void Start()
    {
        Init();
    }

    //初始化
    private void Init()
    {
        ipEndPoint = new IPEndPoint(IPAddress.Any, 8060);    //端口号要与发送端一致
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipEndPoint);

        thread = new Thread(new ThreadStart(Receive));      //开启一个线程，接收发送端的消息
        thread.IsBackground = true;
        thread.Start();
    }
    //接收消息函数
    private void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)sender;

        while (true)
        {
            if(SailingSceneManage.Instance!=null )
            {
                bytes = new byte[1024];
                bytesLength = socket.ReceiveFrom(bytes, ref remote);
                if(bytesLength <= 0)
                {
                    return;
                }
                else
                {
                    receiveMsg = Encoding.UTF8.GetString(bytes, 0, bytesLength);
                    DataClassification(receiveMsg);
                    //Debug.Log("接收的数据==" + receiveMsg);
                }

            }
            //UDPSend.Instance.Send(receiveMsg);
        }
    }

    //关闭socket，关闭thread
    private void OnDisable()
    {
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }
        if (thread != null)
        {
            thread.Interrupt();
            thread.Abort();
        }
    }

    /// <summary>
    /// 数据分类
    /// </summary>
    /// <param name="msg"></param>
    private void DataClassification(string msg)
    {
        if (SailingSceneManage.Instance != null && SailingSceneManage.Instance.ControlState == MTFrame.ControlSwitch.Hide)
            return;

        DataProtocol dataProtocol = new DataProtocol();
        dataProtocol = JsonConvert.DeserializeObject<DataProtocol>(msg);
        ParmaterCodes parmaterCodes = (ParmaterCodes)Enum.Parse(typeof(ParmaterCodes), dataProtocol.DataType);
        switch (parmaterCodes)
        {
            /*模拟航行页*/
            case ParmaterCodes.WeatherType:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.WeatherType);
                break;
            case ParmaterCodes.WeatherIntensity:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.WeatherIntensity);
                break;
            case ParmaterCodes.DayNightTime:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.DayNightTime);
                break;
            case ParmaterCodes.BoatSpeed:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.BoatSpeed);
                break;
            case ParmaterCodes.OceanWaveSize:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.OceanWaveSize);
                break;
            case ParmaterCodes.OceanLightData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.OceanLightData);
                break;
            case ParmaterCodes.CameraState:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.CameraState);
                break;
            case ParmaterCodes.TargetPosition:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.TargetPosition);
                break;
            case ParmaterCodes.TrainModelData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.TrainModelData);
                break;
            case ParmaterCodes.PuGuanCameraData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.PuGuanCameraData);
                break;
            case ParmaterCodes.AutoDriveData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.AutoDriveData);
                break;
            case ParmaterCodes.DriveTurnData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.DriveTurnData);
                break;
            case ParmaterCodes.DriveSpeed:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.DriveSpeed);
                break;
            case ParmaterCodes.PuGuanSwitchData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.PuGuanSwitchData);
                break;
            case ParmaterCodes.TurnTableData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.TurnTableData);
                break;
            case ParmaterCodes.CraneHandData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.CraneHandData);
                break;
            case ParmaterCodes.HookData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.HookData);
                break;
            /*模拟航行页——UI接口数据类型*/
            case ParmaterCodes.TimeData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.TimeData);
                break;
            case ParmaterCodes.TemperatureData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.TemperatureData);
                break;
            case ParmaterCodes.DepthData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.DepthData);
                break;
            case ParmaterCodes.RoteOfTurnData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.RoteOfTurnData);
                break;
            case ParmaterCodes.LogData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.LogData);
                break;
            case ParmaterCodes.RAPData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.RAPData);
                break;
            case ParmaterCodes.GyroData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.GyroData);
                break;
            case ParmaterCodes.RollData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.RollData);
                break;
            case ParmaterCodes.RelDirectionData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.RelDirectionData);
                break;
            case ParmaterCodes.RelSpeedData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.RelSpeedData);
                break;
            case ParmaterCodes.PropellerState:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.PropellerState);
                break;
            case ParmaterCodes.GeneratorData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.GeneratorData);
                break;
            case ParmaterCodes.WindData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.WindData);
                break;
            case ParmaterCodes.TrueData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.TrueData);
                break;
            case ParmaterCodes.RelativeData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.RelativeData);
                break;
            case ParmaterCodes.RotData:
                SentDataToState(dataProtocol.DataMsg, ParmaterCodes.RotData);
                break;
            default:
                break;
            
        }
    }

    private void SentDataToState(string msg, ParmaterCodes parmater, TransportType type = TransportType.UdpToState)
    {
        QueueData queueData = new QueueData();
        queueData.msg = msg;
        queueData.parmaterCodes = parmater;
        eventParamete.AddParameter(queueData);
        //UnityEngine.Debug.Log(msg);
        EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, type.ToString(), eventParamete);
    }
}