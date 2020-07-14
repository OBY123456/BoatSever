﻿using Crest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MTFrame.MTEvent;
using System;
using DigitalRuby.WeatherMaker;
using Newtonsoft.Json;
using MTFrame;
using UnityEngine.UI;

public enum FogType
{
    Day_Sunny,
    Day_Cloudy,
    Night_Sunny,
    Night_Cloudy,
}

public class SailingSceneManage : MonoBehaviour
{
    public static SailingSceneManage Instance;

    public Camera[] CameraGroup;
    public Camera[] ThirdPersonCamera;

    public GameObject[] Target;
    public GameObject PPV;

    //public Minimap minimap;

    public BoatProbes boatProbes;
    public DataManager dataManager;

    public Transform[] WaveGroup;

    public AutoDrive autoDrive;

    private float Transition_Time = 4.0f;

    public Light SceneLight;

    public GameObject[] ZuanJingPingTaiLight;

    private float SceneLightIntensity_day = 1f;
    private float SceneLightIntensity_Rain = 0.4f;
    private float SceneLightIntensity_Night = 0.01f;

    private Color LightColor_Sunyday = new Color(229/255f,226/255f,219/255f,1);
    private Color LightColor_RainDay = new Color(153/255f,200/255f,226/255f,1);

    public GameObject DayLightGroup;

    public Material[] Skybox;

    //铺管装置
    //public GameObject Boat_PuGuan;

    //public CameraFallow[] MainCameraFallow;
    public RectTransform Display6Rect;

    public Transform ParticleMask;

    public WeatherMakerFallingParticleScript3D weatherMakerFallingParticleScript3D;

    //是否是晚上
    public bool IsNight;

    /// <summary>
    /// 是否在倒退状态
    /// </summary>
    private bool IsTurnBack;

    /// <summary>
    /// 吊装平台
    /// </summary>
    public Transform DiaoZhuangPingTai;

    //public Transform DiaoZhuangCanvas;

    public Cargo[] Cargos;

    public CapsuleCollider BoatCollider;
    public GameObject DiaoZhuangCollider;

