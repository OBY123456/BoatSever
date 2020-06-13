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
    public GameObject Boat_PuGuan;

    //public CameraFallow[] MainCameraFallow;
    public RectTransform Display6Rect;

    public Transform ParticleMask;

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
        //EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TargetPosition.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.CameraState.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.TrainModelData.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.AutoDriveData.ToString(), Callback);
        //EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanCameraData.ToString(), Callback);
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
            //case ParmaterCodes.TargetPosition:
            //    SetTargetPosition(msg);
            //    break;
            case ParmaterCodes.TrainModelData:
                TrainModelChange(msg);
                break;
            //case ParmaterCodes.PuGuanCameraData:
            //    SetPuGuanCameraState(msg);
            //break;
            case ParmaterCodes.AutoDriveData:
                StartDrive(msg);
                break;
            default:
                break;
        }
    }

    private void StartDrive(string msg)
    {
        AutoDriveData autoDriveData = new AutoDriveData();
        autoDriveData = JsonConvert.DeserializeObject<AutoDriveData>(msg);
        AutoDriveEnum driveEnum = (AutoDriveEnum)Enum.Parse(typeof(AutoDriveEnum), autoDriveData.state);
        switch (driveEnum)
        {
            case AutoDriveEnum.Start:
                if(autoDrive.IsReset)
                {
                    autoDrive.StartAutoDrive();
                }
                break;
            case AutoDriveEnum.Wait:
                autoDrive.Reset_ZhuanChuang();
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
                break;
            case TrainModel.Laying:
                //DataPanel.Instance.Tiletle.text = "铺 管 训 练";
                break;
            case TrainModel.Lifting:
                //DataPanel.Instance.Tiletle.text = "吊 装 训 练";
                break;
            default:
                break;
        }
    }

    //private void SetPuGuanCameraState(string msg)
    //{
    //    PuGuanCameraData data = new PuGuanCameraData();
    //    data = JsonConvert.DeserializeObject<PuGuanCameraData>(msg);
    //    PuGuanCameraState state = (PuGuanCameraState)Enum.Parse(typeof(PuGuanCameraState), data.state);
    //    switch (state)
    //    {
    //        case PuGuanCameraState.Open:
    //            Boat_PuGuan.SetActive(true);
    //            UICanvas.Instance.ViewOpen();
    //            break;
    //        case PuGuanCameraState.Hide:
    //            Boat_PuGuan.SetActive(false);
    //            UICanvas.Instance.ViewHide();
    //            break;
    //        default:
    //            break;
    //    }
    //}

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
        Display6Rect.gameObject.SetActive(false);
        foreach (Camera item in CameraGroup)
        {
            item.gameObject.SetActive(false);
        }
        //FirstPersonTransform.gameObject.SetActive(true);
    }

    public void FirstPersonOpen()
    {
        Display6Rect.gameObject.SetActive(true);
        foreach (Camera item in CameraGroup)
        {
            item.gameObject.SetActive(false);
        }
        ParticleMask.gameObject.SetActive(true);
        CameraGroup[0].gameObject.SetActive(true);
        //FirstPersonTransform.gameObject.SetActive(false);
    }

    private void RearViewCameraOpen()
    {
        foreach (Camera item in CameraGroup)
        {
            item.gameObject.SetActive(false);
        }
        ParticleMask.gameObject.SetActive(false);
        CameraGroup[1].gameObject.SetActive(true);
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
        if(WeatherMakerPrecipitationManagerScript.Instance.Precipitation == WeatherMakerPrecipitationType.None)
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
        //EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.PuGuanCameraData.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.AutoDriveData.ToString(), Callback);
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
