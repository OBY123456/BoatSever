using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollider : MonoBehaviour
{
    public RopeControl ropeControl;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("Floor") || other.tag.Contains("Cargo"))
        {
            if(!ropeControl.IsGrab)
            {
                ropeControl.IsElongate = false;
                //Debug.Log("Hook Enter");
            }
            
        }

        if (other.tag.Contains("Cargo") && !ropeControl.IsGrab)
        {
            other.transform.GetComponent<Cargo>().Traget = this.transform;
            other.transform.GetComponent<Cargo>().IsFllow = true;
            ropeControl.IsElongate = false;
            //Debug.Log("Hook Enter");
            ropeControl.IsGrab = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Floor"))
        {
            ropeControl.IsElongate = false;
            //Debug.Log("Hook Stay");
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag.Contains("Floor") && !ropeControl.IsShorten)
    //    {
    //        ropeControl.IsElongate = true;
    //    }
    //}
}
