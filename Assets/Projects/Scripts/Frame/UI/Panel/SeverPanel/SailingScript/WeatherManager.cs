using DigitalRuby.WeatherMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    private WeatherMakerPrecipitationManagerScript weatherMaker;
    private WeatherMakerWindScript windMaker;

    public WeatherMakerPrecipitationType WeatherType;

    /// <summary>
    /// 天气密度
    /// </summary>
    [Range(0, 1)]
    public float Intensity = 0.5f;

    [Range(0,1)]
    public float Wind_Intensity = 0;

    [Range(0,-360)]
    public float Wind_Rotate = -103.6f;

    //[Range(0,86399)]
    //public float TimeValue;

    //public string TimeOfDayText = "00:00:00";



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        weatherMaker = WeatherMakerPrecipitationManagerScript.Instance;
        windMaker = WeatherMakerWindScript.Instance;
        float duration = WeatherMakerPrecipitationManagerScript.Instance.PrecipitationChangeDuration;

        windMaker.SetWindProfileAnimated(WeatherMakerScript.Instance.LoadResource<WeatherMakerWindProfileScript>("WeatherMakerWindProfile_MediumWind"), 0.0f, duration);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    SetTime(TimeValue);
        //}

        //SetWeather(WeatherType, Intensity);
        //SetWindIntensity(Wind_Intensity);
        //SetWindRotate(Wind_Rotate);
    }

    public void SetWeather(WeatherMakerPrecipitationType type,float value)
    {
        switch (type)
        {
            case WeatherMakerPrecipitationType.None:
                weatherMaker.Precipitation = WeatherMakerPrecipitationType.None;
                break;
            case WeatherMakerPrecipitationType.Rain:
                weatherMaker.Precipitation = WeatherMakerPrecipitationType.Rain;
                SetPrecipitationIntensity(Intensity);
                break;
            case WeatherMakerPrecipitationType.Snow:
                weatherMaker.Precipitation = WeatherMakerPrecipitationType.Snow;
                SetPrecipitationIntensity(Intensity);
                break;
            default:
                break;
        }
    }

    public void SetPrecipitationIntensity(float value)
    {
        weatherMaker.PrecipitationIntensity = value;
    }

    public void SetWindIntensity(float value)
    {
        windMaker.WindZone.windMain = value * 3.0f;
        //windMaker.AudioSourceWind.Play(2.0f * value);
    }

    public void SetWindRotate(float value)
    {
        windMaker.gameObject.transform.localEulerAngles = Vector3.up * value;
    }

    //public void SetTime(float value)
    //{
    //    WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = value;
    //}

    //private void UpdateTimeOfDay()
    //{
    //    if (WeatherMakerDayNightCycleManagerScript.Instance != null)
    //    {
    //        float Time = WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay;
    //        if (RenderSettings.fog)
    //        {
    //            if (Time > 22400 && Time < 71172)
    //            {
    //                RenderSettings.fogColor = new Color(212 / 255f, 237 / 255f, 238 / 255f);
    //            }
    //            else if ((Time >= 71172 && Time < 80000) || (Time > 8000 && Time <= 22400))
    //            {
    //                RenderSettings.fogColor = new Color(59 / 255f, 72 / 255f, 73 / 255f);
    //            }
    //            //晚上
    //            else if (Time <= 8000 || Time >= 80000)
    //            {
    //                RenderSettings.fogColor = Color.black;
    //            }
    //        }

    //        if (TimeOfDayText!=null)
    //        {
    //            System.TimeSpan t = System.TimeSpan.FromSeconds(WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay);
    //            TimeOfDayText = string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
    //        }
    //        //TimeOfDayCategoryText.text = WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDayCategory.ToString();
    //    }
    //}
}
