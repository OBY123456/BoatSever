using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeControl : MonoBehaviour
{
    /// <summary>
    /// 绳子组的总节点
    /// </summary>
    public Transform Rope_Node;

    /// <summary>
    /// 绳子的第一个节点合集
    /// </summary>
    public Transform[] Link0Group;

    /// <summary>
    /// 是否伸长
    /// </summary>
    //[HideInInspector]
    public bool IsElongate;

    /// <summary>
    /// 是否缩短
    /// </summary>
    //[HideInInspector]
    public bool IsShorten;

    /// <summary>
    /// 伸缩速率
    /// </summary>
    private float Rate = 0.08f;

    private float MaxValue = -5.06f;

    private float MinValue = -34.0f;

    /// <summary>
    /// 需要关闭的物体集合
    /// </summary>
    public Transform[] OtherObj;

    /// <summary>
    /// 是否在抓取中
    /// </summary>
    public bool IsGrab;

    public Transform RayPosition;

    public float RayLenth;

    //public bool IsRayOpen;

    public FloorCollider floorCollider;


    //private void Update()
    //{
    //    if(IsRayOpen)
    //    {
    //        return;
    //        Ray ray = new Ray(RayPosition.position, Vector3.down);
    //        RaycastHit hit;
    //        Vector3 vector3 = Vector3.down * RayLenth;
    //        bool IsRaycast = Physics.Raycast(ray, out hit, RayLenth);
    //        Debug.DrawRay(ray.origin, vector3, Color.green);
    //        if (IsRaycast)
    //        {
    //            if(hit.transform.name.Contains("SailingBoat") || hit.transform.name.Contains("Floor"))
    //            {
    //                //IsElongate = false;
    //               // Debug.Log("Floor");
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 伸长
    /// </summary>
    private IEnumerator Elongate()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            
            if (IsElongate)
            {
                float temp = Link0Group[0].localPosition.y - Rate;

                if (temp > MinValue)
                {

                    foreach (Transform item in Link0Group)
                    {
                        item.localPosition = Vector3.up * temp;
                    }
                }

            }
        }
    }

    /// <summary>
    /// 缩短
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shorten()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            
            if (IsShorten)
            {
                float temp = Link0Group[0].localPosition.y + Rate;
      
                if (temp < MaxValue)
                {
      
                    foreach (Transform item in Link0Group)
                    {
                        item.localPosition = Vector3.up * temp;
                    }
                }

            }
        }

    }

    /// <summary>
    /// 开启携程，通过布尔变量控制伸缩的开始和结束
    /// </summary>
    public void ScaleOpen()
    {
        StartCoroutine(Elongate());
        StartCoroutine(Shorten());
        if (OtherObj.Length > 0)
        {
            foreach (Transform item in OtherObj)
            {
                item.gameObject.SetActive(false);
            }
        }

    }

    public void ScaleClose()
    {
        StopAllCoroutines();
        //IsRayOpen = false;
        if (OtherObj.Length > 0)
        {
            foreach (Transform item in OtherObj)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    public void Reset()
    {
        Rope_Node.localEulerAngles = Vector3.zero;
        foreach (Transform item in Link0Group)
        {
            item.localPosition = Vector3.down * 5.06f;
        }

        IsElongate = IsShorten = IsGrab = false;

        if(floorCollider!=null)
        {
            floorCollider.ColliderClose();
        }
    }
}
