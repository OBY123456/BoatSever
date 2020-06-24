using Crest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class AutoDrive : MonoBehaviour
{
    /// <summary>
    /// 船
    /// </summary>
    private Transform Boat;

    /// <summary>
    /// 转向速度
    /// </summary>
    private float Turnto_speed =1.0f;

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
    /// 是否开启转向
    /// </summary>
    private bool IsTurnTo;

    /// <summary>
    /// 是否开启定时检测转向
    /// </summary>
    private bool IsRotate;

    public bool IsAutoDrive;

    public bool IsArrive;

    /// <summary>
    /// 是否从右边进入钻井平台
    /// </summary>
    public bool IsRight;

    ///// <summary>
    ///// 小地图上的船
    ///// </summary>
    //private RectTransform map_Boat;

    ///// <summary>
    ///// 船的当前位置
    ///// </summary>
    //private Vector3 Current_Destination = Vector3.zero;

    ///// <summary>
    ///// 小地图船的位置
    ///// </summary>
    //private Vector2 Current_Map_Boat;

    public BoatAnimationControl animationControl;

    public GameObject[] BoatLightGroup;

    public bool IsReset;

    // Start is called before the first frame update
    void Start()
    {
        Boat = this.transform;
        boatProbes = this.transform.GetComponent<BoatProbes>();
        //Target = SailingSceneManage.Instance.Target[1];
        //map_Boat = SailingSceneManage.Instance.minimap.map_Boat;
        SailingSceneManage.Instance.SetWaveScale(0.01f);
        //animationControl.EngineDown();     
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (IsAutoDrive)
            {
                IsAutoDrive = false;
            }
            else
            {
                StartAutoDrive();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Reset_ZhuanChuang();
        }
#endif
        //Debug.DrawLine(Boat.position, Target.transform.position, Color.red);
        //Debug.DrawRay(Boat.position, Boat.forward * 100.0f, Color.blue);
    }

    private void FixedUpdate()
    {
        if(IsAutoDrive)
        {
            Boat_Turnto(Target.gameObject.transform.position);
        }

        if (!IsComplete)
        {
            BoatStraighten(target1);
        }
    }

    private bool lerp ;
    private float IntervalTime = 5;
    private float CurrentTime = 0;
    //private float Distance = 10.0f;

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
        if(lerp && IsRotate && boatProbes._enginePower!= 0)
        {
            CurrentTime += Time.deltaTime;
            if(CurrentTime > IntervalTime)
            {
                IsTurnTo = true;
                CurrentTime = 0;
                Turnto_speed = 0.1f;
            }
        }

        if(IsTurnTo)
        {
            if (Mathf.Abs(rot.eulerAngles.y - Boat.eulerAngles.y) > 5.0f)
            {
                //向右转
                if (Vector3.Cross(Boat.forward, (vector - Boat.position).normalized).y > 0)
                {
                    boatProbes._turnPower = Turnto_speed;
                    if(!IsRotate)
                    {
                        //animationControl.ShaftRotate
                        //animationControl.Engine_Shaft[0].localEulerAngles = Vector3.Lerp(animationControl.Engine_Shaft[0].localEulerAngles, Vector3.back * 45, 3.0f);
                        //animationControl.Engine_Shaft[1].localEulerAngles = Vector3.Lerp(animationControl.Engine_Shaft[1].localEulerAngles, Vector3.back * 45, 3.0f);
                        animationControl.ShaftRotate(-45f, 3.0f);
                        //Debug.Log("111");
                    }
                }
                else if (Vector3.Cross(Boat.forward, (vector - Boat.position).normalized).y < 0)//向左转
                {
                    boatProbes._turnPower = -Turnto_speed;
                    if(!IsRotate)
                    {
                        animationControl.ShaftRotate(45f, 3.0f);
                        //animationControl.Engine_Shaft[0].localEulerAngles = Vector3.Lerp(animationControl.Engine_Shaft[0].localEulerAngles, Vector3.forward * 45, Time.deltaTime);
                        //animationControl.Engine_Shaft[1].localEulerAngles = Vector3.Lerp(animationControl.Engine_Shaft[1].localEulerAngles, Vector3.forward * 45, Time.deltaTime);
                        //Debug.Log("111");
                    }
                }
                else
                {
                    Debug.Log("Boat.forward, vector.normalized).y == 0");
                }
            }
            else
            {
                boatProbes._turnPower = 0;
                IsTurnTo = false;
                lerp = true;
                IsRotate = true;
                animationControl.ShaftRotate(0, 3.0f);
            }
        }

        if (lerp)
        {

            //animationControl.Engine_Shaft[0].localEulerAngles = Vector3.Lerp(animationControl.Engine_Shaft[0].localEulerAngles, Vector3.zero, 3.0f );
            //animationControl.Engine_Shaft[1].localEulerAngles = Vector3.Lerp(animationControl.Engine_Shaft[1].localEulerAngles, Vector3.zero, 3.0f);
            
            //if (Vector2.Distance(Current_Map_Boat, map_Boat.anchoredPosition) > Distance)
            //{
            //    Current_Map_Boat = map_Boat.anchoredPosition;
            //    SailingSceneManage.Instance.minimap.Creat_RouteImage(map_Boat.anchoredPosition);
            //}

            float dis = Vector3.Distance(Target.transform.position, Boat.position);

            if (dis < 0.2 * InitialDistance && IsTurnTo == false)
            {
                IsTurnTo = true;
            }
        }
    }

    private void DestinationChange()
    {
        float dis1 = Vector3.Distance(Boat.position, SailingSceneManage.Instance.Target[1].transform.position);
        float dis2 = Vector3.Distance(Boat.position, SailingSceneManage.Instance.Target[2].transform.position);
        //float dis3 = Vector3.Distance(Boat.position, SailingSceneManage.Instance.Target[0].transform.position);

        //if(dis3 < dis1 && dis3 < dis2)
        //{
        //    Target = SailingSceneManage.Instance.Target[0];
        //    return;
        //}

        if (dis1 <= dis2)
        {
            Target = SailingSceneManage.Instance.Target[1];
            IsRight = true;
        }
        else
        {
            Target = SailingSceneManage.Instance.Target[2];
        }
    }

    /// <summary>
    /// 开始自动驾驶
    /// </summary>
    public void StartAutoDrive()
    {
        if(!IsArrive)
        {
            DestinationChange();
            IsTurnTo = true;
            IsAutoDrive = true;
            SailingSceneManage.Instance.WaveChange();
            StartSailing();
            IsReset = false;
            animationControl.IsRotate = true;
        }
    }

    /// <summary>
    /// 关闭自动驾驶，开始手动驾驶
    /// </summary>
    public void CloseAutoDrive()
    {
        IsTurnTo = false;
        IsAutoDrive = false;
        SailingSceneManage.Instance.SetWaveScale(0.01f);
        IsReset = true;
        animationControl.IsRotate = false;
        animationControl.ResetShaft();
        boatProbes._enginePower = 0;
        boatProbes._turnPower = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Boat.DOKill();
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, MainCameraRotate);
        IsComplete = true;
        IsRotateComplete = false;
        CurrentTime = 0;
        lerp = false;

        IsRotate = false;

        IsRight = false;

        MainCameraReset();
        InitialDistance = 0;
    }

    public void ArriveTarget()
    {
        CurrentTime = 0;
        lerp = false;
        IsRotate = false;
        IsTurnTo = false;
        boatProbes._enginePower = 0;
        boatProbes._turnPower = 0;
        SailingSceneManage.Instance.WaveChange(0.01f);
        //IsAutoDrive = false;
        IsArrive = true;
        IsAutoDrive = false;


    }

    bool IsComplete = true;
    bool IsRotateComplete = false;
    Transform target1;
    /// <summary>
    /// 到达设置的点后调整角度和位置
    /// </summary>
    /// <param name="target">目标点</param>
    /// <param name="i">角度的正负值</param>
    public void SetBoatTransform(Transform target,float i)
    {
        float temp = Mathf.Abs(Boat.localEulerAngles.y - 360 - (-90));
        float time = temp / 8;

        if(time < 1)
        {
            TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 3.0f, MainCameraRotate);
            SailingSceneManage.Instance.WaveChange(0.8f);
        }
        else
        {
            boatProbes._enginePower = 0;
            boatProbes._turnPower = 0;
            //SailingSceneManage.Instance.SetWaveScale(0.01f);
            target1 = target;
            IsComplete = false;
            animationControl.ShaftRotate(-45f, time);
            Boat.DORotate(new Vector3(Boat.localEulerAngles.x, i * 90.0f, Boat.localEulerAngles.z), time).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    animationControl.ShaftRotate(0, 3.0f);
                    IsRotateComplete = true;
                });


        }
        Target = SailingSceneManage.Instance.Target[0];
    }


    private void BoatStraighten(Transform target)
    {
        Rigidbody _rb = GetComponent<Rigidbody>();
        float distance = Vector3.Distance(target.position, Boat.position);
        //Debug.Log("Distance==" + distance);
        if (distance > 5)
        {
            Vector3 vector3 = (target.position - Boat.position).normalized;
            _rb.AddForceAtPosition(vector3 * 15.0f, _rb.position, ForceMode.Acceleration);          
        }
        else
        {
            _rb.velocity = Vector3.zero;
            if (IsRotateComplete)
            {
                IsComplete = true;
                SailingSceneManage.Instance.WaveChange(0.8f);
                IsTurnTo = true;
                IsAutoDrive = true;
                TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 3.0f, MainCameraRotate);
                StartSailing();
                Boat.DOKill();
                //Debug.Log("Complete!");
            }
        }
    }

    public void MainCameraRotate()
    {
        if(IsRight)
        {
            if(SailingSceneManage.Instance.ThirdPersonCamera[0])
            {
                DG.Tweening.DOTween.To(() => SailingSceneManage.Instance.ThirdPersonCamera[0].transform.GetComponent<CameraFallow>().offset.x,
                                x => SailingSceneManage.Instance.ThirdPersonCamera[0].transform.GetComponent<CameraFallow>().offset.x = x, 105.63f, 5.0f);
            }


            if (SailingSceneManage.Instance.ThirdPersonCamera[1])
            {
                DG.Tweening.DOTween.To(() => SailingSceneManage.Instance.ThirdPersonCamera[1].transform.GetComponent<CameraFallow>().offset.x,
                                x => SailingSceneManage.Instance.ThirdPersonCamera[1].transform.GetComponent<CameraFallow>().offset.x = x, 105.63f, 5.0f);
            }                
        }
    }

    private void MainCameraReset()
    {
        foreach (Camera item in SailingSceneManage.Instance.ThirdPersonCamera)
        {
            item.transform.GetComponent<CameraFallow>().offset.x = -105.63f;
        }
    }

    private void StartSailing()
    {

        //SailingSceneManage.Instance.WaveChange();

        boatProbes._enginePower = SailingSpeed;
        boatProbes._turnPower = 1.0f;
        InitialDistance = Vector3.Distance(Target.transform.position, Boat.position);
        lerp = false;
        
        //Current_Map_Boat = map_Boat.anchoredPosition;
        //IsRotate = true;
        CurrentTime = 0;
        //SailingSceneManage.Instance.minimap.Creat_RouteImage(map_Boat.anchoredPosition);
    }

    //重置转场训练
    public void Reset_ZhuanChuang()
    {
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, MainCameraRotate);
        boatProbes._enginePower = 0;
        boatProbes._turnPower = 0;
        IsRotate = false;
        lerp = false;
        IsRight = false;
        IsTurnTo = false;
        IsAutoDrive = false;
        IsArrive = false;
        CurrentTime = 0;
        MainCameraReset();
        Boat.position = new Vector3(0, 180, 0);
        Boat.eulerAngles = new Vector3(0, -90, 0);
        Target = null;
        InitialDistance = 0;
        SailingSceneManage.Instance.SetWaveScale(0.01f);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Turnto_speed = 1.0f;
        IsComplete = true;
        IsRotateComplete = false;
        Boat.DOKill();
        animationControl.IsRotate = false;
        animationControl.ResetShaft();
        IsReset = true;
        //OceanManager.Instance.ResetOcean();
        //WeatherManager.Instance.ResetWeather();
        //SailingSceneManage.Instance.Time_Day();
    }

    private void OnTriggerStay  (Collider other)
    {
        if(other.tag == "Obstacle" && IsAutoDrive /*&& !IsEnter*/)
        {
            Transform obs = other.transform;
            //当障碍物在船的前方时
            //Debug.Log("前方==" + Vector3.Dot(Boat.forward, (obs.position - Boat.position)));
            if(Vector3.Dot(Boat.forward,(obs.position - Boat.position)) >= 0)
            {
                //Debug.Log("左右=="+Vector3.Cross(Boat.forward, (obs.position - Boat.position).normalized).y);
                lerp = IsRotate = IsTurnTo = false;
                CurrentTime = 0;
                if (Vector3.Cross(Boat.forward, (obs.position - Boat.position).normalized).y >= 0)
                {
                    boatProbes._turnPower = -1;
                    animationControl.ShaftRotate(45f, 3.0f);
                }
                else
                {
                    boatProbes._turnPower = 1;
                    animationControl.ShaftRotate(-45f, 3.0f);

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Obstacle" && IsAutoDrive)
        {
            IsTurnTo =  true;
            lerp = false;
            Turnto_speed = 1.0f;
        }
    }

    public void BoatLightGroup_Open()
    {
        foreach (GameObject item in BoatLightGroup)
        {
            item.SetActive(true);
        }
    }

    public void BoatLightGroup_Hide()
    {
        foreach (GameObject item in BoatLightGroup)
        {
            item.SetActive(false);
        }
    }
}
