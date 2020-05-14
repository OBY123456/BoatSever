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

    /// <summary>
    /// 小地图上的船
    /// </summary>
    //private RectTransform map_Boat;

    /// <summary>
    /// 船的当前位置
    /// </summary>
    //private Vector3 Current_Destination = Vector3.zero;

    /// <summary>
    /// 小地图船的位置
    /// </summary>
    //private Vector2 Current_Map_Boat;

    public BoatAnimationControl animationControl;

    // Start is called before the first frame update
    void Start()
    {
        Boat = this.transform;
        boatProbes = this.transform.GetComponent<BoatProbes>();
        //Target = SailingSceneManage.Instance.Target[1];
        //map_Boat = SailingSceneManage.Instance.minimap.map_Boat;
        SailingSceneManage.Instance.SetWaveScale(0.1f);
        //animationControl.EngineDown();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(IsAutoDrive)
            {
                IsAutoDrive = false;
            }
            else
            {
                StartAutoDrive();
            }
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            Reset_ZhuanChuang();
        }

        //Debug.DrawLine(Boat.position, Target.transform.position, Color.red);
        //Debug.DrawRay(Boat.position, Boat.forward * 100.0f, Color.blue);
    }

    private void FixedUpdate()
    {
        if(IsAutoDrive)
        {
            Boat_Turnto(Target.gameObject.transform.position);
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
            else
            {
                boatProbes._turnPower = 0;
                IsTurnTo = false;
                lerp = true;
            }
        }

        if (lerp)
        {
           
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
        float dis3 = Vector3.Distance(Boat.position, SailingSceneManage.Instance.Target[0].transform.position);

        if(dis3 < dis1 && dis3 < dis2)
        {
            Target = SailingSceneManage.Instance.Target[0];
            return;
        }

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

    private void StartAutoDrive()
    {
        if(!IsArrive)
        {
            DestinationChange();
            IsTurnTo = true;
            IsAutoDrive = true;
            StartSailing();
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
        IsAutoDrive = false;
        IsArrive = true;
    }

    /// <summary>
    /// 到达设置的点后调整角度和位置
    /// </summary>
    /// <param name="target">目标点</param>
    /// <param name="Rota">角度的正负值</param>
    public void SetBoatTransform(Transform target,float i)
    {
        boatProbes._enginePower = 0;
        boatProbes._turnPower = 0;
        SailingSceneManage.Instance.SetWaveScale(0.1f);

        float temp = Mathf.Abs( Boat.localEulerAngles.y - 360 - (-90));
        //Debug.Log("temp==" + temp);
        float time = temp / 8;

        Boat.DORotate(new Vector3( Boat.localEulerAngles.x, i * 90.0f, Boat.localEulerAngles.z), time);
        Boat.DOMove(new Vector3( target.position.x,Boat.position.y,target.position.z), time).OnComplete(()=> {
            Target = SailingSceneManage.Instance.Target[0];
            SailingSceneManage.Instance.SetWaveScale(0.3f);
            IsTurnTo = true;
            IsAutoDrive = true;
            StartSailing();
        });
    }

    public void MainCameraRotate()
    {
        if(IsRight)
        {
            float temp = SailingSceneManage.Instance.MainCameraFallow.offset.x;
            DG.Tweening.DOTween.To(() => SailingSceneManage.Instance.MainCameraFallow.offset.x, x => SailingSceneManage.Instance.MainCameraFallow.offset.x = x, 105.63f, 5.0f);
        }
    }

    private void StartSailing()
    {

        SailingSceneManage.Instance.WaveChange();

        boatProbes._enginePower = SailingSpeed;
        InitialDistance = Vector3.Distance(Target.transform.position, Boat.position);
        lerp = false;
        
        //Current_Map_Boat = map_Boat.anchoredPosition;
        IsRotate = true;
        CurrentTime = 0;
        //SailingSceneManage.Instance.minimap.Creat_RouteImage(map_Boat.anchoredPosition);
    }

    //重置转场训练
    private void Reset_ZhuanChuang()
    {
        boatProbes._enginePower = 0;
        boatProbes._turnPower = 0;
        IsRotate = false;
        lerp = false;
        IsRight = false;
        IsTurnTo = false;
        IsAutoDrive = false;
        IsArrive = false;
        CurrentTime = 0;
        SailingSceneManage.Instance.MainCameraFallow.offset.x = -105.63f;
        Boat.position = new Vector3(0, 180, 0);
        Boat.eulerAngles = new Vector3(0, -90, 0);
        Target = null;
        InitialDistance = 0;
        SailingSceneManage.Instance.SetWaveScale(0.1f);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
