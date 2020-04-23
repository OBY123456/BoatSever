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

    public GameObject Target;
    public GameObject PPV;

    public Minimap minimap;

    public BoatProbes boatProbes;

    public Transform[] WaveGroup;

    public AutoDrive autoDrive;

    private float Transition_Time = 4.0f;

    public Light SceneLight;
    public Light WeatherLight;

    public Material[] Skybox;


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

        //Time_Night();
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
        Debug.Log("海浪大小===" + waveSize.value);
        OceanManager.Instance.SetWaveSize(waveSize.value);
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
            case CameraSwitch.Open:
                CameraOpen();
                break;
            case CameraSwitch.Close:
                CameraHide();
                break;
            default:
                break;
        }
    }

    private void SetTargetPosition(string msg)
    {
        TargetPosition targetPosition = new TargetPosition();
        targetPosition = JsonConvert.DeserializeObject<TargetPosition>(msg);
        Vector3 vector3 = new Vector3(targetPosition.x, 180.0f, targetPosition.z);
        Target.transform.position = vector3;
        autoDrive.IsStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void Time_Day()
    {
        PPV.SetActive(true);
        SceneLight.intensity = 1;
        WeatherLight.enabled = true;
        RenderSettings.skybox = Skybox[0];
        OceanManager.Instance.SetOceanLight(0.85f);
    }

    private void Time_Night()
    {
        PPV.SetActive(false);
        SceneLight.intensity = 0.05f;
        WeatherLight.enabled = false;
        RenderSettings.skybox = Skybox[1];
        OceanManager.Instance.SetOceanLight(0.044f);
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
    }
}
