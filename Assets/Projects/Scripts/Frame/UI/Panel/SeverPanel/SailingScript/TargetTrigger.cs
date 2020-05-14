using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TargetName
{
    TargetCenter,
    TargetRight,
    TargetLeft,
}

public class TargetTrigger : MonoBehaviour
{
    private TargetName Current;

    private void Start()
    {
        Current = (TargetName)Enum.Parse(typeof(TargetName), this.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        //要在Collider组件上添加这个tag，不然没有效果
        if(other.tag == "Boat" && other.GetComponentInParent<AutoDrive>().IsAutoDrive)
        {
            switch (Current)
            {
                case TargetName.TargetCenter:
                    other.GetComponentInParent<AutoDrive>().ArriveTarget();
                    break;
                case TargetName.TargetRight:
                    other.GetComponentInParent<AutoDrive>().SetBoatTransform(this.transform, -1);
                    break;
                case TargetName.TargetLeft:
                    other.GetComponentInParent<AutoDrive>().SetBoatTransform(this.transform, 1);
                    break;
                default:
                    break;
            }
            
        }
    }
}
