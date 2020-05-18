using Crest;
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

public class SailingSceneManage : MonoBehaviour
{
    public static SailingSceneManage Instance;

    public Transform RawImageTransform;

    public GameObject[] Target;
    public GameObject PPV;

    //public Minimap minimap;

    public BoatProbes boatProbes;

    public Transform[] WaveGroup;

    public AutoDrive autoDrive;

    private float Transition_Time = 4.0f;

    public Light SceneLight;
    public Light WeatherLight;

    public Material[] Skybox;

    //铺管装置
    public GameObject Boat_PuGuan;

    public CameraFallow MainCameraFallow;

    //是否是晚上
    public bool IsNight;

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
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TargetPosition.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.CameraState.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TrainModelData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanCameraData.ToString(), Callback);
        //Time_Night();
        Time_Day();
    }

    private void Callback(EventParamete parameteData)
    {
        ParmaterCodes codes = (ParmaterCodes)Enum.Parse(typeof(ParmaterCodes), parameteData.EvendName);
        string msg = parameteData.GetParameter<string>()[0];
        switch (codes)
        {
            case ParmaterCodes.WeatherType:
                SetWeatherType(msg);
                break;
            case ParmaterCodes.WeatherIntensity:
                SetWeatherIntensity(msg);
                break;
            case ParmaterCodes.DayNightTime:
                SetDateTime(msg);
                break;
            case ParmaterCodes.BoatSpeed:
                SetBoatSpeed(msg);
                break;
            case ParmaterCodes.OceanWaveSize:
                SetOceanWaveSize(msg);
                break;
            case ParmaterCodes.OceanLightData:
                SetOceanLightData(msg);
                break;
            case ParmaterCodes.CameraState:
                SetCameraState(msg);
                break;
            case ParmaterCodes.TargetPosition:
                SetTargetPosition(msg);
                break;
            case ParmaterCodes.TrainModelData:
                TrainModelChange(msg);
                break;
            case ParmaterCodes.PuGuanCameraData:
                SetPuGuanCameraState(msg);
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
        OceanManager.Instance.SetWaveSize(waveSize.value/9 * 1.7f);
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
                CameraHide();
                break;
            case CameraSwitch.FirstPerson:
                CameraOpen();
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
                DataPanel.Instance.Tiletle.text = "转 场 训 练";
                break;
            case TrainModel.Laying:
                DataPanel.Instance.Tiletle.text = "铺 管 训 练";
                break;
            case TrainModel.Lifting:
                DataPanel.Instance.Tiletle.text = "吊 装 训 练";
                break;
            default:
                break;
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
                Boat_PuGuan.SetActive(true);
                UICanvas.Instance.ViewOpen();
                break;
            case PuGuanCameraState.Hide:
                Boat_PuGuan.SetActive(false);
                UICanvas.Instance.ViewHide();
                break;
            default:
                break;
        }
    }

    private void SetTargetPosition(string msg)
    {
        //TargetPosition targetPosition = new TargetPosition();
        //targetPosition = JsonConvert.DeserializeObject<TargetPosition>(msg);
        //Vector3 vector3 = new Vector3(targetPosition.x, 180.0f, targetPosition.z);
        //Target.transform.position = vector3;
        //autoDrive.IsStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if(IsNight)
            {
                Time_Day();
            }
            else
            {
                Time_Night();
            }
        }
    }

    public void CameraOpen()
    {
        RawImageTransform.gameObject.SetActive(true);
    }

    public void CameraHide()
    {
        RawImageTransform.gameObject.SetActive(false);
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
    public void WaveChange(float value = 1.0f)
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
        SceneLight.intensity = 1;
        //WeatherLight.enabled = true;
        RenderSettings.skybox = Skybox[0];

        if(WeatherManager.Instance.WeatherType == WeatherMakerPrecipitationType.None)
        {
            OceanManager.Instance.SetOceanLight(0.85f);
            Skybox[0].SetFloat("_Exposure", 0.55f);
        }
        else
        {
            Skybox[0].SetFloat("_Exposure", 0.4f);
            OceanManager.Instance.SetOceanLight(0.58f);
        }
    
        autoDrive.animationControl.Set_Day_Smoke();
        IsNight = false;
    }

    private void Time_Night()
    {
        PPV.SetActive(false);
        SceneLight.intensity = 0.01f;
        //WeatherLight.enabled = false;
        RenderSettings.skybox = Skybox[1];
        OceanManager.Instance.SetOceanLight(0.29f);
        autoDrive.animationControl.Set_Night_Smoke();
        IsNight = true;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.BoatSpeed.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.DayNightTime.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.OceanLightData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.OceanWaveSize.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.WeatherIntensity.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.WeatherType.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TargetPosition.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.CameraState.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.TrainModelData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanCameraData.ToString(), Callback);
    }
}
