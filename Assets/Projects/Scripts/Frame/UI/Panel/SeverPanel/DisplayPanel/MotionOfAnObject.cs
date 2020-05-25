//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[ExecuteInEditMode]
//public class CameraRotate : MonoBehaviour
//{
//    public Transform Target;

//    [Range(0,360)]
//    public float RotateY = 0;

//    private float MinY = 0;
//    private float MaxY = 360;

//    private float PositionOffect;
//    private float PositionMaxZ = -88.7f;
//    private float PositionMinZ = -118.7f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        PositionOffect = PositionMaxZ - PositionMinZ;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        SetTargetRotate(RotateY);
//    }

//    public void SetTargetRotate(float value)
//    {
//        Target.localEulerAngles = -Vector3.up * value;
//        if(value >= MinY && value < MaxY * 0.25f)
//        {
//            transform.position = new Vector3(transform.position.x,transform.position.y, PositionMaxZ - (value / (MaxY * 0.25f) * PositionOffect)) ;
//        }
//        else if(value >= MaxY * 0.25f && value < MaxY * 0.5f)
//        {
//            transform.position = new Vector3(transform.position.x, transform.position.y, PositionMinZ + ((value - MaxY * 0.25f) / (MaxY * 0.25f) * PositionOffect));
//        }
//        else if(value >= MaxY * 0.5f && value < MaxY * 0.75f)
//        {
//            transform.position = new Vector3(transform.position.x, transform.position.y, PositionMaxZ - ((value - MaxY * 0.5f) / (MaxY * 0.25f) * PositionOffect));
//        }
//        else if(value >= MaxY * 0.75f && value <= MaxY)
//        {
//            transform.position = new Vector3(transform.position.x, transform.position.y, PositionMinZ + ((value - MaxY * 0.75f) / (MaxY * 0.25f) * PositionOffect));
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
///                     圆的参数方程 x=a+r cosθ y=b+r sinθ（θ∈ [0，2π) ） (a,b) 为圆心坐标，r 为圆半径，θ 为参数，(x,y) 为经过点的坐标
///                     椭圆的参数方程 x=a cosθ　 y=b sinθ（θ∈[0，2π）） a为长半轴长 b为短半轴长 θ为参数
///                     双曲线的参数方程 x=a secθ （正割） y=b tanθ a为实半轴长 b为虚半轴长 θ为参数    secθ （正割）即1/cosθ 
///                     
/// </summary>
public class MotionOfAnObject : MonoBehaviour
{
    [Tooltip("要移动的物体")]
    public GameObject go;

    [Tooltip("长轴长")]
    public float Ellipse_a;

    [Tooltip("短轴长")]
    public float Ellipse_b;

    [Tooltip("角度")]
    float angle;

    [Tooltip("半径")]
    public float Circular_R;

    [Tooltip("原点")]
    public GameObject Point;

    [Tooltip("双曲线实轴")]
    public float Hyperbola_a;

    [Tooltip("双曲线虚轴")]
    public float Hyperbola_b;

    private void Start()
    {
        go = this.gameObject;
    }

    private void Update()
    {
        //角度
        angle += Time.deltaTime / 50f;

        if (Input.GetKey(KeyCode.A))
        {
            //椭圆运动
            Move(Ellipse_X(Ellipse_a, angle), Ellipse_Y(Ellipse_b, angle));
        }

        if (Input.GetKey(KeyCode.S))
        {
            //圆运动
            Move(Circular_X(0, Circular_R, angle), Circular_Y(0, Circular_R, angle));
        }

        if (Input.GetKey(KeyCode.D))
        {
            //双曲线运动
            Move(Hyperbola_X(Hyperbola_a, angle), Hyperbola_Y(Hyperbola_b, angle));
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void Move(float x, float y)
    {
        go.transform.position = new Vector3(x + Point.transform.position.x, y + Point.transform.position.y, 0);
    }


    /// <summary>
    /// 椭圆x坐标
    /// </summary>
    /// <param name="a">长半轴</param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private float Ellipse_X(float a, float angle)
    {
        return a * Mathf.Cos(angle * Mathf.Rad2Deg);
    }

    /// <summary>
    /// 椭圆y坐标
    /// </summary>
    /// <param name="b">短半轴</param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private float Ellipse_Y(float b, float angle)
    {
        return b * Mathf.Sin(angle * Mathf.Rad2Deg);
    }


    /// <summary>
    /// 圆x坐标
    /// </summary>
    /// <param name="a">圆心x坐标</param>
    /// <param name="r">半径</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    private float Circular_X(float a, float r, float angle)
    {
        return (a + r * Mathf.Cos(angle * Mathf.Rad2Deg));
    }

    /// <summary>
    /// 圆y坐标
    /// </summary>
    /// <param name="b">圆心y坐标</param>
    /// <param name="r">半径</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    private float Circular_Y(float b, float r, float angle)
    {
        return (b + r * Mathf.Sin(angle * Mathf.Rad2Deg));
    }

    /// <summary>
    /// 双曲线x坐标
    /// </summary>
    /// <param name="a">实轴</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    private float Hyperbola_X(float a, float angle)
    {
        return a * 1 / Mathf.Cos(angle * Mathf.Rad2Deg);
    }

    /// <summary>
    /// 双曲线y坐标
    /// </summary>
    /// <param name="a">虚轴</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    private float Hyperbola_Y(float b, float angle)
    {
        return b * Mathf.Tan(angle * Mathf.Rad2Deg);
    }
}
