using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    /// 转盘,范围0~360
    /// </summary>
    public Transform TurnTable;

    /// <summary>
    /// 吊臂，仰角范围0~30
    /// </summary>
    public Transform CraneHand;

    /// <summary>
    /// 吊钩
    /// </summary>
    public Transform Hook;

    [HideInInspector]
    public bool IsRotate;
    private float RotateSpeed_Propeller = 50;

    [Range(0,360)]
    public float RotateSpeed_Engine = 0;


    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

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
    }

    public void DiaozhuangReset()
    {
        TurnTable.localEulerAngles = CraneHand.localEulerAngles = Vector3.zero;
    }

    //public void EngineUp()
    //{
    //    Engine_Liftinglever.transform.DOLocalMoveZ(0f, 3.0f);
    //}

    //public void EngineDown()
    //{
    //    Engine_Liftinglever.transform.DOLocalMoveZ(-50.2f, 3.0f);
    //}

    //public void EngineRotate(float value)
    //{
    //    Engine_Shaft.transform.DOLocalRotate(value*Vector3.forward, 3.0f);
    //}

    //private void PropellerRotate()
    //{
    //    Engine_Propeller.transform.Rotate(Vector3.right * RotateSpeed_Propeller);
    //}

    //public void Set_Day_Smoke()
    //{
    //    // SmokeParticle.GetComponent<Renderer>().material = SmokeMaterials[0];
    //    SmokeParticle.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(191 / 255f, 191 / 255f, 191 / 255f, 10 / 255f));
    //}

    //public void Set_Night_Smoke()
    //{
    //    //SmokeParticle.GetComponent<Renderer>().material = SmokeMaterials[1];
    //    SmokeParticle.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(50 / 255f, 50 / 255f, 50 / 255f, 10 / 255f));
    //}
}