    public TrainModel CurrentTrainModel;
    public ControlSwitch ControlState;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.BoatSpeed.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.DayNightTime.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.OceanLightData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.OceanWaveSize.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.WeatherIntensity.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.WeatherType.ToString(), Callback);
        //EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TargetPosition.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.CameraState.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TrainModelData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.AutoDriveData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanCameraData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.DriveTurnData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.DriveSpeed.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanSwitchData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TurnTableData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.CraneHandData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.HookData.ToString(), Callback);

        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.ControlSwitchData.ToString(), Callback2);

        //Time_Night();
        CurrentTrainModel = TrainModel.Transitions;
        ControlState = ControlSwitch.Hide;
        Cargos = DiaoZhuangPingTai.Find("CargoGroup").GetComponent<Transform>().GetComponentsInChildren<Cargo>();
        TransitionsModelReset();
        Time_Day();
    }

    private void Callback2(EventParamete parameteData)
    {
        ParmaterCodes codes = (ParmaterCodes)Enum.Parse(typeof(ParmaterCodes), parameteData.EvendName);
        string msg = parameteData.GetParameter<string>()[0];
        switch (codes)
        {
            case ParmaterCodes.ControlSwitchData:
                ControlSwitchData data = new ControlSwitchData();
                data = JsonConvert.DeserializeObject<ControlSwitchData>(msg);
                ControlSwitch controlSwitch = (ControlSwitch)Enum.Parse(typeof(ControlSwitch), data.state);
                switch (controlSwitch)
                {
                    case ControlSwitch.Open:
                        ControlState = ControlSwitch.Open;
                        autoDrive.CloseAutoDrive();
                        PipelineManager.instance.Stop();
                        break;
                    case ControlSwitch.Hide:
                        ControlState = ControlSwitch.Hide;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        dataManager.Reset();
    }

    private void Callback(EventParamete parameteData)
    {

        ParmaterCodes codes = (ParmaterCodes)Enum.Parse(typeof(ParmaterCodes), parameteData.EvendName);
        string msg = parameteData.GetParameter<string>()[0];
        switch (codes)
        {
            case ParmaterCodes.WeatherType:
                if (CurrentTrainModel != TrainModel.Lifting)
                    SetWeatherType(msg);
                break;
            case ParmaterCodes.WeatherIntensity:
                if (CurrentTrainModel != TrainModel.Lifting)
                    SetWeatherIntensity(msg);
                break;
            case ParmaterCodes.DayNightTime:
                if (CurrentTrainModel != TrainModel.Lifting)
                    SetDateTime(msg);
                break;
            case ParmaterCodes.BoatSpeed:
                if (CurrentTrainModel == TrainModel.Transitions && !autoDrive.IsAutoDrive)
                    SetBoatSpeed(msg);
                break;
            case ParmaterCodes.OceanWaveSize:
                if (CurrentTrainModel != TrainModel.Lifting)
                    SetOceanWaveSize(msg);
                break;
            case ParmaterCodes.OceanLightData:
                if (CurrentTrainModel != TrainModel.Lifting)
                    SetOceanLightData(msg);
                break;
            case ParmaterCodes.CameraState:
                if (CurrentTrainModel != TrainModel.Lifting)
                    SetCameraState(msg);
                break;
            case ParmaterCodes.TrainModelData:
                TrainModelChange(msg);
                break;
            case ParmaterCodes.PuGuanCameraData:
                if(CurrentTrainModel != TrainModel.Lifting)
                SetPuGuanCameraState(msg);
                break;
            case ParmaterCodes.AutoDriveData:
                if (CurrentTrainModel == TrainModel.Transitions)
                    StartDrive(msg);
                break;
            case ParmaterCodes.DriveTurnData:
                if (CurrentTrainModel == TrainModel.Transitions && !autoDrive.IsAutoDrive)
                    SetDriveTurn(msg);
                break;
            case ParmaterCodes.DriveSpeed:
                if (CurrentTrainModel == TrainModel.Transitions && !autoDrive.IsAutoDrive)
                    SetDriveSpeed(msg);
                break;
            case ParmaterCodes.PuGuanSwitchData:
                if (CurrentTrainModel != TrainModel.Lifting)
                    SetPuGuanSwitch(msg);
                break;
            case ParmaterCodes.TurnTableData:
                if (CurrentTrainModel == TrainModel.Lifting)
                    SetTurnTable(msg);
                break;
            case ParmaterCodes.CraneHandData:
                if (CurrentTrainModel == TrainModel.Lifting)
                    SetCraneHand(msg);
                break;
            case ParmaterCodes.HookData:
                if (CurrentTrainModel == TrainModel.Lifting)
                    SetHookState(msg);
                break;
            default:
                break;
        }
    }

    private void StartDrive(string msg)
    {
        AutoDriveData autoDriveData = new AutoDriveData();
        autoDriveData = JsonConvert.DeserializeObject<AutoDriveData>(msg);
        AutoDriveSwitch driveEnum = (AutoDriveSwitch)Enum.Parse(typeof(AutoDriveSwitch), autoDriveData.state);
        switch (driveEnum)
        {
            case AutoDriveSwitch.Open:
                if(autoDrive.IsReset)
                {
                    autoDrive.StartAutoDrive();
                }
                break;
            case AutoDriveSwitch.Reset:
                autoDrive.Reset_ZhuanChuang();
                PipelineManager.instance.Stop();
                IsTurnBack = false;
                break;
            case AutoDriveSwitch.Close:
                autoDrive.CloseAutoDrive();
                IsTurnBack = false;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 手动模式
    /// </summary>
    /// <param name="msg"></param>
    private void SetDriveTurn(string msg)
    {
        DriveTurnData turnData = new DriveTurnData();
        turnData = JsonConvert.DeserializeObject<DriveTurnData>(msg);
        DriveTurn state = (DriveTurn)Enum.Parse(typeof(DriveTurn), turnData.state);
        switch (state)
        {
            case DriveTurn.TurnLeft:
                boatProbes._turnPower = -0.5f;
                autoDrive.animationControl.ShaftRotate(45f, 3.0f);
                break;
            case DriveTurn.TurnRight:
                boatProbes._turnPower = 0.5f;
                autoDrive.animationControl.ShaftRotate(-45f, 3.0f);
                break;
            case DriveTurn.TurnBack:
                if(IsTurnBack)
                {
                    boatProbes._enginePower = Mathf.Abs(boatProbes._enginePower);
                    IsTurnBack = false;
                }
                else
                {
                    boatProbes._enginePower = -boatProbes._enginePower;
                    IsTurnBack = true;
                }
                break;
            case DriveTurn.Complete:
                boatProbes._turnPower = 0;
                //boatProbes._enginePower = Mathf.Abs(boatProbes._enginePower);
                autoDrive.animationControl.ShaftRotate(0, 3.0f);
                break;
            default:
                break;
        }
    }

    private void SetDriveSpeed(string msg)
    {
        DriveSpeed driveSpeed = new DriveSpeed();
        driveSpeed = JsonConvert.DeserializeObject<DriveSpeed>(msg);

        if(driveSpeed.value <= 0)
        {
            autoDrive.animationControl.IsRotate = false;
        }
        else
        {
           autoDrive.animationControl.IsRotate = true;
        }

        if(IsTurnBack)
        {
            boatProbes._enginePower = -(driveSpeed.value / 30.0f * 20.0f);
        }
        else
        {
            boatProbes._enginePower = driveSpeed.value / 30f * 20f;
        }
        
    }

    private void SetPuGuanSwitch(string msg)
    {
        PuGuanSwitchData data = new PuGuanSwitchData();
        data = JsonConvert.DeserializeObject<PuGuanSwitchData>(msg);
        PuGuanSwitch type = (PuGuanSwitch)Enum.Parse(typeof(PuGuanSwitch), data.state);
        switch (type)
        {
            case PuGuanSwitch.Open:
                PipelineManager.instance.Play();
                break;
            case PuGuanSwitch.Close:
                PipelineManager.instance.Stop();
                break;
            default:
                break;
        }
    }

    private void SetTurnTable(string msg)
    {
        TurnTableData data = new TurnTableData();
        data = JsonConvert.DeserializeObject<TurnTableData>(msg);
        autoDrive.animationControl.TurnTableRotate(data.value);
    }

    private void SetCraneHand(string msg)
    {
        CraneHandData data = new CraneHandData();
        data = JsonConvert.DeserializeObject<CraneHandData>(msg);
        autoDrive.animationControl.CraneHandRotate(data.value);
    }

    private void SetHookState(string msg)
    {
        HookData data = new HookData();
        data = JsonConvert.DeserializeObject<HookData>(msg);
        HookState state = (HookState)Enum.Parse(typeof(HookState), data.state);
        switch (state)
        {
            case HookState.Down:
                autoDrive.animationControl.Hookstate(HookState.Down);
                break;
            case HookState.Up:
                autoDrive.animationControl.Hookstate(HookState.Up);
                break;
            case HookState.Stop:
                autoDrive.animationControl.Hookstate(HookState.Stop);
                break;
            case HookState.Reset:
                autoDrive.animationControl.Hookstate(HookState.Reset);
                ResetCargos();
                break;
            case HookState.PutDown:
                autoDrive.animationControl.Hookstate(HookState.Stop);
                foreach (Cargo item in Cargos)
                {
                    item.PutDown();
                }
                autoDrive.animationControl.Hookstate(HookState.PutDown);
                break;
            default:
                break;
        }
    }

    private void SetWeatherType(string msg)
    {
        WeatherType weatherType = new WeatherType();
        weatherType = JsonConvert.DeserializeObject<WeatherType>(msg);
        WeatherMakerPrecipitationType type = (WeatherMakerPrecipitationType)Enum.Parse(typeof(WeatherMakerPrecipitationType), weatherType.weather);
        WeatherManager.Instance.SetWeather(type,weatherType.value);
    }

    private void SetWeatherIntensity(string msg)
    {
        WeatherIntensity intensity = new WeatherIntensity();
        intensity = JsonConvert.DeserializeObject<WeatherIntensity>(msg);
        WeatherManager.Instance.SetPrecipitationIntensity(intensity.value);
    }

    //private void SetDateTime(string msg)
    //{
    //    MTFrame.DateTime dateTime = new MTFrame.DateTime();
    //    dateTime = JsonConvert.DeserializeObject<MTFrame.DateTime>(msg);
    //    WeatherManager.Instance.SetTime(dateTime.value);
    //}

    private void SetDateTime(string msg)
    {
        DayNightTime dayNight = new DayNightTime();
        dayNight = JsonConvert.DeserializeObject<DayNightTime>(msg);
        DayNightCycle cycle = (DayNightCycle)Enum.Parse(typeof(DayNightCycle), dayNight.DayNight);
        switch (cycle)
        {
            case DayNightCycle.day:
                Time_Day();
                break;
            case DayNightCycle.night:
                Time_Night();
                break;
            default:
                break;
        }
    }

    private void SetBoatSpeed(string msg)
    {
        BoatSpeed boatspeed = new BoatSpeed();
        boatspeed = JsonConvert.DeserializeObject<BoatSpeed>(msg);
        boatProbes._enginePower = boatspeed.speed;
        autoDrive.SailingSpeed = boatspeed.speed;
    }

 

    private void SetOceanWaveSize(string msg)
    {
        OceanWaveSize waveSize = new OceanWaveSize();
        waveSize = JsonConvert.DeserializeObject<OceanWaveSize>(msg);
        //Debug.Log("海浪大小===" + waveSize.value);
        OceanManager.Instance.SetWaveSize(waveSize.value/9f * 1.5f + 0.2f);
        Display5Panel.Instance.SetOceanText(waveSize.value.ToString());
    }

    private void SetOceanLightData(string msg)
    {
        OceanLightData oceanLight = new OceanLightData();
        oceanLight = JsonConvert.DeserializeObject<OceanLightData>(msg);
        OceanManager.Instance.SetOceanLight(oceanLight.value);
    }

    private void SetCameraState(string msg)
    {
        CameraState cameraState = new CameraState();
        cameraState = JsonConvert.DeserializeObject<CameraState>(msg);
        CameraSwitch cameraSwitch = (CameraSwitch)Enum.Parse(typeof(CameraSwitch), cameraState.state);
        switch (cameraSwitch)
        {
            case CameraSwitch.ThirdPerson:
                ThirdPersonOpen();
                break;
            case CameraSwitch.FirstPerson:
                FirstPersonOpen();
                break;
            case CameraSwitch.RearView:
                RearViewCameraOpen();
                break;
            default:
                break;
        }
    }

    private void TrainModelChange(string msg)
    {
        TrainModelData trainModelData = new TrainModelData();
        trainModelData = JsonConvert.DeserializeObject<TrainModelData>(msg);
        TrainModel model = (TrainModel)Enum.Parse(typeof(TrainModel), trainModelData.trainModel);
        switch (model)
        {
            case TrainModel.Transitions:

                //DataPanel.Instance.Tiletle.text = "转 场 训 练";
                //关闭吊装相机
                //隐藏吊装平台
                TransitionsModelReset();
                CurrentTrainModel = TrainModel.Transitions;
                break;
            case TrainModel.Laying:
                //DataPanel.Instance.Tiletle.text = "铺 管 训 练";
                //关闭吊装相机
                //隐藏吊装平台
                LayingModelReset();
                CurrentTrainModel = TrainModel.Laying;
                break;
            case TrainModel.Lifting:
                LiftingModelReset();
                CurrentTrainModel = TrainModel.Lifting;
                //DataPanel.Instance.Tiletle.text = "吊 装 训 练";
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 吊装训练初始化
    /// </summary>
    private void LiftingModelReset()
    {
        WeatherManager.Instance.SetWeather(WeatherMakerPrecipitationType.None, 0);
        Time_Day();
        OceanManager.Instance.SetWaveSize(0.3f);
        Display5Panel.Instance.Reset();
        autoDrive.Reset_ZhuanChuang();
        PipelineManager.instance.Stop();
        //FirstPersonOpen();
        Display6Rect.gameObject.SetActive(false);
        ResetCargos();
        DiaoZhuangPingTai.gameObject.SetActive(true);
        //打开吊装相机
        //DiaoZhuangCanvas.gameObject.SetActive(true);
        autoDrive.animationControl.DiaozhuangReset();
        autoDrive.animationControl.ropeControls[0].ScaleOpen();
        autoDrive.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        BoatCollider.enabled = false;
        DiaoZhuangCollider.SetActive(true);
        RearViewCameraOpen();
    }

    /// <summary>
    /// 转场训练初始化
    /// </summary>
    private void TransitionsModelReset()
    {
        ResetCargos();
        DiaoZhuangPingTai.gameObject.SetActive(false);
        autoDrive.animationControl.DiaozhuangReset();
        //DiaoZhuangCanvas.gameObject.SetActive(false);
        autoDrive.animationControl.ropeControls[0].ScaleClose();
        DiaoZhuangCollider.SetActive(false);
        BoatCollider.enabled = true;
        autoDrive.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if(CurrentTrainModel == TrainModel.Lifting)
        {
            FirstPersonOpen();
            Debug.Log("FirstPersonOpen");
        }
        
    }

    /// <summary>
    /// 铺管训练初始化
    /// </summary>
    private void LayingModelReset()
    {
        ResetCargos();
        DiaoZhuangPingTai.gameObject.SetActive(false);
        autoDrive.animationControl.DiaozhuangReset();
        //DiaoZhuangCanvas.gameObject.SetActive(false);
        autoDrive.animationControl.ropeControls[0].ScaleClose();
        DiaoZhuangCollider.SetActive(false);
        BoatCollider.enabled = true;
        autoDrive.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (CurrentTrainModel == TrainModel.Lifting)
        {
            FirstPersonOpen();
        }
    }

    private void ResetCargos()
    {
        foreach (Cargo item in Cargos)
        {
            item.Reset();
        }
    }

    private void SetPuGuanCameraState(string msg)
    {
        PuGuanCameraData data = new PuGuanCameraData();
        data = JsonConvert.DeserializeObject<PuGuanCameraData>(msg);
        PuGuanCameraState state = (PuGuanCameraState)Enum.Parse(typeof(PuGuanCameraState), data.state);
        switch (state)
        {
            case PuGuanCameraState.Open:
                Display6Rect.gameObject.SetActive(true);
                //Boat_PuGuan.SetActive(true);
                break;
            case PuGuanCameraState.Hide:
                Display6Rect.gameObject.SetActive(false);
                // Boat_PuGuan.SetActive(false);
                break;
            default:
                break;
        }
    }

    //private void SetTargetPosition(string msg)
    //{
    //TargetPosition targetPosition = new TargetPosition();
    //targetPosition = JsonConvert.DeserializeObject<TargetPosition>(msg);
    //Vector3 vector3 = new Vector3(targetPosition.x, 180.0f, targetPosition.z);
    //Target.transform.position = vector3;
    //autoDrive.IsStart = true;
    //}

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (IsNight)
            {
                Time_Day();
            }
            else
            {
                Time_Night();
            }
        }
#endif

        if (IsNight)
        {
            var euler = ZuanJingPingTaiLight[1].transform.rotation.eulerAngles;
            euler += new Vector3(0,25,0) * Time.deltaTime;
            ZuanJingPingTaiLight[1].transform.rotation = Quaternion.Euler(euler);
        }
    }

    public void ThirdPersonOpen()
    {
        foreach (Camera item in CameraGroup)
        {
            item.gameObject.SetActive(false);
        }
        weatherMakerFallingParticleScript3D.Height = 10;
        //FirstPersonTransform.gameObject.SetActive(true);
    }

    public void FirstPersonOpen()
    {
        foreach (Camera item in CameraGroup)
        {
            item.gameObject.SetActive(false);
        }
        ParticleMask.gameObject.SetActive(true);
        CameraGroup[0].gameObject.SetActive(true);
        CameraGroup[2].gameObject.SetActive(true);
        weatherMakerFallingParticleScript3D.Height = 20;
        //FirstPersonTransform.gameObject.SetActive(false);
    }

    private void RearViewCameraOpen()
    {
        //Display6Rect.gameObject.SetActive(false);
        foreach (Camera item in CameraGroup)
        {
            item.gameObject.SetActive(false);
        }
        ParticleMask.gameObject.SetActive(false);
        CameraGroup[1].gameObject.SetActive(true);
        CameraGroup[3].gameObject.SetActive(true);
        weatherMakerFallingParticleScript3D.Height = 20;
    }

    public void SetWaveScale(float value)
    {
        foreach (Transform item in WaveGroup)
        {
            item.DOKill();
        }

        Vector3 vector3 = new Vector3(value, value, value);

        foreach (Transform item in WaveGroup)
        {
            item.localScale = vector3;
        }
    }

    /// <summary>
    /// 波纹变化，默认变大
    /// </summary>
    /// <param name="waveScale"></param>
    /// <param name="value"></param>
    public void WaveChange(float value = 2.0f)
    {
        Vector3 vector3 = new Vector3(value, value, value);
        foreach (Transform item in WaveGroup)
        {
            item.DOKill();
        }

        foreach (Transform item in WaveGroup)
        {
            item.DOScale(vector3, Transition_Time);
        }
    }

    public void Time_Day()
    {
        PPV.SetActive(true);
        SceneLight.gameObject.transform.localEulerAngles = new Vector3(155.74f, -555.2f, 0);
        OceanManager.Instance.SetDayOceanMaterial();
        WeatherManager.Instance.SetDayColor();
        //WeatherLight.enabled = true;
        RenderSettings.skybox = Skybox[0];
        DayLightGroup.SetActive(true);
        Display5Panel.Instance.SetDayText("白天");
        if (WeatherMakerPrecipitationManagerScript.Instance.Precipitation == WeatherMakerPrecipitationType.None)
        {
            SetUnityFog(FogType.Day_Sunny);
        }
        else
        {
            SetUnityFog(FogType.Day_Cloudy);
        }
        ZuanJingPingTaiLight_Hide();
        autoDrive.BoatLightGroup_Open();
        //autoDrive.animationControl.Set_Day_Smoke();
        IsNight = false;
    }

    private void Time_Night()
    {
        PPV.SetActive(false);
        SceneLight.intensity = SceneLightIntensity_Night;
        SceneLight.gameObject.transform.localEulerAngles = new Vector3(155.74f, -392.37f, 0);
        WeatherManager.Instance.SetNightColor();
        DayLightGroup.SetActive(false);
        //WeatherLight.enabled = false;
        Display5Panel.Instance.SetDayText("黑夜");
        if (WeatherMakerPrecipitationManagerScript.Instance.Precipitation == WeatherMakerPrecipitationType.None)
        {
            SetUnityFog(FogType.Night_Sunny);
        }
        else
        {
            SetUnityFog(FogType.Night_Cloudy);
        }

        

        //OceanManager.Instance.SetOceanLight(0.12f);
        //autoDrive.animationControl.Set_Night_Smoke();
        ZuanJingPingTaiLight_Open();
        autoDrive.BoatLightGroup_Hide();
        IsNight = true;
    }

    private void ZuanJingPingTaiLight_Hide()
    {
        foreach (GameObject item in ZuanJingPingTaiLight)
        {
            item.SetActive(false);
        }
    }

    private void ZuanJingPingTaiLight_Open()
    {
        foreach (GameObject item in ZuanJingPingTaiLight)
        {
            item.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.BoatSpeed.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.DayNightTime.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.OceanLightData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.OceanWaveSize.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.WeatherIntensity.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.WeatherType.ToString(), Callback);
        //EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TargetPosition.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.CameraState.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TrainModelData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanCameraData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.AutoDriveData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.DriveTurnData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.DriveSpeed.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanSwitchData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TurnTableData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.CraneHandData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.HookData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.ControlSwitchData.ToString(), Callback2);
    }

    public void SetUnityFog(FogType fogType)
    {
        switch (fogType)
        {
            case FogType.Day_Sunny:
                //远方雾效果
                //RenderSettings.fogDensity = 0.00004f;
                RenderSettings.fogColor = new Color(88 / 255f, 122 / 255f, 162 / 255f, 255 / 255f);
                //海洋亮度
                OceanManager.Instance.SetOceanLight(0.85f);
                //天空盒亮度
                Skybox[0].SetFloat("_Exposure", 0.55f);
                //场景光
                SceneLight.intensity = SceneLightIntensity_day;
                SceneLight.color = LightColor_Sunyday;
                //船体光
                foreach (GameObject item in autoDrive.BoatLightGroup)
                {
                    item.GetComponent<Light>().intensity = 10000;
                    if(item.name == "Spot Light (3)")
                    {
                        item.GetComponent<Light>().intensity = 400;
                    }

                    if (item.name == "Spot Light (2)")
                    {
                        item.GetComponent<Light>().intensity = 5000;
                    }
                }
                DayLightGroup.SetActive(true);
                break;
            case FogType.Day_Cloudy:
                //RenderSettings.fogDensity = 0.00004f;
                RenderSettings.fogColor = new Color(106 / 255f, 124 / 255f, 140 / 255f, 255 / 255f);
                Skybox[0].SetFloat("_Exposure", 0.4f);
                OceanManager.Instance.SetOceanLight(0.58f);
                SceneLight.intensity = SceneLightIntensity_Rain;
                SceneLight.color = LightColor_RainDay;
                DayLightGroup.SetActive(false);
                foreach (GameObject item in autoDrive.BoatLightGroup)
                {
                    item.GetComponent<Light>().intensity = 1000;
                    if (item.name == "Spot Light (3)")
                    {
                        item.GetComponent<Light>().intensity = 100;
                    }

                    if (item.name == "Spot Light (2)")
                    {
                        item.GetComponent<Light>().intensity = 400;
                    }
                }
                break;
            case FogType.Night_Sunny:
                //RenderSettings.fogDensity = 0.0004f;
                RenderSettings.fogColor = new Color(6 / 255f, 13 / 255f, 20 / 255f, 255 / 255f);
                RenderSettings.skybox = Skybox[1];
                OceanManager.Instance.SetNightOceanMaterial();
                SceneLight.color = Color.white;
                break;
            case FogType.Night_Cloudy:
                RenderSettings.fogColor = new Color(6 / 255f, 13 / 255f, 20 / 255f, 255 / 255f);
                RenderSettings.skybox = Skybox[2];
                OceanManager.Instance.SetNightRainOceanMaterial();
                SceneLight.color = Color.white;
                break;
            default:
                break;
        }
    }


}
