using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 只会接收和发送对应枚举的数据
/// </summary>
public enum OperateCodes : byte
{
    Game
}

/// <summary>
/// 只会接收和发送对应枚举的数据
/// </summary>
public enum ParmaterCodes : byte
{
    /// <summary>
    /// 字符串类型
    /// </summary>
    index,

    /// <summary>
    /// 切换场景
    /// </summary>
    SceneSwitch,

    /*船体展示页传输数据类型枚举*/
    /// <summary>
    /// 船体展示页1
    /// </summary>
    BoatRotate,
    /// <summary>
    /// 船体展示页2
    /// </summary>
    Display_PlayVideo,
    /// <summary>
    /// 船体展示页3
    /// </summary>
    Display_VideoControl,

    /*模拟航行页传输数据类型枚举*/
    /// <summary>
    /// 模拟航行页1
    /// </summary>
    WeatherType,
    /// <summary>
    /// 模拟航行页2
    /// </summary>
    WeatherIntensity,
    /// <summary>
    /// 模拟航行页3
    /// </summary>
    DayNightTime,
    /// <summary>
    /// 模拟航行页4
    /// </summary>
    BoatSpeed,
    /// <summary>
    /// 模拟航行页5
    /// </summary>
    OceanWaveSize,
    /// <summary>
    /// 模拟航行页6
    /// </summary>
    OceanLightData,
    /// <summary>
    /// 模拟航行页7
    /// </summary>
    CameraState,
    /// <summary>
    /// 模拟航行页8
    /// </summary>
    TargetPosition,
    /// <summary>
    /// 模拟航行页9
    /// </summary>
    TrainModelData,
    /// <summary>
    /// 模拟航行页10
    /// </summary>
    PuGuanCameraData,
    /// <summary>
    /// 测试用数据11
    /// </summary>
    AutoDriveData,
    /// <summary>
    /// 模拟航行页12
    /// </summary>
    DriveTurnData,
    /// <summary>
    /// 模拟航行页13
    /// </summary>
    DriveSpeed,
    /// <summary>
    /// 模拟航行页14
    /// </summary>
    PuGuanSwitchData,
    /// <summary>
    /// 模拟航行页15
    /// </summary>
    TurnTableData,
    /// <summary>
    /// 模拟航行页16
    /// </summary>
    CraneHandData,
    /// <summary>
    /// 模拟航行页17
    /// </summary>
    HookData,
    /// <summary>
    /// 模拟航行页18
    /// </summary>
    ControlSwitchData,
    /// <summary>
    /// UI接口数据类型1
    /// </summary>
    TimeData,
    /// <summary>
    /// UI接口数据类型2
    /// </summary>
    TemperatureData,
    /// <summary>
    /// UI接口数据类型3
    /// </summary>
    DepthData,
    /// <summary>
    /// UI接口数据类型4
    /// </summary>
    RoteOfTurnData,
    /// <summary>
    /// UI接口数据类型5
    /// </summary>
    LogData,
    /// <summary>
    /// UI接口数据类型6
    /// </summary>
    RAPData,
    /// <summary>
    /// UI接口数据类型7
    /// </summary>
    GyroData,
    /// <summary>
    /// UI接口数据类型8
    /// </summary>
    RollData,
    /// <summary>
    /// UI接口数据类型9
    /// </summary>
    RelDirectionData,
    /// <summary>
    /// UI接口数据类型10
    /// </summary>
    RelSpeedData,
    /// <summary>
    /// UI接口数据类型11
    /// </summary>
    PropellerState,
    /// <summary>
    /// UI接口数据类型12
    /// </summary>
    GeneratorData,
    /// <summary>
    /// UI接口数据类型13
    /// </summary>
    WindData,
    /// <summary>
    /// UI接口数据类型14
    /// </summary>
    TrueData,
    /// <summary>
    /// UI接口数据类型15
    /// </summary>
    RelativeData,
    /// <summary>
    /// UI接口数据类型16
    /// </summary>
    RotData,

}
