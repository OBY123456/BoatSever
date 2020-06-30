using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    public RopeControl ropeControl;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("Floor"))
        {
            ropeControl.IsElongate = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Floor"))
        {
            ropeControl.IsElongate = false;
        }
    }

    public void ColliderOpen()
    {
        transform.GetComponent<BoxCollider>().enabled = true;
    }

    public void ColliderClose()
    {
        transform.GetComponent<BoxCollider>().enabled = false;
    }
}
