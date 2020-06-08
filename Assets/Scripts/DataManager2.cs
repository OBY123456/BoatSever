using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 世界坐标的Z轴正方向北向,X轴正方向为东向
/// </summary>
public class DataManager2 : MonoBehaviour
{

    public GameObject origin;
    public GameObject target;

    public GameObject boat;
    public WindZone windzone;

    float delta = 45.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(windzone.gameObject.transform.forward);
        Debug.DrawLine(origin.transform.position , target.transform.position , Color.red);
      
    }


    //相对风向
    public float GetWindRotation ( )
    {
        // 真风 船舶在静止时观测到的风，实际上就是windzone的风向
        // 船风 静止空气于船舶相对和形成的风，风速和航速相同
        // 相对风向 = 真风向量+船风向量
        // Vector3 windZoneDir = windzone.transform.forward;     
        // Vector3 relativeWind = boat.transform.TransformPoint(boat.transform.forward) + windzone.transform.TransformPoint(windZoneDir);//相对风向
        // Debug.Log(relativeWind);
        // float angle = SignedAngleBetween(boat.transform.TransformPoint(boat.transform.forward) ,windZoneDir,Vector3.up);    
        //return angle;
        Vector3 worldBoatDir = boat.transform.TransformDirection(boat.transform.forward);
        float delta = 45.0f;//偏移量
        float angle = SignedAngleBetween(worldBoatDir , new Vector3(1 , 0 , 0) , Vector3.up) + delta;
        return angle + delta;

    }


    //https://www.cnblogs.com/chenwz91/p/4603255.html
    /// <summary>
    /// =
    /// </summary>
    /// <param name="from">from:起始向量 这里可以填Boat.tranform.fwd</param>
    /// <param name="to">目标向量 这里直接传入风向量</param>
    /// <param name="n">朝向,绕轴</param>
    /// <returns></returns>
    public float SignedAngleBetween (Vector3 from , Vector3 to , Vector3 n)
    {
        float angle = Vector3.Angle(from , to);
        float sign = Mathf.Sign(Vector3.Dot(n , Vector3.Cross(from , to)));
        float signed_angle = angle * sign;
        return ( signed_angle <= 0 ) ? 360 + signed_angle : signed_angle;
    }



}
