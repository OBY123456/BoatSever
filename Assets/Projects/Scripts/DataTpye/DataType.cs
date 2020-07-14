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
        重播,
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
    /// 是否切换摄像机
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
        FirstPerson,
        ThirdPerson,
        RearView,
    }

    /// <summary>
    /// 昼夜状态
    /// </summary>
    public enum DayNightCycle
    {
        day,
        night,
    }

    /// <summary>
    /// 训练模式
    /// </summary>
    public enum TrainModel
    {
        /// <summary>
        /// 转场训练
        /// </summary>
        Transitions = 0,
        /// <summary>
        /// 铺管训练
        /// </summary>
        Laying = 1,
        /// <summary>
        /// 吊装训练
        /// </summary>
        Lifting = 2,
    }

    public class TrainModelData
    {
        public string trainModel;
    }

    /// <summary>
    /// 铺管视图状态
    /// </summary>
    public enum PuGuanCameraState
    {
        Open = 0,
        Hide = 1,
    }

    public class PuGuanCameraData
    {
        public string state;
    }
    public enum AutoDriveSwitch
    {
        Open = 0,
        Close = 1,
        Reset = 2,
    }

    public class AutoDriveData
    {
        public string state;
    }

    public enum DriveTurn
    {
        //左转
        TurnLeft = 0,
        //右转
        TurnRight = 1,
        //倒退
        TurnBack = 2,
        //完成
        Complete = 3,
    }

    public class DriveTurnData
    {
        public string state;
    }

    public class DriveSpeed
    {
        public int value;
    }

    //铺管训练
    public enum PuGuanSwitch
    {
        //开始
        Open = 0,
        //结束
        Close = 1,
        //重置
        //Reset = 2,
    }

    public class PuGuanSwitchData
    {
        public string state;
    }

    //转台，旋转范围0~90
    public class TurnTableData
    {
        public float value;
    }

    //吊臂，仰角范围0~45
    public class CraneHandData
    {
        public float value;
    }

    //吊勾
    public enum HookState
    {
        //下降
        Down,
        //上升
        Up,
        //停止
        Stop,
        //重置
        Reset,
        //放下
        PutDown,
    }

    public class HookData
    {
        public string state;
    }

    //大屏端控制权切换
    public enum ControlSwitch
    {
        Open = 0,
        Hide = 1,
    }

    public class ControlSwitchData
    {
        public string state;
    }

    /*模拟航行页——UI接口数据类型*/
    //Time
    public class TimeData
    {
        public int Hours;
        public int Minutes;
        public int Seconds;
    }

    //Temperature
    public class TemperatureData
    {
        public float value;
    }

    //Depth,范围0~5000
    public class DepthData
    {
        public float value;
    }

    //Rote of turn ,范围-30~30
    public class RoteOfTurnData
    {
        public float value;
    }

    //Log 
    public class LogData
    {
        //艏部横向速度
        public float value1;
        //尾部横向速度
        public float value2;
        //纵向速度
        public float value3;
    }

    //位置枚举类型
    public enum Direction
    {
        //主左
        Left,
        //主右
        Right,
        //左下UI面板
        LeftDown
    }

    //RPM,范围0~110,Azimuth Angle,范围-180~180,Pitch,范围0~110
    public class RAPData
    {
        //Direction转字符串赋值
        public string direction;
        public float RPMValue;
        public float AngleValue;
        public float PitchValue;
    }

    //Gyro,范围0~360
    public class GyroData
    {
        public float value;
    }

    //Roll,范围-20~20
    public class RollData
    {
        public float value;
    }

    //Rel·direction,范围-180~180
    public class RelDirectionData
    {
        public float value;
    }

    //Rel·speed
    public class RelSpeedData
    {
        public float value;
    }

    //推进器状态
    public class PropellerState
    {
        //推进器准备好框内文本
        public string msg1;
        //推进器运行框内文本
        public string msg2;
    }

    //发电机组数据
    public class GeneratorData
    {
        /// <summary>
        /// 发电机组在线框内文本
        /// </summary>
        public string msg;
        /// <summary>
        /// 发电机组当前功率数值
        /// </summary>
        public float Generatorvalue;
        /// <summary>
        /// 开关状态框内文本
        /// </summary>
        public string state;
        /// <summary>
        /// 配电板当前功率
        /// </summary>
        public float PeiDianBanValue;
    }

    //wind,范围-180~180
    public class WindData
    {
        public float value;
    }

    //true
    public class TrueData
    {
        //TRUE下第一个数据
        public float value1;
        //True下第二个数据
        public float value2;
    }

    //RELATIVE
    public class RelativeData
    {
        //Relative下第一个数据
        public float value1;
        //Relative下第二个数据
        public float value2;
    }


    //ROT等多项数据
    public class RotData
    {
        public float Rotvalue;
        public float Maonvalue;
        public float Gyrovalue;
        public float Cobvalue;
        public float Bogvalue;
        public float Pitchvalue;
    }

    /*船体数据*/
    /// <summary>
    /// 船体位置数据，以初始位置为（0,0）
    /// </summary>
    public class BoatPositionData
    {
        /// <summary>
        /// 水平横坐标,范围-4000~4000
        /// </summary>
        public float x;
        /// <summary>
        /// 水平纵坐标,范围-4000~4000
        /// </summary>
        public float z;
    }

    /// <summary>
    /// 船体角度数据
    /// </summary>
    public class BoatRotateData
    {
        //范围0~360
        public float value;
    }

    /// <summary>
    /// 船体航行速度,最大速度为20节
    /// </summary>
    public class BoatSpeedData
    {
        public float value;
    }
}











