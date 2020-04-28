﻿/* 传输的数据类型集合 */

namespace MTFrame
{
    /*全局按钮页*/

    public class SceneSwitch
    {
        public string SceneName;
    }

    /*船体展示页*/
    public class BoatRotate
    {
        public float X;
        public float Y;
        public float Z;
    }

    public enum VideoName
    {
        起吊系统 = 1,
        J型铺管 = 2,
        S型铺管 = 3,
        推进器系统 = 4,
        动力系统 = 5,
        结束 = 6,
    }

    public enum VideoControl
    {
        快进,
        快退,
        暂停,
        播放,
    }

    public class Display_PlayVideo
    {
        public string name;
    }

    public class Display_VideoControl
    {
        public string state;
    }

    /*模拟航行页*/
    public class WeatherType
    {
        /// <summary>
        /// 将天气枚举类型转化为字符串赋值
        /// </summary>
        public string weather;
        public float value;
    }

    /// <summary>
    /// 天气数值
    /// </summary>
    public class WeatherIntensity
    {
        public float value;
    }

    /// <summary>
    /// 时间
    /// </summary>
    public class DayNightTime
    {
        /// <summary>
        /// 使用DayNightCycle转字符串
        /// </summary>
        public string DayNight;
    }

    /// <summary>
    /// 船行驶速度
    /// </summary>
    public class BoatSpeed
    {
        public float speed;
    }

    /// <summary>
    /// 风浪大小
    /// </summary>
    public class OceanWaveSize
    {
        public float value;
    }

    /// <summary>
    /// 海面亮度
    /// </summary>
    public class OceanLightData
    {
        public float value;
    }

    /// <summary>
    /// 是否开启多画面模式
    /// </summary>
    public class CameraState
    {
        public string state;
    }

    /// <summary>
    /// 目的地坐标
    /// </summary>
    public class TargetPosition
    {
        public float x;
        public float z;
    }

    /// <summary>
    /// 摄像机状态
    /// </summary>
    public enum CameraSwitch
    {
        Open,
        Close,
    }

    /// <summary>
    /// 昼夜状态
    /// </summary>
    public enum DayNightCycle
    {
        day,
        night,
    }
}











