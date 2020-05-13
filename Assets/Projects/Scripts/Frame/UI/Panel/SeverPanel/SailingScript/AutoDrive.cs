using Crest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDrive : MonoBehaviour
{
    /// <summary>
    /// 船
    /// </summary>
    private Transform Boat;

    /// <summary>
    /// 转向速度
    /// </summary>
    private float Turnto_speed = 0.5f;

    /// <summary>
    /// 航行速度
    /// </summary>
    public float SailingSpeed = 50.0f;

    /// <summary>
    /// 船和目的地最初的距离
    /// </summary>
    private float InitialDistance;

    /// <summary>
    /// 目标点
    /// </summary>
    private GameObject Target;

    /// <summary>
    /// 船航行控制脚本
    /// </summary>
    private BoatProbes boatProbes;

    /// <summary>
    /// 是否在转向
    /// </summary>
    private bool IsTurnTo;

    /// <summary>
    /// 是否开始航行
    /// </summary>
    public bool IsStart;

    /// <summary>
    /// 小地图上的船
    /// </summary>
    private RectTransform map_Boat;

    /// <summary>
    /// 船的当前位置
    /// </summary>
    private Vector3 Current_Destination = Vector3.zero;

    /// <summary>
    /// 小地图船的位置
    /// </summary>
    private Vector2 Current_Map_Boat;

    public BoatAnimationControl animationControl;


    // Start is called before the first frame update
    void Start()
    {
        Boat = this.transform;
        boatProbes = this.transform.GetComponent<BoatProbes>();
        Target = SailingSceneManage.Instance.Target;
        map_Boat = SailingSceneManage.Instance.minimap.map_Boat;
        SailingSceneManage.Instance.SetWaveScale(0.1f);
        //animationControl.EngineDown();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(Boat.position, Target.transform.position, Color.red);
        Debug.DrawRay(Boat.position, Boat.forward * 100.0f, Color.blue);
    }

    private void FixedUpdate()
    {
        if(IsStart)
        {
            Boat_Turnto(Target.gameObject.transform.position);
        }

    }

    private bool lerp, IsRotate;
    private float IntervalTime = 5;
    private float CurrentTime = 0;
    private float Distance = 10.0f;

    /// <summary>
    /// Turnto == 转向
    /// </summary>
    /// <param name="vector"></param>
    private void Boat_Turnto(Vector3 vector)
    {
        Vector3 dir;
        dir = vector - Boat.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        //Debug.Log("角度==" + Mathf.Abs(rot.eulerAngles.y - Boat.eulerAngles.y));
        //Debug.Log("rot=== " + rot.eulerAngles.y+" boat eulerAngle.y "+Boat.eulerAngles.y);

        //每隔5秒检查一次角度
        if(lerp && !IsTurnTo && boatProbes._enginePower!= 0)
        {
            CurrentTime += Time.deltaTime;
            if(CurrentTime > IntervalTime)
            {
                IsRotate = true;
                CurrentTime = 0;
            }
        }

        if (Mathf.Abs(rot.eulerAngles.y - Boat.eulerAngles.y) > 5.0f)
        {
            if (Destination_Change(vector)|| IsRotate)
            {
                IsTurnTo = true;

                //向右转
                if (Vector3.Cross(Boat.forward, (vector - Boat.position).normalized).y > 0)
                {
                    boatProbes._turnPower = Turnto_speed;
                }
                else if (Vector3.Cross(Boat.forward, (vector - Boat.position).normalized).y < 0)//向左转
                {
                    boatProbes._turnPower = -Turnto_speed;
                }
                else
                {
                    Debug.Log("Boat.forward, vector.normalized).y == 0");
                }
            }
        }
        else
        {
            //IsTurnTo用来限制里面代码只执行一次
            if (IsTurnTo)
            {
                boatProbes._turnPower = 0;
                IsTurnTo = false;
                lerp = true;
                IsRotate = false;
            }
        }

        if (lerp)
        {
            SailingSceneManage.Instance.WaveChange();

            if (Vector2.Distance(Current_Map_Boat, map_Boat.anchoredPosition) > Distance)
            {
                Current_Map_Boat = map_Boat.anchoredPosition;
                SailingSceneManage.Instance.minimap.Creat_RouteImage(map_Boat.anchoredPosition);
            }

            float dis = Vector3.Distance(Target.transform.position, Boat.transform.position);

            if (dis < 0.2* InitialDistance && IsRotate == false)
            {
                IsRotate = true;
            }
        }
    }

    /// <summary>
    /// 判断目的地是否已经改变
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private bool Destination_Change(Vector3 vector)
    {
        if (Vector3.Angle(Current_Destination, (vector - Current_Destination).normalized) < 1.5f && Current_Destination != Vector3.zero)
        {
            return false;
        }
        //else if(Vector3.Angle(Current_Destination, (vector - Current_Destination).normalized) < 1.5f && vector != Current_Destination)
        //{
        //    Current_Destination = vector;
        //    StartSailing();
        //    return true;
        //}

        if (vector != Current_Destination)
        {
            Current_Destination = vector;
            StartSailing();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ArriveTarget()
    {
        CurrentTime = 0;
        lerp = false;
        IsRotate = false;
        boatProbes._enginePower = 0;
        boatProbes._turnPower = 0;
        SailingSceneManage.Instance.WaveChange(0.1f);
        IsStart = false;
    }

    private void StartSailing()
    {
        boatProbes._enginePower = SailingSpeed;
        InitialDistance = Vector3.Distance(Target.transform.position, Boat.transform.position);
        lerp = false;
        SailingSceneManage.Instance.WaveChange(0.1f);
        Current_Map_Boat = map_Boat.anchoredPosition;
        IsRotate = false;
        CurrentTime = 0;
        SailingSceneManage.Instance.minimap.Creat_RouteImage(map_Boat.anchoredPosition);
    }
}
