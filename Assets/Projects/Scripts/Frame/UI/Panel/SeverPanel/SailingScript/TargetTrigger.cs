using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //要在Collider组件上添加这个tag，不然没有效果
        if(other.tag == "Boat")
        {
            SailingSceneManage.Instance.autoDrive.ArriveTarget();
        }
    }
}
