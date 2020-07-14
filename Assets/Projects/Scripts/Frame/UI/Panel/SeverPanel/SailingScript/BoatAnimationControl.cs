using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MTFrame;

public class BoatAnimationControl : MonoBehaviour
{
    //升降杆
    //public GameObject Engine_Liftinglever;

    /// <summary>
    /// 旋转轴
    /// </summary>
    public Transform[] Engine_Shaft;

    /// <summary>
    /// 螺旋桨
    /// </summary>
    public Transform[] Engine_Propeller;

    /// <summary>
    /// 转盘,范围0~90
    /// </summary>
    public Transform TurnTable;

    /// <summary>
    /// 吊臂，仰角范围0~45
    /// </summary>
    public Transform CraneHand;

    [HideInInspector]
    public bool IsRotate;

    /// <summary>
    /// 范围30~70
    /// </summary>
    private float RotateSpeed_Propeller = 50;

    //[Range(0,360)]
    //public float RotateSpeed_Engine = 0;

    public RopeControl[] ropeControls;

    [Header("上方绳子")]
    public Transform TopRopeBind;//静止状态时绑定的坐标
    public Transform TopRopeUpdate;//运行时，绳子需要更新到的坐标

    // Start is called before the first frame update
    void Start()
    {
        if (TopRopeBind && TopRopeUpdate)
        TopRopeBind.transform.position = TopRopeUpdate.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //EngineRotate(RotateSpeed_Engine);

        if(IsRotate)
        {
            PropellerRotate();
        }
    }

    private void PropellerRotate()
    {
        Engine_Propeller[0].Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[1].Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[2].Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[3].Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[4].Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[5].Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[6].Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[7].Rotate(Vector3.right * RotateSpeed_Propeller);
    }

    /// <summary>
    /// 控制螺旋桨方向函数
    /// </summary>
    /// <param name="value">目标角度</param>
    /// <param name="time">所需时间</param>
    public void ShaftRotate(float value,float time)
    {
        
        Vector3 vector = Vector3.forward * value;
        //Debug.Log("vector==" + vector);
        foreach (Transform item in Engine_Shaft)
        {
            item.DOKill();
        }

        Engine_Shaft[0].DOLocalRotate(vector, time).SetEase(Ease.Linear);
        Engine_Shaft[1].DOLocalRotate(vector, time).SetEase(Ease.Linear);
    }

    public void ResetShaft()
    {
        foreach (Transform item in Engine_Shaft)
        {
            item.DOKill();
            item.localEulerAngles = Vector3.zero;
        }
    }

    public void TurnTableRotate(float value)
    {
        TurnTable.localEulerAngles = new Vector3(0, 0, -value);
    }

    public void CraneHandRotate(float value)
    {
        CraneHand.localEulerAngles = new Vector3(0, value, 0);
        foreach (RopeControl item in ropeControls)
        {
            item.Rope_Node.localEulerAngles = new Vector3(0, -value, 0);
        }
    }

    public void DiaozhuangReset()
    {
        TurnTableRotate(0);
        CraneHandRotate(0);
        ropeControls[0].Reset();
        ropeControls[1].Rope_Node.localEulerAngles = Vector3.zero;
    }

    public void Hookstate(HookState state)
    {
        switch (state)
        {
            case HookState.Down:
                ropeControls[0].IsShorten = false;
                ropeControls[0].IsElongate = true;            
                break;
            case HookState.Up:
                ropeControls[0].IsElongate = false;
                ropeControls[0].IsShorten = true; 
                break;
            case HookState.Stop:
                ropeControls[0].IsElongate = false;
                ropeControls[0].IsShorten = false;
                break;
            case HookState.Reset:
                DiaozhuangReset();
                break;
            case HookState.PutDown:
                ropeControls[0].IsGrab = false;
                break;
            default:
                break;
        }
        
    }

    //public void Set_Night_Smoke()
    //{
    //    //SmokeParticle.GetComponent<Renderer>().material = SmokeMaterials[1];
    //    SmokeParticle.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(50 / 255f, 50 / 255f, 50 / 255f, 10 / 255f));
    //}
}
