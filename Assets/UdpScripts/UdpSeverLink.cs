using NetworkCommonTools;
using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MTFrame.MTEvent;
using System;
using Newtonsoft.Json;
using MTFrame;
using UnityEngine.SceneManagement;

//****Udp服务器端****
//****数据接收在GameHandle脚本接收****
//****要在PlayerSettings->otherSettings里面将Api Compatibility Level设置为.net4.x 如果没有则无法使用

public class UdpSeverLink : MonoBehaviour
{
    public static UdpSeverLink Instance;
    public GameLocalServerEngineListener localServerEngine;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //打印debug信息，如果不需要可以注释掉
        Log.Init(new UnityDebug(), true);
        Log.LogIsDebug[Log.LogType.Normal] = true;
        Log.WriteLine("开始");

        UserManager.Instance.LocalUser = new User() { ID = "001", nickname = "xxx" };
        //设置房间信息
        RoomManager.Instance.LocalRoom = new Room(UserManager.Instance.LocalUser, 4);
        RoomManager.Instance.LocalRoom.RoomInfo.RoomName = "房间001";

        localServerEngine = new GameLocalServerEngineListener(9999, "Test4");
        localServerEngine.Creat();

        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.PanelSwitchData.ToString(), callback);
    }

    private void callback(EventParamete parameteData)
    {
        if(parameteData.EvendName == ParmaterCodes.PanelSwitchData.ToString())
        {
            string data = parameteData.GetParameter<string>()[0];
            PanelSwitchData switchData = new PanelSwitchData();
            switchData = JsonConvert.DeserializeObject<PanelSwitchData>(data);

            PanelName name = (PanelName)Enum.Parse(typeof(PanelName), switchData.PanelName);
            Debug.Log("切换场景 ===" + name.ToString());

            switch (name)
            {
                case PanelName.WaitPanel:
                    //if (UIManager.GetPanel<WaitPanel>(WindowTypeEnum.ForegroundScreen).IsOpen)
                    //    return;
                    PanelChange(PanelName.WaitPanel);
                    SceneManager.LoadScene(SceneName.WaitScene.ToString(), MTFrame.MTScene.LoadingModeType.UnityLocal);
                    Main.Instance.MainCamera.gameObject.SetActive(true);
                    break;

                case PanelName.IntroductionPanel:
                    //if (UIManager.GetPanel<IntroductionPanel>(WindowTypeEnum.ForegroundScreen).IsOpen)
                    //    return;
                    PanelChange(PanelName.IntroductionPanel);
                    SceneManager.LoadScene(SceneName.WaitScene.ToString(), MTFrame.MTScene.LoadingModeType.UnityLocal);
                    Main.Instance.MainCamera.gameObject.SetActive(true);
                    break;

                case PanelName.DisplayPanel:
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneName.DisplayScene.ToString())
                        return;
                    WaitPanel.Instance.SetName(SceneName.DisplayScene, PanelName.DisplayPanel);
                    PanelChange(PanelName.LoadingPanel);
                    break;

                case PanelName.DpPanel:
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneName.DPScene.ToString())
                        return;
                    WaitPanel.Instance.SetName(SceneName.DPScene, PanelName.DpPanel);
                    PanelChange(PanelName.LoadingPanel);
                    break;

                case PanelName.WorkPanel:
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneName.WorkScene.ToString())
                        return;
                    WaitPanel.Instance.SetName(SceneName.WorkScene, PanelName.WorkPanel);
                    PanelChange(PanelName.LoadingPanel);
                    break;

                case PanelName.SailingPanel:
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneName.PirateCove1.ToString())
                        return;
                    WaitPanel.Instance.SetName(SceneName.PirateCove1, PanelName.SailingPanel);
                    PanelChange(PanelName.LoadingPanel);
                    break;

                default:
                    break;
            }

        }
    }

    private void Update()
    {
        //测试用
        if(Input.GetKeyDown(KeyCode.A))
        {
            SendDataToClient(ParmaterCodes.index,"你好，客户端！");
        }
    }

    private void OnDestroy()
    {
        localServerEngine.ShutDown();
    }

    /// <summary>
    /// 发送数据给客户端
    /// </summary>
    /// <param name="parmaterCodes"></param>
    /// <param name="obj"></param>
    public void SendDataToClient(ParmaterCodes parmaterCodes, object obj)
    {
        OperationResponse response = OperationResponseExtend.GetOperationResponse((byte)OperateCodes.Game);
        switch (parmaterCodes)
        {
            case ParmaterCodes.index:
                response.AddParemater((byte)ParmaterCodes.index, obj);
                Debug.Log("发送信息给客户端:" + obj);
                break;
            default:
                break;
        }
        localServerEngine?.SendData(response);
    }

    /// <summary>
    /// 切换Panel
    /// </summary>
    /// <param name="panelName"></param>
    public void PanelChange(PanelName panelName)
    {
        EventParamete eventParamete = new EventParamete();
        eventParamete.AddParameter(panelName);
        EventManager.TriggerEvent(GenericEventEnumType.Message, TransportType.SwitchPanel.ToString(), eventParamete);
    }
}


